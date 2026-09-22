using System;
using System.Activities;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PlotHistory : System.Web.UI.Page
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


    public void LoadSites(string location)
    {
        drpSite.Items.Clear();
        drpSite.Items.Add("");
        string sql = @"Select SiteCode, PhysicalLocation from Sites Where District = @location
                    Order by PhysicalLocation";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@location", location);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpSite.Items.Add(dr.GetString(0) + "|" + dr.GetString(1));
        }

        dr.Close();

    }

    public void LoadDistrict()
    {
        drpDistrict.Items.Clear();
        drpDistrict.Items.Add("");

        string sql = "SELECT Distinct DistrictName FROM  Districts ORDER BY DistrictName";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpDistrict.Items.Add(dr.GetString(0));
        }

        dr.Close();
        dr.Dispose();

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
            SqlCommand command = new SqlCommand(@"WITH PlotHistory AS (
    -- Plot Withdrawals
    SELECT 
        pw.PlotNo,
        pw.SiteNo,
        s.PhysicalLocation,
        pw.OfferedTo AS Owner,
        c.Fullname AS OwnerName,
        'Withdrawal' AS ActionType,
        pw.WithdrawReason AS Reason,
        pw.WithdrawDate AS ActionDate,
        pw.DateAllocated AS AllocationDate,
        NULL AS PreviousOwner,
        NULL AS PreviousOwnerName,
        NULL AS CurrentOwnerFromPlots,
        NULL AS CurrentOwnerNameFromPlots
    FROM [LMS].[dbo].[PlotWithdrawal] pw
    LEFT JOIN [LMS].[dbo].[Sites] s ON pw.SiteNo = s.SiteCode
    LEFT JOIN [LMS].[dbo].[Clients] c ON pw.OfferedTo = c.ClientNo
    
    UNION ALL
    
    -- Change of Ownership (Current Owner)
    SELECT 
        co.Plot_Number AS PlotNo,
        co.Site_Number AS SiteNo,
        s.PhysicalLocation,
        co.Current_Owner AS Owner,
        c_current.Fullname AS OwnerName,
        'Ownership Change' AS ActionType,
        'Transfer' AS Reason,
        co.Date AS ActionDate,
        NULL AS AllocationDate,
        co.Previous_Owner AS PreviousOwner,
        c_prev.Fullname AS PreviousOwnerName,
        NULL AS CurrentOwnerFromPlots,
        NULL AS CurrentOwnerNameFromPlots
    FROM [LMS].[dbo].[ChangeofOwnership] co
    LEFT JOIN [LMS].[dbo].[Sites] s ON co.Site_Number = s.SiteCode
    LEFT JOIN [LMS].[dbo].[Clients] c_current ON co.Current_Owner = c_current.ClientNo
    LEFT JOIN [LMS].[dbo].[Clients] c_prev ON co.Previous_Owner = c_prev.ClientNo
    
    UNION ALL
    
    -- Change of Ownership (Previous Owner - to show the ownership period)
    SELECT 
        co.Plot_Number AS PlotNo,
        co.Site_Number AS SiteNo,
        s.PhysicalLocation,
        co.Previous_Owner AS Owner,
        c_prev.Fullname AS OwnerName,
        'Ownership Ended' AS ActionType,
        'Changed Ownership' AS Reason,
        co.Date AS ActionDate,
        NULL AS AllocationDate,
        NULL AS PreviousOwner,
        NULL AS PreviousOwnerName,
        NULL AS CurrentOwnerFromPlots,
        NULL AS CurrentOwnerNameFromPlots
    FROM [LMS].[dbo].[ChangeofOwnership] co
    LEFT JOIN [LMS].[dbo].[Sites] s ON co.Site_Number = s.SiteCode
    LEFT JOIN [LMS].[dbo].[Clients] c_prev ON co.Previous_Owner = c_prev.ClientNo
)

SELECT 
    ph.PhysicalLocation, ph.PlotNo, ph.OwnerName, ph.ActionType, ph.Reason,
    ph.ActionDate, ph.AllocationDate, ph.PreviousOwnerName, c_current.Fullname AS CurrentOwnerName,
    p.PlotStatus
FROM PlotHistory ph
LEFT JOIN [LMS].[dbo].[Plots] p ON ph.PlotNo = p.PlotNo AND ph.SiteNo = p.SiteNo
LEFT JOIN [LMS].[dbo].[Clients] c_current ON p.OfferedTo = c_current.ClientNo
where ph.SiteNo = @siteNo
ORDER BY ph.PlotNo, ph.SiteNo, ph.ActionDate DESC",  connection);
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
                sb.Append("<tr>");
                sb.Append("<td>" + row["PhysicalLocation"] + "</td>");
                sb.Append("<td>" + row["PlotNo"] + "</td>");
                sb.Append("<td>" + row["OwnerName"] + "</td>");
                sb.Append("<td>" + row["ActionType"] + "</td>");
                sb.Append("<td>" + row["Reason"] + "</td>");
                sb.Append("<td>" + row["ActionDate"] + "</td>");
                sb.Append("<td>" + row["AllocationDate"] + "</td>");
                sb.Append("<td>" + row["PreviousOwnerName"] + "</td>");
                sb.Append("<td>" + row["CurrentOwnerName"] + "</td>");
                sb.Append("<td>" + row["PlotStatus"] + "</td>");
              
                sb.Append("</tr>");
            }
            // Assuming you have a placeholder for the rows
            allPlotsPlaceholder.InnerHtml = sb.ToString();
        }
        else
        {
            // Handle the case when no records are found
            allPlotsPlaceholder.InnerHtml = "<tr><td colspan='11'>No plots found.</td></tr>";
        }
    }

    protected void drpSite_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSite.SelectedIndex == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#ErrorModal').modal('show');", true);
        }

        LoadPlots();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard.aspx");
    }

    protected void drpDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSites(drpDistrict.Text.Trim());
        drpSite.Focus();
    }
}