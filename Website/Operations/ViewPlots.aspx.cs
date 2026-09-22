using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
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
            LoadSites();
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


    public void LoadSites()
    {
        drpSite.Items.Clear();
        drpSite.Items.Add("");
        string sql = @"Select SiteCode, PhysicalLocation from Sites Where District = @location
                    Order by PhysicalLocation";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@location", lblLocation.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpSite.Items.Add(dr.GetString(0) + "|" + dr.GetString(1));
        }

        dr.Close();

    }

    //Load general Report
    public DataTable LoadSelectedPlots()
    {
        string[] site = drpSite.Text.Split('|');
        string siteCode = site[0];

        DataTable dt = new DataTable();
        string connectionString = ConfigurationManager.ConnectionStrings["lms"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(@"  SELECT  P.ID, P.PlotNo, P.NormalPrice, 
            C.Fullname, P.AgreedPrice,  P.AmountPaid, P.Balance, P.DateOffered FROM Plots AS P LEFT JOIN Clients AS C ON P.OfferedTo = C.ClientNo
             WHERE P.SiteNo = @siteNo",  connection);
            command.Parameters.AddWithValue("@siteNo", siteCode);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
        }

        return dt;
    }

    public void LoadPlots()
    {
        DataTable generalReport = LoadSelectedPlots();
        if (generalReport.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in generalReport.Rows)
            {
                string Id = row[0].ToString();

                sb.Append("<tr>");
                sb.Append("<td>" + row["PlotNo"] + "</td>");
                sb.Append("<td>" + row["NormalPrice"] + "</td>");
                sb.Append("<td>" + row["Fullname"] + "</td>");
                sb.Append("<td>" + row["AgreedPrice"] + "</td>");
                sb.Append("<td>" + row["AmountPaid"] + "</td>");
                sb.Append("<td>" + row["Balance"] + "</td>");
                sb.Append("<td>" + row["DateOffered"] + "</td>");
                sb.Append("<td>");
                sb.Append("<a href='PlotRegistration.aspx?id=" + Id + "' class='btn btn-primary btn-sm'> Edit</a>");
                sb.Append("</td>");
                sb.Append("</tr>");
            }
            // Assuming you have a placeholder for the rows
            allPlotsPlaceholder.InnerHtml = sb.ToString();
        }
        else
        {
            // Handle the case when no records are found
            allPlotsPlaceholder.InnerHtml = "<tr><td colspan='7'>Plots Not Available at this site.</td></tr>";
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        if(drpSite.SelectedIndex == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#ErrorModal').modal('show');", true);
        }

        LoadPlots();
    }

    //Add new plot
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
          
            if (txtPlotMark.Text == "")
            {
                lblError.Text = "Please enter Plot Number";
                lblSuccess.Text = "";
                txtPlotMark.Focus();
                return;
            }

            if (txtSize.Text == "")
            {
                lblError.Text = "Please enter land size";
                lblSuccess.Text = "";
                txtSize.Focus();
                return;
            }

            if (txtSellingPrice.Text == "")
            {
                lblError.Text = "Please enter Selling Price";
                lblSuccess.Text = "";
                txtSellingPrice.Focus();
                return;

            }
            if (txtPlotValue.Text == "")
            {
                lblError.Text = "Please enter Plot Value";
                lblSuccess.Text = "";
                txtPlotValue.Focus();
                return;
            }

            string[] Site = drpSite.Text.Split('|');
            string SiteCode = Site[0];

            string plotNo = SiteCode + "/" + txtPlotMark.Text.Trim();

            //Check if already exist
            string sql3 = @"Select * from Plots where PlotNo = @plotNo";
            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@plotNo", plotNo);
            SqlDataReader dr3 = cmd3.ExecuteReader();
            if (dr3.HasRows)
            {
                lblError.Text = "This Plot already exists!";
                lblSuccess.Text = "";
                return;
            }
            else
            {
                dr3.Close();
                dr3.Dispose();


                if (txtDevelopmentCost.Text == "") txtDevelopmentCost.Text = "0";

                double plotValue = Convert.ToDouble(txtPlotValue.Text.Trim());
                double sellingPrince = Convert.ToDouble(txtSellingPrice.Text.Trim());
                double DevCost = Convert.ToDouble(txtDevelopmentCost.Text.Trim());

                //insert into plot register
                string sql = @"INSERT INTO  Plots(PlotNo, SiteNo, PlotValue, NormalPrice, 
                PlotStatus, DateRegistered, RegisteredBy,  OfferedTo, AgreedPrice, AmountPaid, 
                Balance, PlotSize, DevelopmentCost,   Reservations) 
                VALUES (@plotNo, @siteNo, @plotValue, @sellingPrice, @status, GETDATE(), @user, 
                @offeredTo, @agreedPrice, @paid, @balance, @size, @devCost, @reservation)";
                SqlCommand cmd = new SqlCommand(sql, appconSQL2);
                cmd.Parameters.AddWithValue("@plotNo", plotNo);
                cmd.Parameters.AddWithValue("@siteNo", SiteCode);
                cmd.Parameters.AddWithValue("@plotValue", plotValue);
                cmd.Parameters.AddWithValue("@sellingPrice", sellingPrince);
                cmd.Parameters.AddWithValue("@status", "Available");
             
                cmd.Parameters.AddWithValue("@offeredTo", "none");
                cmd.Parameters.AddWithValue("@agreedPrice", 0);
                cmd.Parameters.AddWithValue("@paid", 0);
                cmd.Parameters.AddWithValue("@balance", 0);
                cmd.Parameters.AddWithValue("@size", txtSize.Text.Trim());
                cmd.Parameters.AddWithValue("@devCost", DevCost);
             
                cmd.Parameters.AddWithValue("@reservation", "Available");
                cmd.Parameters.AddWithValue("@user", lblSession.Text.Trim());
                cmd.ExecuteNonQuery();
                cmd.Dispose();


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
        txtSize.Text = string.Empty;
        txtSellingPrice.Text = string.Empty;
        txtPlotValue.Text = string.Empty;
        txtPlotMark.Text = string.Empty;
        txtDevelopmentCost.Text = string.Empty;
        drpSite.SelectedIndex = -1;
        lblError.Text = string.Empty;
        lblSuccess.Text = string.Empty;
    }




    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#newPlot').modal('show');", true);
    }
}