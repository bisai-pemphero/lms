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

            string[] strings = id.Split(new char[] { '|' });

            lblClientCode.Text = strings[0];
            lblPlotNo.Text = strings[1];

            string[] plots = lblPlotNo.Text.Split('/');
            lblSiteCode.Text = plots[0];
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

   

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string NewCustomer = lblClientCode.Text.Trim();
            string PlotNo = lblPlotNo.Text.Trim();

            //insert into Change of Ownership
            string sql = @"Insert into [ChangeofOwnership] ([Previous_Owner], [Current_Owner], [Site_Number],
                        [Plot_Number])
                    select [OfferedTo], @newCustomer, [SiteNo], @plotNo from Plots  where PlotNo = @plotNo";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@newCustomer", NewCustomer);
            cmd.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            //update Plots
            string sql3 = "Update Plots Set [OfferedTo] = @NewCustomerId where PlotNo = @plotNo";
            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@NewCustomerId", NewCustomer);
            cmd3.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd3.ExecuteNonQuery();
            cmd3.Dispose();

            //update plot payments
            string sql2a = " Update [PlotPayments] set PaidBy = @Customer where [PlotNo] = @plotNo";
            SqlCommand cmd2a = new SqlCommand(sql2a, appconSQL2);
            cmd2a.Parameters.AddWithValue("@Customer", NewCustomer);
            cmd2a.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd2a.ExecuteNonQuery();
            cmd2a.Dispose();

            //Update plot allocations
            string sql2 = "Update [PlotAllocations] Set [ClientNo] = @NewCustomerId where PlotNo = @plotNo";
            SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
            cmd2.Parameters.AddWithValue("@NewCustomerId", NewCustomer);
            cmd2.Parameters.AddWithValue("@client", NewCustomer);
            cmd2.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();

            //Update Sale Agreements
            string sql4 = "Update [SaleAgreements] set [ClientNo] = @NewClient where PlotNo = @plotNo";
            SqlCommand cmd4 = new SqlCommand(sql4, appconSQL2);
            cmd4.Parameters.AddWithValue("@NewClient", NewCustomer);
            cmd4.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd4.ExecuteNonQuery();
            cmd4.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#successModal').modal('show');", true);

        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }

  
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("ChangeofOwnership.aspx");
    }

 
}