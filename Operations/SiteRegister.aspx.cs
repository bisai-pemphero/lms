using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Site : System.Web.UI.Page
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
            lblSiteCode.Text = id;
            LoadDistrict();
            LoadUser();
            LoadDetails(id);
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

   

    public void LoadDistrict()
    {
        drpDistrict.Items.Clear();
        drpDistrict.Items.Add("");

        string sql = "SELECT DistrictName FROM  Districts ORDER BY DistrictName";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpDistrict.Items.Add(dr.GetString(0));
        }

        dr.Close();
        dr.Dispose();

    }

    public void LoadDetails(string siteId)
    {
        string sql1 = @"Select [District],[PhysicalLocation],[Size],[PreviousOwner],
            [InitialValue], [AmountPaid], [DevelopmentsDone],[DevelopmentCost], [TA],
            [Village],[Region], [Site_Description], [Price_Per_Square_Meter]
            from Sites Where ID = @Id";
        SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
        cmd1.Parameters.AddWithValue("@Id", siteId);
        SqlDataReader dr1 = cmd1.ExecuteReader();
        while (dr1.Read())
        {
            drpDistrict.Text = dr1.GetString(0);
            txtLocation.Text = dr1.GetString(1);
            txtSize.Text = dr1.GetString(2);
            txtPreviousOwner.Text = dr1.GetString(3);
            txtInitialValue.Text = dr1.GetDouble(4).ToString();
            txtAmountPaid.Text = dr1.GetDouble(5).ToString();
            txtDevelopment.Text = dr1.GetString(6);
            txtDevelopmentCost.Text = dr1.GetDouble(7).ToString();
            txtTA.Text = dr1.GetString(8);
            txtVillage.Text = dr1.GetString(9);
            drpRegion.Text = dr1.GetString(10);
            txtDescription.Text = dr1.GetString(11);
            txtPricePerSquareMeters.Text = dr1.GetDouble(12).ToString();
        }

    }
   
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewAllSites.aspx");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {

            if (drpDistrict.Text == "")
            {

                lblError.Text = "Please select district";
                lblSuccess.Text = "";
                drpDistrict.Focus();
                return;

            }

            if (txtLocation.Text == "")
            {
                lblError.Text = "Please Enter location";
                lblSuccess.Text = "";
                txtLocation.Focus();

                return;

            }

            if (txtSize.Text == "")
            {

                lblError.Text = "Please enter land size";
                lblSuccess.Text = "";
                txtSize.Focus();
                return;

            }
            if (txtPreviousOwner.Text == "")
            {
                lblError.Text = "Please enter previous owner";
                lblSuccess.Text = "";
                txtPreviousOwner.Focus();
                return;

            }
            if (txtAmountPaid.Text == "")
            {
                lblError.Text = "Please enter amount paid";
                lblSuccess.Text = "";
                txtAmountPaid.Focus();
                return;

            }
            if (txtPricePerSquareMeters.Text == "")
            {
                lblError.Text = "Please enter Price Per Square Meter";
                lblSuccess.Text = "";
                txtPricePerSquareMeters.Focus();
                return;

            }
            if (txtDescription.Text == "")
            {
                lblError.Text = "Please enter what better describes the site!";
                lblSuccess.Text = "";
                txtDescription.Focus();
                return;

            }

            if (drpOwner.Text == "")
            {
                lblError.Text = "Please Select Site Owner!";
                lblSuccess.Text = "";
                drpOwner.Focus();
                return;
            }

            if (txtInitialValue.Text == "") txtInitialValue.Text = "0";
                if (txtDevelopmentCost.Text == "") txtDevelopmentCost.Text = "0";

                double InitialValue = Convert.ToDouble(txtInitialValue.Text.Trim());
                double AmountPaid = Convert.ToDouble(txtAmountPaid.Text.Trim());
                double Balance = InitialValue - AmountPaid;
                double DevCost = Convert.ToDouble(txtDevelopmentCost.Text.Trim());
                double SquareMeterPrice = Convert.ToDouble(txtPricePerSquareMeters.Text.Trim());

                //insert into sites register
                string sql = @"Update Sites set District = @district, PhysicalLocation = @location, 
                Size = @size, PreviousOwner =@owner, InitialValue= @value, AmountPaid =@paid, 
                Balance =@balance, DevelopmentsDone = @development, DevelopmentCost = @devCost, 
                TA = @ta, Village = @village, Region = @region, Site_Description = @description,
                Price_Per_Square_Meter = @squareMeterPrice, [BankName] = @bank, [AccountName] = @accountName,
                [AccountNumber] = @accountNumber, [Branch] = @branch,  [OwnedbyInnobuild] = @condition
                where ID = @Id";
                SqlCommand cmd = new SqlCommand(sql, appconSQL2);
               
                cmd.Parameters.AddWithValue("@district", drpDistrict.Text.Trim());
                cmd.Parameters.AddWithValue("@location", txtLocation.Text.Trim());
                cmd.Parameters.AddWithValue("@size", txtSize.Text.Trim());
                cmd.Parameters.AddWithValue("@owner", txtPreviousOwner.Text.Trim());
                cmd.Parameters.AddWithValue("@value", InitialValue);
                cmd.Parameters.AddWithValue("@paid", AmountPaid);
                cmd.Parameters.AddWithValue("@balance", Balance);
                cmd.Parameters.AddWithValue("@development", txtDevelopment.Text.Trim());
                cmd.Parameters.AddWithValue("@devCost", DevCost);
                cmd.Parameters.AddWithValue("@ta", txtTA.Text.Trim());
                cmd.Parameters.AddWithValue("@village", txtVillage.Text.Trim());
                cmd.Parameters.AddWithValue("@region", drpRegion.Text.Trim());
                cmd.Parameters.AddWithValue("@Id", lblSiteCode.Text.Trim());
                cmd.Parameters.AddWithValue("@description", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@squareMeterPrice", SquareMeterPrice);

            cmd.Parameters.AddWithValue("@bank", txtBank.Text.Trim());
            cmd.Parameters.AddWithValue("@accountName", txtAccountName.Text.Trim());
            cmd.Parameters.AddWithValue("@accountNumber", txtAccountNumber.Text.Trim());
            cmd.Parameters.AddWithValue("@branch", txtBranch.Text.Trim());
            cmd.Parameters.AddWithValue("@condition", drpOwner.SelectedValue.Trim());

            cmd.ExecuteNonQuery();
                cmd.Dispose();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#UpdateModal').modal('show');", true);  
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string sqlDelete = @"Delete from Sites where ID = @Id";
        using (SqlCommand cmdDelete = new SqlCommand(sqlDelete, appconSQL2))
        {
            cmdDelete.Parameters.AddWithValue("@Id", lblSiteCode.Text.Trim());
            cmdDelete.ExecuteNonQuery();
        }

        // Show success modal
        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#DeleteModal').modal('show');", true);

    }
}