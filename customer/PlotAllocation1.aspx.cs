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

            LoadPlotDetails();

            LoadOwner();

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
        string sql = @"Select [PlotSize], [PlotValue] from Plots where PlotNo = @plotNo";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@plotNo", lblPlotNo.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
           lblPlotSize.Text = dr.GetString(0).ToString();
           lblSellingPrice.Text = dr.GetDouble(1).ToString("N2");
        }

        dr.Close();
        dr.Dispose();
    }

    public void LoadOwner()
    {
        string sql3 = @"Select O.[OwnedbyInnobuild] from Sites as O
        Join Plots as P on O.SiteCode = P.SiteNo  Where P.PlotNo = plotNo";
        SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
        cmd3.Parameters.AddWithValue("@plotNo", lblPlotNo.Text.Trim());
        SqlDataReader dr3 = cmd3.ExecuteReader();
        while (dr3.Read())
        {
           lblOwner.Text = dr3.GetString(0).ToString();
        }

        dr3.Close();
        dr3.Dispose();
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

            //insert into allocation
            string sql2 = @"INSERT INTO PlotAllocations(PlotNo, SiteNo, PriceCategory, AgreedPrice,
                            Amountpaid, ClientNo, DateAllocated, PostedBy)
                            VALUES(@plotNo, @siteNo, @category, @agreedPrice,
                            @paid, @clientNo, GETDATE(), @user)";

            SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
            cmd2.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd2.Parameters.AddWithValue("@siteNo", siteCode);
            cmd2.Parameters.AddWithValue("@category", drpCategory.Text.Trim());
            cmd2.Parameters.AddWithValue("@agreedPrice", AgreedPrice);
            cmd2.Parameters.AddWithValue("@paid", 0);
            cmd2.Parameters.AddWithValue("@clientNo", ClientNo);
            cmd2.Parameters.AddWithValue("@user", lblSession.Text.Trim());
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();


            string sql3 = @"Update Plots set PriceCategory = @category, PlotStatus = @status,
                          OfferedTo = @offeredTo,  DateOffered = GETDATE(), AgreedPrice = @agreedPrice, 
                            Balance = @agreedPrice where PlotNo = @plotNo";

            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd3.Parameters.AddWithValue("@category", drpCategory.Text.Trim());
            cmd3.Parameters.AddWithValue("@agreedPrice", AgreedPrice);
            cmd3.Parameters.AddWithValue("@status", "Pending");
            cmd3.Parameters.AddWithValue("@offeredTo", ClientNo);
            cmd3.ExecuteNonQuery();
            cmd3.Dispose();

            //choosing offer letter
            if(lblOwner.Text == "True")
            {
                Response.Redirect("OfferLetter.aspx?id=" + PlotNo);
            }
            else
            {
                Response.Redirect("OfferLetter2.aspx?id=" + PlotNo);
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }

   public void Reset()
    {
        //
    }

   
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("PlotAllocation.aspx");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
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
         
            string PlotNo = "plot number";
            string siteCode = string.Empty;
            string ClientNo = string.Empty;
            string Fullname = string.Empty;

            double AgreedPrice = Convert.ToDouble(txtAgreedPrice.Text.Trim());


            //insert into allocation
            string sql2 = @"Update PlotAllocations set PriceCategory = @category,
                            CommitmentPeriod = @commPeriod, AgreedPrice =@agreedPrice, 
                            ClientNo =@clientNo, ClientFullname = @fullname, 
                            ContractDuration =@duration where PlotNo =@plotNo";
            SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
            cmd2.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd2.Parameters.AddWithValue("@category", drpCategory.Text.Trim());
          
            cmd2.Parameters.AddWithValue("@agreedPrice", AgreedPrice);
            cmd2.Parameters.AddWithValue("@clientNo", ClientNo);
            cmd2.Parameters.AddWithValue("@fullname", Fullname);
          
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();

            Reset();

            //replace with redirect to offer letter
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#UpdateModal').modal('show');", true);

        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string PlotNo = "plot number";
            string siteCode = string.Empty;
            string ClientNo = string.Empty;
            string Fullname = string.Empty;

            // Delete customer
            string sqlDelete = @"DELETE FROM PlotAllocations WHERE PlotNo = @plotNo";
            using (SqlCommand cmdDelete = new SqlCommand(sqlDelete, appconSQL2))
            {
                cmdDelete.Parameters.AddWithValue("@plotNo", PlotNo);
                cmdDelete.ExecuteNonQuery();
            }

            //update plots
            string sql = @"UPDATE Plots SET PlotStatus = @status, OfferedTo= @offeredTo,   
            CommitmentPeriod= @commitment, SoldTo= @soldTo, AgreedPrice=@agreedPrice,
            Balance= @balance, DateOffered = NULL, OfferExpiryDate = NULL WHERE PlotNo = @plotNo";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@status", "Available");
            cmd.Parameters.AddWithValue("@offeredTo", "none");
            cmd.Parameters.AddWithValue("@commitment", 0);
            cmd.Parameters.AddWithValue("@soldTo", "none");
            cmd.Parameters.AddWithValue("@agreedPrice", 0);
            cmd.Parameters.AddWithValue("@paid", 0);
            cmd.Parameters.AddWithValue("@balance", 0);
            cmd.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            Reset();

            // Show success modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#DeleteModal').modal('show');", true);

        }
        catch (Exception ex)
        {
            lblError.Text = "There was an error: " + ex.Message;
        }
        finally
        {
            //Ensure the
        }
    }
 
}