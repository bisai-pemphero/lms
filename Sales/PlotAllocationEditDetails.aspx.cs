using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Customers : System.Web.UI.Page
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

        if (Session["USER"] != null) lblSession.Text = Session["USER"].ToString();
        else
        {
            this.Response.Redirect("../UserLogin.aspx");
            return;
        }

        if (!IsPostBack)
        {
            LoadUser();

            string id = Request.QueryString["id"];

            lblPlotNo.Text = id;

            LoadPlotDetails();

        }
    }
   
    public void LoadUser()
    {
        string sql = "select Fullname, Location from Users where Username = @username";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@username", lblSession.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblUser.Text = dr.GetString(0).ToString();
            lblDutyStation.Text = dr.GetString(1).ToString();
        }

        dr.Close();
        dr.Dispose();
    }

    public void LoadPlotDetails()
    {
        string sql = @"Select P.[PlotSize], P.[PlotValue], P.[PriceCategory], P.[AgreedPrice], 
                      C.Fullname, P.[AmountPaid], P.[MonthlyInstallment], P.[OfferPeriod] from Plots as P
                        Join Clients as C on P.[OfferedTo] = C.[ClientNo]
                        where P.PlotNo = @plotNo";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@plotNo", lblPlotNo.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
           lblPlotSize.Text = dr.GetString(0).ToString();
           lblSellingPrice.Text = dr.GetDouble(1).ToString("N2");
            drpCategory.Text = dr.GetString(2).ToString();
            txtAgreedPrice.Text = dr.GetDouble(3).ToString();
            lblClientName.Text = dr.GetString(4).ToString();

            double paid = dr.IsDBNull(5) ? 0 : dr.GetDouble(5);
            lblPaid.Text = paid.ToString();

            txtInstallment.Text = dr.IsDBNull(6) ? "" : dr.GetDouble(6).ToString();
            txtPeriod.Text = dr.GetString(7);
        }

        dr.Close();
        dr.Dispose();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            if (txtAgreedPrice.Text == "0")
            {
                lblError.Text = "Please specify the plot price";
                lblSuccess.Text = "";
                txtAgreedPrice.Focus();
                return;
            }
           
            string PlotNo = lblPlotNo.Text;
            string siteCode = lblSiteCode.Text;
            string ClientNo = lblClientCode.Text;
            

            double Value = Convert.ToDouble(lblSellingPrice.Text.Trim());
            double NormalPrice = Convert.ToDouble(lblSellingPrice.Text.Trim());
            double PromotionPrice = Convert.ToDouble(txtAgreedPrice.Text.Trim());
            double AgreedPrice = Convert.ToDouble(txtAgreedPrice.Text.Trim());
            double balance = AgreedPrice - (Convert.ToDouble(lblPaid.Text.Trim()));

            double installment = Convert.ToDouble(txtInstallment.Text.Trim());

            //Update Plots
            string sql3 = @"Update Plots set PriceCategory = @category, AgreedPrice = @agreedPrice,
                            [Balance] = @balance, [OfferPeriod] = @period, 
                            [MonthlyInstallment] = @installment  where PlotNo = @plotNo";

            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd3.Parameters.AddWithValue("@category", drpCategory.Text.Trim());
            cmd3.Parameters.AddWithValue("@agreedPrice", AgreedPrice);
            cmd3.Parameters.AddWithValue("@balance", balance);
            cmd3.Parameters.AddWithValue("@period", txtPeriod.Text.Trim());
            cmd3.Parameters.AddWithValue("@installment", installment);
            cmd3.ExecuteNonQuery();
            cmd3.Dispose();

            //Update Plot allocations
            string sql2 = @"Update PlotAllocations set PriceCategory = @category,
                           AgreedPrice =@agreedPrice where PlotNo =@plotNo";
            SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
            cmd2.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd2.Parameters.AddWithValue("@category", drpCategory.Text.Trim());
            cmd2.Parameters.AddWithValue("@agreedPrice", AgreedPrice);
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();

            Response.Redirect("OfferLetter.aspx?id=" + PlotNo);
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }


    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("PlotAllocation.aspx");
    }
}