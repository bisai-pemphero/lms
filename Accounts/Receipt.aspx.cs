using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        LoadReceiptDetails(id);

        lblReceipNo.Text = id;

    }



    private void LoadReceiptDetails(string receiptNo)
    {
        try
        {
            string sql = @"SELECT  P.[PlotNo], P.[AmountPaid], P.[NewBalance], C.[Fullname], 
                            P.[PaymentMode], P.[DatePaid], U.[Fullname],  U.Location, 
                            S.[PhysicalLocation], PP.AgreedPrice, PP.AmountPaid, C.Address
                            FROM [PlotPayments] as P join [Clients] as C
                            on P.PaidBy = C.ClientNo join Users as U on U.Username= P.PostedBy
                            join Sites as S on P.SiteNo=S.SiteCode
							Join Plots as PP on P.PlotNo = PP.PlotNo where P.[ReceiptNo]  = @receiptNo";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@receiptNo", receiptNo);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lblPlotNo.Text = dr.GetString(0);
                lblPaid.Text = "MK" +  dr.GetDouble(1).ToString("N2");
                lblBalance.Text = "MK" + dr.GetDouble(2).ToString("N2");
                lblCustomer.Text = dr.GetString(3);
                lblMode.Text = dr.GetString(4);
                lblDate.Text = dr.GetDateTime(5).ToString();
                lblUser.Text = dr.GetString(6);
                lblBranch.Text = dr.GetString(7) + " Office";
                lblSite.Text = dr.GetString(8);
                lblPlotValue.Text = "MK" + dr.GetDouble(9).ToString("N2");
                lblTotalPaid.Text = "MK" + dr.GetDouble(10).ToString("N2");
                lblAdress.Text = dr.GetString(11);
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