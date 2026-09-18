using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
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
            [DevelopmentsDone],[DevelopmentCost], [TA],[Village],[Region], [Site_Description], 
            RoadSize, RemainingAcreage, SiteMap from Sites Where ID = @Id";
        SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
        cmd1.Parameters.AddWithValue("@Id", siteId);
        SqlDataReader dr1 = cmd1.ExecuteReader();
        while (dr1.Read())
        {
            drpDistrict.Text = dr1.GetString(0);
            txtLocation.Text = dr1.GetString(1);
            txtSize.Text = dr1.GetString(2);
            txtPreviousOwner.Text = dr1.GetString(3);
            txtDevelopment.Text = dr1.GetString(4);
            txtDevelopmentCost.Text = dr1.GetDouble(5).ToString();
            txtTA.Text = dr1.GetString(6);
            txtVillage.Text = dr1.GetString(7);
            drpRegion.Text = dr1.GetString(8);
            txtDescription.Text = dr1.GetString(9);
            txtRoadSize.Text = dr1.IsDBNull(10) ? "0" : dr1.GetDouble(10).ToString();
            txtPlotAcreage.Text = dr1.IsDBNull(11) ? "0" : dr1.GetDouble(11).ToString();
            lblSketch.Text = dr1.IsDBNull(12) ? "" : dr1.GetString(12);
        }

    }
   
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewAllSites.aspx");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        
            if (fnSketchmap.FileContent == null)
            {
                return;
            }
            else
            {
                DeleteSketch(lblSketch.Text.Trim());
            }


            if ((fnSketchmap.PostedFile != null) && (fnSketchmap.PostedFile.ContentLength > 0))
            {
                string lblFormat = "";
                string fn = System.IO.Path.GetFileName(fnSketchmap.PostedFile.FileName);
                string SaveLocation = Server.MapPath("~\\assets\\docs") + "\\" + fn;

                lblFormat = fn.Substring(fn.Length - 3, 3);

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
                if (txtDescription.Text == "")
                {
                    lblError.Text = "Please enter a description that best describes the site!";
                    lblSuccess.Text = "";
                    txtDescription.Focus();
                    return;

                }


                if (txtDevelopmentCost.Text == "") txtDevelopmentCost.Text = "0";


                double DevCost = Convert.ToDouble(txtDevelopmentCost.Text.Trim());

                //insert into sites register
                string sql = @"Update Sites set District = @district, PhysicalLocation = @location, 
                Size =@size, PreviousOwner =@owner, DevelopmentsDone = @development, DevelopmentCost = @devCost, TA = @ta, 
                Village = @village, Region = @region, Site_Description = @description, RoadSize = @roadSize,
                RemainingAcreage = @totalPlot, SiteMap = @siteMap  where ID = @Id";
                SqlCommand cmd = new SqlCommand(sql, appconSQL2);

                cmd.Parameters.AddWithValue("@district", drpDistrict.Text.Trim());
                cmd.Parameters.AddWithValue("@location", txtLocation.Text.Trim());
                cmd.Parameters.AddWithValue("@size", txtSize.Text.Trim());
                cmd.Parameters.AddWithValue("@owner", txtPreviousOwner.Text.Trim());
                cmd.Parameters.AddWithValue("@development", txtDevelopment.Text.Trim());
                cmd.Parameters.AddWithValue("@devCost", DevCost);
                cmd.Parameters.AddWithValue("@ta", txtTA.Text.Trim());
                cmd.Parameters.AddWithValue("@village", txtVillage.Text.Trim());
                cmd.Parameters.AddWithValue("@region", drpRegion.Text.Trim());
                cmd.Parameters.AddWithValue("@description", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Id", lblSiteCode.Text.Trim());

                cmd.Parameters.AddWithValue("@totalPlot", txtPlotAcreage.Text.Trim());
                cmd.Parameters.AddWithValue("@siteMap", fn);
                cmd.Parameters.AddWithValue("@roadSize", txtRoadSize.Text.Trim());
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                fnSketchmap.PostedFile.SaveAs(SaveLocation);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#UpdateModal').modal('show');", true);
            }
                catch (Exception ex)
                {
                    Response.Write("Document upload Error : " + ex.Message);
                }
            }
            else
            {
                lblError.Text = "PLEASE SELECT FILE TO UPLOAD";
                fnSketchmap.Focus();
            }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewAllSites.aspx");
        //string sqlDelete = @"Delete from Sites where ID = @Id";
        //using (SqlCommand cmdDelete = new SqlCommand(sqlDelete, appconSQL2))
        //{
        //    cmdDelete.Parameters.AddWithValue("@Id", lblSiteCode.Text.Trim());
        //    cmdDelete.ExecuteNonQuery();
        //}

        //// Show success modal
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#DeleteModal').modal('show');", true);

    }

    public void DeleteSketch(string sketch)
    {
        // physical paths
        string sketchPath = Server.MapPath("~/assets/docs/") + sketch;

        try
        {
            if (File.Exists(sketchPath))
            {
                File.Delete(sketchPath);
            }
        }
        catch (IOException ioEx)
        {
            // log or surface the error as needed
            lblError.Text = "File‑system error: " + ioEx.Message;
        }
    }

    protected void btnUpdateSketch_Click(object sender, EventArgs e)
    {

        DeleteSketch(lblSketch.Text.Trim());

        if ((fnSketchmap.PostedFile != null) && (fnSketchmap.PostedFile.ContentLength > 0))
        {
            string lblFormat = "";
            string fn = System.IO.Path.GetFileName(fnSketchmap.PostedFile.FileName);
            string SaveLocation = Server.MapPath("~\\assets\\docs") + "\\" + fn;

            lblFormat = fn.Substring(fn.Length - 3, 3);

            try
            {
                //update sites 
                string sql = @"Update Sites set SiteMap = @siteMap  where ID = @Id";
                SqlCommand cmd = new SqlCommand(sql, appconSQL2);
                cmd.Parameters.AddWithValue("@Id", lblSiteCode.Text.Trim());
                cmd.Parameters.AddWithValue("@siteMap", fn);
               
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                fnSketchmap.PostedFile.SaveAs(SaveLocation);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#UpdateModal').modal('show');", true);
            }
            catch (Exception ex)
            {
                Response.Write("Document upload Error : " + ex.Message);
            }
        }
        else
        {
            lblError.Text = "PLEASE SELECT FILE TO UPLOAD";
            fnSketchmap.Focus();
        }
    }
}