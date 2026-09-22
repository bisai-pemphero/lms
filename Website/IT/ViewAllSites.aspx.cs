using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewSite : System.Web.UI.Page
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
            LoadDistrict();
            LoadSiteCode();
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
            lblLocation.Text = dr.GetString(1).ToString();
        }

        dr.Close();
        dr.Dispose();
    }

    public void LoadSiteDetails()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["lms"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(@"  Select [ID], [District],[PhysicalLocation],[Size],
            [PreviousOwner],[InitialValue], [AmountPaid],[Balance] from Sites where 
            District = @district", connection);
            command.Parameters.AddWithValue("@district", lblLocation.Text.Trim());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string siteId = reader["ID"].ToString();

                Response.Write("<tr>");
                Response.Write("<td>" + reader["District"] + "</td>");
                Response.Write("<td>" + reader["PhysicalLocation"] + "</td>");
                Response.Write("<td>" + reader["Size"] + "</td>");
                Response.Write("<td>" + reader["PreviousOwner"] + "</td>");
                Response.Write("<td>" + reader["InitialValue"] + "</td>");
                Response.Write("<td>" + reader["AmountPaid"] + "</td>");
                Response.Write("<td>" + reader["Balance"] + "</td>");
                Response.Write("<td>");
                Response.Write("<a href='SiteRegister.aspx?id="+siteId +"' class='btn btn-primary btn-sm'> Approve / Edit</a>");
                Response.Write("</td>");
                Response.Write("</tr>");
            }

            reader.Close();
        }
    }

    



    //load districts
    public void LoadDistrict()
    {
       

        drpNewDistrict.Items.Clear();
        drpNewDistrict.Items.Add("");

        string sql = "SELECT DistrictName FROM  Districts ORDER BY DistrictName";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
          
            drpNewDistrict.Items.Add(dr.GetString(0));
        }

        dr.Close();
        dr.Dispose();

    }

    //load site code
    public void LoadSiteCode()
    {

        string sql = "SELECT   SiteCode + 1 AS Next FROM    ValueSequence";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblSiteCode.Text = dr.GetInt32(0).ToString();
        }

        dr.Close();
        dr.Dispose();

    }
    protected void btnNewSave_Click(object sender, EventArgs e)
    {
        //saving the new modal
        try
        {

            if (drpNewDistrict.Text == "")
            {

                lblError.Text = "Please select district";
                lblSuccess.Text = "";
                drpNewDistrict.Focus();
                return;

            }

            if (txtNewLocation.Text == "")
            {
                lblError.Text = "Please Enter location";
                lblSuccess.Text = "";
                txtNewLocation.Focus();

                return;

            }

            if (txtNewLandSite.Text == "")
            {

                lblError.Text = "Please enter land size";
                lblSuccess.Text = "";
                txtNewLandSite.Focus();
                return;

            }
            if (txtNewPrevOwner.Text == "")
            {
                lblError.Text = "Please enter previous owner";
                lblSuccess.Text = "";
                txtNewPrevOwner.Focus();
                return;
            }
            if (txtNewAmountPaid.Text == "")
            {
                lblError.Text = "Please enter amount paid";
                lblSuccess.Text = "";
                txtNewAmountPaid.Focus();
                return;
            }
            if (txtTA.Text == "")
            {
                lblError.Text = "Please enter TA";
                lblSuccess.Text = "";
                txtTA.Focus();
                return;
            }
            if (txtVillage.Text == "")
            {
                lblError.Text = "Please enter Village or Township name";
                lblSuccess.Text = "";
                txtVillage.Focus();
                return;
            }

            string SiteCode = "CRE0" + lblSiteCode.Text.Trim();

            //Check if already exist
            string sql3 = @"SELECT * FROM Sites WHERE SiteCode='" + SiteCode + "'";
            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            SqlDataReader dr3 = cmd3.ExecuteReader();
            if (dr3.HasRows)
            {
                lblError.Text = "This site Code already exists!";
                lblSuccess.Text = "";
                return;
            }
            else
            {
                dr3.Close();
                dr3.Dispose();

                if (txtNewInitialValue.Text == "") txtNewInitialValue.Text = "0";
                if (txtNewDevCosts.Text == "") txtNewDevCosts.Text = "0";

                double InitialValue = Convert.ToDouble(txtNewInitialValue.Text.Trim());
                double AmountPaid = Convert.ToDouble(txtNewAmountPaid.Text.Trim());
                double Balance = InitialValue - AmountPaid;
                double DevCost = Convert.ToDouble(txtNewDevCosts.Text.Trim());

                //insert into sites register
                string sql = @"INSERT INTO  Sites(SiteCode, District, PhysicalLocation, 
            Size, PreviousOwner, InitialValue, AmountPaid, Balance, DevelopmentsDone, 
            DevelopmentCost, DateRegistered, RegisteredBy, TA, Village, Region) 
            VALUES (@siteCode, @district, @location, @size, @owner, @value, @paid, @balance,
            @development, @devCost, GETDATE(), @user, @ta, @village, @region)";
                SqlCommand cmd = new SqlCommand(sql, appconSQL2);
                cmd.Parameters.AddWithValue("@siteCode", SiteCode);
                cmd.Parameters.AddWithValue("@district", drpNewDistrict.Text.Trim());
                cmd.Parameters.AddWithValue("@location", txtNewLocation.Text.Trim());
                cmd.Parameters.AddWithValue("@size", txtNewLandSite.Text.Trim());
                cmd.Parameters.AddWithValue("@owner", txtNewPrevOwner.Text.Trim());
                cmd.Parameters.AddWithValue("@value", InitialValue);
                cmd.Parameters.AddWithValue("@paid", AmountPaid);
                cmd.Parameters.AddWithValue("@balance", Balance);
                cmd.Parameters.AddWithValue("@development", txtNewDevDone.Text.Trim());
                cmd.Parameters.AddWithValue("@devCost", DevCost);
                cmd.Parameters.AddWithValue("@user", lblSession.Text.Trim());
                cmd.Parameters.AddWithValue("@ta", txtTA.Text.Trim());
                cmd.Parameters.AddWithValue("@village", txtVillage.Text.Trim());
                cmd.Parameters.AddWithValue("@region", drpRegion.Text.Trim());
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //update site code in vsequnce
                string sql2 = "UPDATE ValueSequence SET    SiteCode=   SiteCode + 1 WHERE ID=1";
                SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
                cmd2.ExecuteNonQuery();
                cmd2.Dispose();


               Reset();
               ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#successModal').modal('show');", true);
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }

    public void Reset()
    {
        txtNewAmountPaid.Text = string.Empty;
        txtNewDevCosts.Text = string.Empty;
        txtNewDevDone.Text = string.Empty;
        txtNewInitialValue.Text = string.Empty;
        txtNewLandSite.Text = string.Empty;
        txtNewLocation.Text = string.Empty;
        txtNewPrevOwner.Text = string.Empty;
        drpNewDistrict.SelectedIndex = -1;
        drpRegion.SelectedIndex = -1;
        txtVillage.Text = string.Empty;
        txtTA.Text = string.Empty;
        LoadSiteCode();
        txtNewLocation.Focus();
    }
}