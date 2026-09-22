using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dashboard : System.Web.UI.Page
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

        //if (Session["USER"] != null) lblSession.Text = Session["USER"].ToString();
        //else
        //{
        //    this.Response.Redirect("../UserLogin.aspx");
        //    return;
        //}

     
    }


//    public void RevenueOverview()
//    {

//        string sql = @"SELECT 
//	sum(P.MonthlyInstallment )
   
//FROM Plots P
//INNER JOIN Clients C
//    ON P.OfferedTo = C.ClientNo
//INNER JOIN Penalties Pen
//    ON P.MonthlyInstallment BETWEEN Pen.FromRange AND Pen.ToRange
//WHERE P.OfferPeriod > 12 and P.Balance > 0
//AND NOT EXISTS
//(
//    SELECT 1
//    FROM PlotPayments PP
//    WHERE PP.PlotNo = P.PlotNo
//    AND PP.SiteNo = P.SiteNo
//    AND MONTH(PP.DatePaid) = MONTH(GETDATE())
//    AND YEAR(PP.DatePaid) = YEAR(GETDATE()))";
//        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
//        SqlDataReader dr = cmd.ExecuteReader();
//        while (dr.Read())
//        {
//            lblPendingPayments.Text = "K" + dr.GetDouble(0).ToString("N2");
//        }

//        dr.Close();
//        dr.Dispose();


//        string sql3 = @"Select Sum(AmountPaid) as TotalCollected from PlotPayments
//        Where MONTH(DatePaid) = MONTH(GETDATE())
//        AND YEAR(DatePaid) = YEAR(GETDATE())";
//        SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
//        SqlDataReader dr3 = cmd3.ExecuteReader();
//        while (dr3.Read())
//        {
//            lblTotalSales.Text = "K" + dr3.GetDouble(0).ToString("N2");
//        }

//        dr3.Close();
//        dr3.Dispose();

//        string sql1 = @"Select Sum([PlotValue]) Total from Plots where PlotStatus != 'Completed'";
//        SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
//        SqlDataReader dr1 = cmd1.ExecuteReader();
//        while (dr1.Read())
//        {
//            lblPlotStockValue.Text = "K" + dr1.GetDouble(0).ToString("N2");
//        }

//        dr1.Close();
//        dr1.Dispose();

//    }
}