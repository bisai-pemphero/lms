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
        string sql = @"Select SiteCode, PhysicalLocation from Sites";
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
    P.OfferedTo AS ClientNo,
    C.FullName,
    STUFF((
        SELECT ', ' + P2.PlotNo
        FROM Plots P2
        WHERE P2.OfferedTo = P.OfferedTo
          AND P2.SiteNo = P.SiteNo
          AND P2.PlotStatus = 'Completed'
          AND P2.PlotNo NOT IN (SELECT PlotNo FROM SaleAgreementPlots)
        FOR XML PATH(''), TYPE
    ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS PlotNumbers,
    COUNT(*) AS TotalPlots,
    SUM(P.AgreedPrice) AS TotalAgreedPrice,
    SUM(P.AmountPaid) AS TotalAmountPaid,
    SUM(P.Balance) AS TotalBalance,
    MIN(P.DateOffered) AS DateOffered
FROM Plots P
LEFT JOIN Clients C
    ON P.OfferedTo = C.ClientNo
WHERE P.SiteNo = @siteNo
    AND P.PlotStatus = @status
    AND P.PlotNo NOT IN (SELECT PlotNo FROM SaleAgreementPlots)
GROUP BY
    P.OfferedTo,
    C.FullName,
    P.SiteNo
ORDER BY
    C.FullName",  connection);
            command.Parameters.AddWithValue("@siteNo", siteCode);
            command.Parameters.AddWithValue("@status", "Completed");
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
                string clientNo = row["ClientNo"].ToString();

                sb.Append("<tr>");
              

                sb.Append("<td>" + row["FullName"] + "</td>");
                sb.Append("<td>" + row["PlotNumbers"] + "</td>");
                sb.Append("<td>" + row["TotalPlots"] + "</td>");
                sb.Append("<td>" + row["TotalAgreedPrice"] + "</td>");
                sb.Append("<td>" + row["TotalAmountPaid"] + "</td>");
                sb.Append("<td>" + row["TotalBalance"] + "</td>");
                sb.Append("<td>" + row["DateOffered"] + "</td>");
                sb.Append("<td>");
                sb.Append("<a href='ConfirmAgreement.aspx?id=" + clientNo + "' class='btn btn-primary btn-sm'> Sale Agreement</a>");
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