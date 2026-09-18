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
            LoadCustomers();
            LoadSites();
            LoadUpsert();
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

    public void LoadSites()
    {
        drpSite.Items.Clear();
        drpSite.Items.Add("");
        string sql = @"Select SiteCode, PhysicalLocation from Sites Where District = @location
                    Order by PhysicalLocation";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@location", lblDutyStation.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpSite.Items.Add(dr.GetString(0) + "|" + dr.GetString(1));
        }

        dr.Close();

    }

    public void LoadCustomers()
    {
        drpClient.Items.Clear();
        drpClient.Items.Add("");
        string sql = @"Select C.ClientNo, C.Fullname from Clients as C Join Users as U
        on U.Username = C.PostedBy  Where U.Location = @location";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@location", lblDutyStation.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpClient.Items.Add(dr.GetString(0) + "|" + dr.GetString(1));
        }

        dr.Close();
        dr.Dispose();

    }

    public void LoadUpsert()
    {
        drpUpsert.Items.Clear();
        drpUpsert.Items.Add("");
        drpUpsert.Items.Add("ADD NEW");

        string sql = @"	  Select P.PlotNo, P.SoldTo from Plots as P 
	                Join Sites as S on P.SiteNo = S.SiteCode
	                where S.District = @district and P.PlotStatus != 'Available' order by P.SoldTo ASC";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@district", lblDutyStation.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpUpsert.Items.Add(dr.GetString(0) + "|" + dr.GetString(1));
        }

        dr.Close();
        dr.Dispose();
    }
    protected void drpUpsert_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblSuccess.Text = "";

        if (drpUpsert.Text == "")
        {
           // Reset();
            btnSave.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

        }
        else if (drpUpsert.Text == "ADD NEW")
        {
            //Reset();
            drpSite.Focus();
            btnSave.Enabled = true; ;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

        }
        else
        {
            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            drpPlot.Enabled = false ;
            drpSite.Enabled = false;
            txtSize.Visible = false;
            txtSellingPrice.Visible = false;

            string[] offerLetter = drpUpsert.Text.Split('|');
            string offerId = offerLetter[0];  

            //display details
            string sql1 = @"Select AgreedPrice, CommitmentPeriod, ContractDuration, ClientNo,
                            ClientFullname, PriceCategory  from PlotAllocations where PlotNo = @plotNo";
            SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
            cmd1.Parameters.AddWithValue("@plotNo", offerId);
            SqlDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                txtAgreedPrice.Text = dr1.GetDouble(0).ToString();
                txtCommitmentPeriod.Text = dr1.GetString(1);
                txtContractDuration.Text = dr1.GetString(2);
                drpClient.Text = dr1.GetString(3) + "|" + dr1.GetString(4);
                drpCategory.Text = dr1.GetString(5);
            }
            txtAgreedPrice.Focus();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (drpPlot.Text == "")
            {
                lblError.Text = "Please select plot";
                lblSuccess.Text = "";
                drpPlot.Focus();
                return;

            }

            if (txtAgreedPrice.Text == "0")
            {
                lblError.Text = "Please specify the plot price";
                lblSuccess.Text = "";
                txtAgreedPrice.Focus();
                return;
            }
            if (drpClient.Text == "")
            {
                lblError.Text = "Please select client";
                lblSuccess.Text = "";
                drpClient.Focus();
                return;
            }
            if (txtCommitmentPeriod.Text == "")
            {
                lblError.Text = "Please specify the period which the client is willing to make initial payment!";
                lblSuccess.Text = "";
                txtCommitmentPeriod.Focus();    
                return;
            }
            if (txtContractDuration.Text == "")
            {
                lblError.Text = "Please specify the duration of this purchase in Months!";
                lblSuccess.Text = "";
                txtContractDuration.Focus();
                return;
            }


            string PlotNo = drpPlot.Text.Trim();
            string[] site = drpSite.Text.Split('|');
            string siteCode = site[0];

            string[] Customer = drpClient.Text.Split('|');
            string ClientNo = Customer[0];
            string Fullname = Customer[1];


            double Value = Convert.ToDouble(txtSellingPrice.Text.Trim());
            double NormalPrice = Convert.ToDouble(txtSellingPrice.Text.Trim());
            double PromotionPrice = Convert.ToDouble(txtAgreedPrice.Text.Trim());
            double AgreedPrice = Convert.ToDouble(txtAgreedPrice.Text.Trim());

            int ComPeriod = Convert.ToInt32(txtCommitmentPeriod.Text.Trim());
            string ContractDuration = txtContractDuration.Text.Trim();

            //insert into allocation
            string sql2 = @"INSERT INTO PlotAllocations(PlotNo, SiteNo, PriceCategory,
                            CommitmentPeriod, AgreedPrice, Amountpaid, ClientNo,
                            ClientFullname, DateAllocated, PostedBy,  ContractDuration)
                            VALUES(@plotNo, @siteNo, @category, @commPeriod, @agreedPrice,
                            @paid, @clientNo, @fullname, GETDATE(), @user, @duration) ";
            SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
            cmd2.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd2.Parameters.AddWithValue("@siteNo", siteCode);
            cmd2.Parameters.AddWithValue("@category", drpCategory.Text.Trim());
            cmd2.Parameters.AddWithValue("@commPeriod", ComPeriod);
            cmd2.Parameters.AddWithValue("@agreedPrice", AgreedPrice);
            cmd2.Parameters.AddWithValue("@paid", 0);
            cmd2.Parameters.AddWithValue("@clientNo", ClientNo);
            cmd2.Parameters.AddWithValue("@fullname", Fullname);
            cmd2.Parameters.AddWithValue("@user", lblSession.Text.Trim());
            cmd2.Parameters.AddWithValue("@duration", ContractDuration);
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();

            
            //update plots
            string sql = "UPDATE Plots SET OfferedTo='" + ClientNo + "', DateOffered=GETDATE(), OfferExpiryDate=DATEADD(day, " + ComPeriod + ", GETDATE()),   CommitmentPeriod='" + ComPeriod + "', SoldTo='" + Fullname + "', AgreedPrice='" + AgreedPrice + "', Balance='" + AgreedPrice + "', PlotStatus='Pending' WHERE PlotNo ='" + PlotNo + "'";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            //string[] Location = drpSite.Text.Trim().Split('|');

            //string Reference = Location[0] + "|" + PlotNo + "|" + Location[1];

            Reset();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#successModal').modal('show');", true);
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
            if (drpUpsert.Text == "")
            {
                lblError.Text = "Please select plot to Edit";
                lblSuccess.Text = "";
                drpUpsert.Focus();
                return;

            }

            if (txtAgreedPrice.Text == "0")
            {
                lblError.Text = "Please specify the plot price";
                lblSuccess.Text = "";
                txtAgreedPrice.Focus();
                return;
            }
            if (drpClient.Text == "")
            {
                lblError.Text = "Please select client";
                lblSuccess.Text = "";
                drpClient.Focus();
                return;
            }
            if (txtCommitmentPeriod.Text == "")
            {
                lblError.Text = "Please specify the period which the client is willing to make initial payment!";
                lblSuccess.Text = "";
                txtCommitmentPeriod.Focus();
                return;
            }
            if (txtContractDuration.Text == "")
            {
                lblError.Text = "Please specify the duration of this purchase in Months!";
                lblSuccess.Text = "";
                txtContractDuration.Focus();
                return;
            }

            string[] customer = drpClient.Text.Split('|');
            string ClientNo = customer[0];
            string Fullname = customer[1];

            string[] plot = drpUpsert.Text.Split('|');
            string PlotNo = plot[0];

            double AgreedPrice = Convert.ToDouble(txtAgreedPrice.Text.Trim());

            int ComPeriod = Convert.ToInt32(txtCommitmentPeriod.Text.Trim());
            string ContractDuration = txtContractDuration.Text.Trim();

            //insert into allocation
            string sql2 = @"Update PlotAllocations set PriceCategory = @category,
                            CommitmentPeriod = @commPeriod, AgreedPrice =@agreedPrice, 
                            ClientNo =@clientNo, ClientFullname = @fullname, 
                            ContractDuration =@duration where PlotNo =@plotNo";
            SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
            cmd2.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd2.Parameters.AddWithValue("@category", drpCategory.Text.Trim());
            cmd2.Parameters.AddWithValue("@commPeriod", ComPeriod);
            cmd2.Parameters.AddWithValue("@agreedPrice", AgreedPrice);
            cmd2.Parameters.AddWithValue("@clientNo", ClientNo);
            cmd2.Parameters.AddWithValue("@fullname", Fullname);
            cmd2.Parameters.AddWithValue("@duration", ContractDuration);
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();


            //update plots
            string sql = @"UPDATE Plots SET OfferedTo='" + ClientNo + "', OfferExpiryDate=DATEADD(day, " + ComPeriod + ", GETDATE()),   CommitmentPeriod='" + ComPeriod + "', SoldTo='" + Fullname + "', AgreedPrice='" + AgreedPrice + "', Balance='" + AgreedPrice + "' WHERE PlotNo ='" + PlotNo + "'";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

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
            if (drpUpsert.SelectedIndex == 0)
            {
                lblError.Text = "Please select Action";
                lblSuccess.Text = "";
                drpUpsert.Focus();
                return;
            }

            string[] plot = drpUpsert.Text.Split('|');
            string PlotNo = plot[0];

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
            // Ensure the
        }
    }


    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        string sql = @"	Select P.PlotNo, P.SoldTo from Plots as P 
	                Join Sites as S on P.SiteNo = S.SiteCode
	                where S.District = @district and P.PlotStatus != 'Available' and P.SoldTo like '%"+txtSearch.Text.Trim()+"%'";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@district", lblDutyStation.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpUpsert.Text = dr.GetString(0) + "|" + dr.GetString(1);
        }

        dr.Close();
        dr.Dispose();
    }

    public void LoadPlot(string SiteNo)
    {
        drpPlot.Items.Clear();
        drpPlot.Items.Add("");

        string sql = "SELECT PlotNo  FROM   Plots  WHERE SiteNo='" + SiteNo + "' AND PlotStatus='Available'";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpPlot.Items.Add(dr.GetString(0));
        }

        dr.Close();
        dr.Dispose();

    }
    protected void drpSite_SelectedIndexChanged(object sender, EventArgs e)
    {
        //load plots
        string[] sites = drpSite.Text.Split('|');
        string siteCode = sites[0];

        LoadPlot(siteCode);
    }

    protected void drpCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string plotNo = drpPlot.Text.Trim();

        string sql = @"Select PlotSize, NormalPrice from Plots where PlotNo = @plotNo";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@plotNo", plotNo);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            txtSize.Text = dr.GetString(0);
            txtSellingPrice.Text = dr.GetDouble(1).ToString("N2");
        }

        dr.Close();
        dr.Dispose();

        txtAgreedPrice.Focus();
    }
}