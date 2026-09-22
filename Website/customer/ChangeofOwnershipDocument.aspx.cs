using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Test : System.Web.UI.Page
{
    private long number;

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

        LoadProfile(id);
    }
  protected void btnPrint_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterClientScriptBlock(this.GetType(), "key", "window.print()", true);
    }

    protected void Button1_Click1(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard.aspx");
    }

    public void LoadProfile(string proof)
    {
        try
        {
            string sql = @"Select C.[Fullname], S.[PhysicalLocation], 
                O.[Plot_Number], P.[PlotSize], OP.[AmountPaid], OP.[PaymentMode], 
                O.[ProofofPayment] from [ChangeofOwnership]
                 as O join [Clients] as C on O.[Previous_Owner] = C.ClientNo
                 join Sites as S on O.Site_Number = S.SiteCode 
                join [ChangeofOwershipPayments] as OP on O.Plot_Number = OP.PlotNo 
                join Plots as P on O.Plot_Number = P.PlotNo
                where O.[ProofofPayment] = @proof";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@proof", proof);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lblCustomer1.Text = dr.GetString(0);
                lblSiteName.Text = dr.GetString(1);
                lblPlotNo.Text = dr.GetString(2);
                lblPlotSize.Text = dr.GetString(3) + " Square Meters";
                lblFee.Text = "MK" + dr.GetDouble(4).ToString("N2");
                lblPaymentMode.Text = dr.GetString(5);
                lblProofofPayment.Text = dr.GetString(6);
               
                lblPrevCustomer.Text = dr.GetString(0);
            }

            dr.Close();
            dr.Dispose();

            //Get New Client Name
            string sql2 = "Select C.Fullname  from Clients as C Join [ChangeofOwnership] as O on C.ClientNo = O.Current_Owner where O.[ProofofPayment] = @proof";
            SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
            cmd2.Parameters.AddWithValue("@proof", proof);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                lblNewClient.Text = dr2.GetString(0);
                lblNewCustomer.Text = dr2.GetString(0);
            }

            dr2.Close();
            dr2.Dispose();

            //Get prepared by name and office
            string sql3 = " Select U.Fullname, U.Location from Users as U join [ChangeofOwershipPayments] as O on U.Username = O.PostedBy where O.[ReceiptNo] = @proof";
            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@proof", proof);
            SqlDataReader dr3 = cmd3.ExecuteReader();
            while (dr3.Read())
            {
                lblPreparedby.Text = dr3.GetString(0) + " at " + dr3.GetString(1) + " Office";
            }

            dr3.Close();
            dr3.Dispose();
        }
        catch (Exception ex)
        {
            lblError.Text = "There was an Error!" + ex;
            //Response.Redirect("SalesDashboard.aspx");
        }

    }

}