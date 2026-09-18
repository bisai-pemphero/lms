using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

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
            string id = Request.QueryString["id"];
            lblPlotNo.Text = id;


            string[] Site = lblPlotNo.Text.Split('/');

            string siteNo = Site[0];

            lblSiteNo.Text = siteNo;


            LoadUser();
            LoadClient();
        }
    }
   
    public void LoadUser()
    {
        string sql = "select Fullname from Users where Username = @username";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@username", lblSession.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblUser.Text = dr.GetString(0).ToString();
           
        }

        dr.Close();
        dr.Dispose();
    }

    public void LoadClient()
    {
        string sql = " Select OfferedTo from Plots where PlotNo = @plotNo";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@plotNo", lblPlotNo.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblClient.Text = dr.GetString(0).ToString();

        }

        dr.Close();
        dr.Dispose();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string plotNo = lblPlotNo.Text.Trim();

            string siteNo = lblSiteNo.Text.Trim();

            //Check if already exist
            string sql3 = "SELECT * FROM  SaleAgreements WHERE   PlotNo=@plotNo";
            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@plotNo", plotNo);
          

            SqlDataReader dr3 = cmd3.ExecuteReader();
            if (dr3.HasRows)
            {
                lblError.Text = "Sale Agreement already produced! Go to Documents to Reprint the Document!";
                return;
            }
            else
            {
                dr3.Close();
                dr3.Dispose();


                string year = DateTime.Now.ToString("yyyy");
                string date = DateTime.Now.ToString("dddd, dd MMMM yyyy");

                // Insert into Sale Agreements register
                string sql = "INSERT INTO [SaleAgreements] ([PlotNo], [SiteNo], [ClientNo], [OfferDate], [OfferYear]) " +
                             "VALUES (@plotNo, @siteNo, @client, @date, @year)";
                using (SqlCommand cmd = new SqlCommand(sql, appconSQL2))
                {
                    cmd.Parameters.AddWithValue("@plotNo", plotNo);
                    cmd.Parameters.AddWithValue("@siteNo", siteNo);
                    cmd.Parameters.AddWithValue("@client", lblClient.Text.Trim());
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.ExecuteNonQuery();
                }

              
                Response.Redirect("SaleAgreement.aspx?id=" + plotNo);

            }
        }
        catch (Exception ex)
        {
            lblError.Text = "There was an error" + ex;
        }

    }
}