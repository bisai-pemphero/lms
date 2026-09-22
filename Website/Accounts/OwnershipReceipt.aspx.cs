using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Test : System.Web.UI.Page
{
    string appconStr;
    string server, appdb, user, password, version;
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

        LoadSaleAgreementNumber(id);

        LoadReceiptDetails(id);
       
        lblReceipNo.Text = id;

    }

    private void LoadSaleAgreementNumber(string receiptNo)
    {
        try
        {
            string sql3 = @"Select [SaleAgreementId] from ChangeofOwershipPayments 
            where ReceiptNo = @receiptNo";
            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@receiptNo", receiptNo);
    
            SqlDataReader dr3 = cmd3.ExecuteReader();
            while (dr3.Read())
            {
                lblSaleAgreementId.Text = dr3.GetString(0);
            }
            dr3.Close();
            dr3.Dispose();
        }
        catch (Exception ex)
        {
            lblError.Text = "An error occured!" + ex;
        }
    }


    private void LoadReceiptDetails(string receiptNo)
    {
        try
        {
            string sql = @"Select  STUFF((
    SELECT ', ' + P2.PlotNo
    FROM SaleAgreementPlots P2
    WHERE P2.SaleAgreementId = @saId
     
    FOR XML PATH(''), TYPE
).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS PlotNumbers, COP.[AmountPaid], C.Fullname,  COP.[PaymentMode], 
           COP.[DatePaid], U.Fullname, U.Location, S.PhysicalLocation 
            from   [ChangeofOwershipPayments] as COP
			Join SaleAgreementPlots as SA on SA.SaleAgreementId = COP.[SaleAgreementId]
            Join Sites as S on S.SiteCode = SA.SiteNo
            Join Clients as C on C.ClientNo = COP.PaidBy
            Join Users as U on COP.PostedBy = U.Username where COP.[ReceiptNo]  = @receiptNo";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@receiptNo", receiptNo);
            cmd.Parameters.AddWithValue("@saId", lblSaleAgreementId.Text.Trim());
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lblPlotNo.Text = dr.GetString(0);
                lblPaid.Text = "MK" + dr.GetDouble(1).ToString("N2");
                lblCustomer.Text = dr.GetString(2);
                lblMode.Text = dr.GetString(3);
                lblDate.Text = dr.GetString(4).ToString();
                lblUser.Text = dr.GetString(5);
                lblBranch.Text = dr.GetString(6) + " Office";
                lblSite.Text = dr.GetString(7);
            }
            dr.Close();
            dr.Dispose();
        }
        catch (Exception ex)
        {
            lblError.Text = "An error occured!" + ex;
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard.aspx");
    }
}