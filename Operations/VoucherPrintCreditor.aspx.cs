using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Web.UI;

public partial class VoucherPrint : System.Web.UI.Page
{

    string appconStr;
    string server, appdb, user, password, version, station;
    SqlConnection appconSQL2;

    private void readConf()
    {
        System.IO.StreamReader sr;
        {
            sr = System.IO.File.OpenText(Server.MapPath("../dbconn.ini"));


            string s = "";
            string[] rfInfo = new string[2];
            char SplitChar = '=';
            while ((s = sr.ReadLine()) != null)
            {
                if (!(s.Trim() == "") || s.StartsWith("#"))
                {
                    rfInfo = s.Split(SplitChar);
                    switch (rfInfo[0].Trim().ToLower())
                    {
                        case "server":
                            server = rfInfo[1].Trim();
                            break;
                        case "user":
                            user = rfInfo[1].Trim();
                            break;
                        case "password":
                            password = rfInfo[1].Trim();
                            break;
                        case "appdb":
                            appdb = rfInfo[1].Trim();
                            break;
                        case "version":
                            version = rfInfo[1].Trim();
                            break;


                    }
                }
            }
        }
    }

    private void dbconnect()
    {
        appconStr = "Data Source=" + server + ";user id=" + user + ";password=" + password + ";max pool size= 65536;Initial Catalog=" + appdb + ";";
        appconSQL2 = new System.Data.SqlClient.SqlConnection(appconStr);
        appconSQL2.Open();

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        readConf();
        dbconnect();

        string id = Request.QueryString["id"];
        LoadVoucherDetails(id);
    }


    public void LoadVoucherDetails(string id)
    {
        string sql = @"SELECT [PaymentVoucherID], [Status], [ReferenceNo], [DatePrepared], 
        [PayeeName], [PaymentMethod], [ChequeNo], [BankName], [AccountNo], [Amount], 
        [AmountInWords], [Description], [PreparedBy], [DatePrepared], [ApprovedBy], 
        [DateApproved], [ReceivedBy], [DateReceived] from [PaymentVoucherCreditor]  where [PaymentVoucherID] = @Id";

        using (SqlCommand cmd = new SqlCommand(sql, appconSQL2))
        {
            cmd.Parameters.AddWithValue("@Id", id);

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    lblVoucherNo.Text = "0000NB" + (dr["PaymentVoucherID"] != DBNull.Value ? dr["PaymentVoucherID"].ToString() : "-");
                    lblStatus.Text = dr["Status"] != DBNull.Value ? dr["Status"].ToString() : "-";
                    lblReferenceNo.Text = dr["ReferenceNo"] != DBNull.Value ? dr["ReferenceNo"].ToString() : "-";
                    lblDatePrepared.Text = dr["DatePrepared"] != DBNull.Value ? Convert.ToDateTime(dr["DatePrepared"]).ToString("dd/MM/yyyy") : "-";
                    lblPayeeName.Text = dr["PayeeName"] != DBNull.Value ? dr["PayeeName"].ToString() : "-";
                    
                    lblVoucherFooter.Text = "0000NB" + (dr["PaymentVoucherID"] != DBNull.Value ? dr["PaymentVoucherID"].ToString() : "-");
                    lblPaymentMethod.Text = dr["PaymentMethod"] != DBNull.Value ? dr["PaymentMethod"].ToString() : "-";
                    lblChequeNo.Text = dr["ChequeNo"] != DBNull.Value ? dr["ChequeNo"].ToString() : "-";
                    lblBankName.Text = dr["BankName"] != DBNull.Value ? dr["BankName"].ToString() : "-";
                    lblAccountNo.Text = dr["AccountNo"] != DBNull.Value ? dr["AccountNo"].ToString() : "-";
                    
                    lblAmount.InnerText = "MWK " + (dr["Amount"] != DBNull.Value ? string.Format("{0:N0}", Convert.ToDecimal(dr["Amount"])) : "-");
                    lblAmountWords.Text = dr["AmountInWords"] != DBNull.Value ? dr["AmountInWords"].ToString() : "-";
                    lblDescription.Text = dr["Description"] != DBNull.Value ? dr["Description"].ToString() : "-";
                    lblPreparedBy.Text = dr["PreparedBy"] != DBNull.Value ? dr["PreparedBy"].ToString() : "-";
                    
                    lblPreparedDate.Text = dr["DatePrepared"] != DBNull.Value ? Convert.ToDateTime(dr["DatePrepared"]).ToString("dd/MM/yyyy") : "-";
                    lblApprovedBy.Text = dr["ApprovedBy"] != DBNull.Value ? dr["ApprovedBy"].ToString() : "-";
                    lblDateApproved.Text = dr["DateApproved"] != DBNull.Value ? Convert.ToDateTime(dr["DateApproved"]).ToString("dd/MM/yyyy") : "-";
                    lblReceivedBy.Text = dr["ReceivedBy"] != DBNull.Value ? dr["ReceivedBy"].ToString() : "-";
                    lblDateReceived.Text = dr["DateReceived"] != DBNull.Value ? Convert.ToDateTime(dr["DateReceived"]).ToString("dd/MM/yyyy") : "-";
                }
            }
        }
    }


}


