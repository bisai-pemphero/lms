using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
        string sql = @"Select SiteCode, PhysicalLocation from Sites Order by PhysicalLocation";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
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
            SqlCommand command = new SqlCommand(@"SELECT 
    SA.Id,
    C.Fullname, 
 
    S.PhysicalLocation,

    STUFF((
        SELECT ', ' + P2.PlotNo
        FROM SaleAgreementPlots P2
        WHERE P2.SaleAgreementId = SA.Id
        FOR XML PATH(''), TYPE
    ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS PlotNumbers,

    SA.OfferDate

FROM Clients AS C

JOIN SaleAgreements AS SA 
    ON C.ClientNo = SA.ClientNo

JOIN SaleAgreementPlots AS SAP 
    ON SA.Id = SAP.SaleAgreementId

JOIN Sites AS S 
    ON S.SiteCode = SAP.SiteNo

JOIN Plots AS P 
    ON SAP.PlotNo = P.PlotNo

WHERE SAP.SiteNo = @siteNo

GROUP BY 
    C.Fullname, 
    S.PhysicalLocation,
    SA.OfferDate,
    SA.Id",  connection);
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
                string plotNo = row["Id"].ToString();

                sb.Append("<tr>");
               

                sb.Append("<td>" + row["Fullname"] + "</td>");
                sb.Append("<td>" + row["PhysicalLocation"] + "</td>");
                sb.Append("<td>" + row["PlotNumbers"] + "</td>");
                sb.Append("<td>" + row["OfferDate"] + "</td>");
                sb.Append("<td>");
                sb.Append("<a href='SaleAgreement.aspx?id=" + plotNo + "' class='btn btn-primary btn-sm'> View Agreement</a>");
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append("<a href='SaleAgreementEdit.aspx?id=" + plotNo + "' class='btn btn-secondary btn-sm'> Edit Agreement</a>");
                sb.Append("</td>");

                sb.Append("</tr>");
            }
            // Assuming you have a placeholder for the rows
            allPlotsPlaceholder.InnerHtml = sb.ToString();
        }
        else
        {
            // Handle the case when no records are found
            allPlotsPlaceholder.InnerHtml = "<tr><td colspan='7'>No plots found.</td></tr>";
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

   

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        //implement save agreement here
    }
}