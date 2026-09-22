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

public partial class Dashboard : System.Web.UI.Page
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

        LoadUser();
        LoadTotalSites();
        LoadTotalPlots();
        LoadAvailablePlots();
        LoadTotalCustomers();
        LoadPlotStaus();
        RevenueOverview();
        LoadPendingWithdrawals();
        LoadPendingVouchers();
        LoadthisWeek();
        LoadToday();
        LoadSiteDetails();

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
            lblCEOName.Text = dr.GetString(0).ToString();
            lblLocation.Text = dr.GetString(1).ToString();
        }

        dr.Close();
        dr.Dispose();
    }
    public void LoadTotalSites()
    {

        string sql = @"Select Count(PhysicalLocation) from Sites";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblTotalSites.Text = dr.IsDBNull(0) ? null : dr.GetInt32(0).ToString();
        }

        dr.Close();
        dr.Dispose();
    }

    public void LoadTotalPlots()
    {

        string sql = @"Select Count(P.PlotNo) from Plots as P Join Sites as S  
        on P.SiteNo = S.SiteCode";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
      
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblTotalPlots.Text = dr.IsDBNull(0) ? null : dr.GetInt32(0).ToString();
        }

        dr.Close();
        dr.Dispose();

    }

    public void LoadPendingVouchers()
    {

        string sql = @"Select Count([PaymentVoucherID]) from[PaymentVoucher] 
            where Status = @status ";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@status", "Pending");
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblPendingVouchers.Text = dr.IsDBNull(0) ? null : dr.GetInt32(0).ToString();
        }

        dr.Close();
        dr.Dispose();

        string sql0 = @"Select Count([PaymentVoucherID]) from[PaymentVoucherCreditor] 
            where Status = @status ";
        SqlCommand cmd0 = new SqlCommand(sql0, appconSQL2);
        cmd0.Parameters.AddWithValue("@status", "Pending");
        SqlDataReader dr0 = cmd0.ExecuteReader();
        while (dr0.Read())
        {
            lblPendingCreditor.Text = dr0.IsDBNull(0) ? null : dr0.GetInt32(0).ToString();
        }

        dr0.Close();
        dr0.Dispose();

    }

    public void LoadAvailablePlots()
    {

        string sql = @"Select Count(P.PlotNo) from Plots as P Join Sites as S  
        on P.SiteNo = S.SiteCode where P.PlotStatus = @status";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@district", lblLocation.Text.Trim());
        cmd.Parameters.AddWithValue("@status", "Available");
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblAvailablePlots.Text = dr.IsDBNull(0) ? null : dr.GetInt32(0).ToString();
        }

        dr.Close();
        dr.Dispose();

    }

    
    public void LoadTotalCustomers()
    {

        string sql = @"Select Count(Fullname) from Clients";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblTotalCustomers.Text = dr.IsDBNull(0) ? null : dr.GetInt32(0).ToString();
        }

        dr.Close();
        dr.Dispose();

    }

    public void LoadPlotStaus()
    {

        string sql = @" Select Count(PlotNo) as Available from Plots where PlotStatus = 'Available'";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblAvailable.Text = dr.IsDBNull(0) ? null : dr.GetInt32(0).ToString();
        }

        dr.Close();
        dr.Dispose();


        string sql1 = @"Select Count(PlotNo) as Pending from Plots where PlotStatus = 'Pending'";
        SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
        SqlDataReader dr1 = cmd1.ExecuteReader();
        while (dr1.Read())
        {
            lblPending.Text = dr1.IsDBNull(0) ? null : dr1.GetInt32(0).ToString();
        }

        dr1.Close();
        dr1.Dispose();


        string sql2 = @"Select Count(PlotNo) as Pending from Plots where PlotStatus = 'Pending'";
        SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
        SqlDataReader dr2 = cmd2.ExecuteReader();
        while (dr2.Read())
        {
            lblPending.Text = dr2.IsDBNull(0) ? null : dr2.GetInt32(0).ToString();
        }

        dr2.Close();
        dr1.Dispose();

        string sql3 = @"Select Count(PlotNo) as Allocated from Plots where PlotStatus = 'Allocated'";
        SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
        SqlDataReader dr3 = cmd3.ExecuteReader();
        while (dr3.Read())
        {
            lblAllocated.Text = dr3.IsDBNull(0) ? null : dr3.GetInt32(0).ToString();
        }

        dr3.Close();
        dr3.Dispose();

        string sql4 = @"Select Count(PlotNo) as Completed from Plots where PlotStatus = 'Completed'";
        SqlCommand cmd4 = new SqlCommand(sql4, appconSQL2);
        SqlDataReader dr4 = cmd4.ExecuteReader();
        while (dr4.Read())
        {
            lblCompleted.Text = dr4.IsDBNull(0) ? null : dr4.GetInt32(0).ToString();
        }

        dr4.Close();
        dr4.Dispose();
    }

    public void RevenueOverview()
    {

        string sql = @"SELECT 
	sum(P.MonthlyInstallment )
   
FROM Plots P
INNER JOIN Clients C
    ON P.OfferedTo = C.ClientNo
INNER JOIN Penalties Pen
    ON P.MonthlyInstallment BETWEEN Pen.FromRange AND Pen.ToRange
WHERE P.OfferPeriod > 12 and P.Balance > 0 and P.PlotStatus != 'Pending'
AND NOT EXISTS
(
    SELECT 1
    FROM PlotPayments PP
    WHERE PP.PlotNo = P.PlotNo
    AND PP.SiteNo = P.SiteNo
    AND MONTH(PP.DatePaid) = MONTH(GETDATE())
    AND YEAR(PP.DatePaid) = YEAR(GETDATE()))";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblPendingPayments.Text = "K" + (dr.IsDBNull(0) ? null : dr.GetDouble(0).ToString("N2"));
        }

        dr.Close();
        dr.Dispose();


        string sql3 = @"Select Sum(AmountPaid) as TotalCollected from PlotPayments
        Where MONTH(DatePaid) = MONTH(GETDATE())
        AND YEAR(DatePaid) = YEAR(GETDATE())";
        SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
        SqlDataReader dr3 = cmd3.ExecuteReader();
        while (dr3.Read())
        {
            lblTotalSales.Text = "K" + (dr3.IsDBNull(0) ? null : dr3.GetDouble(0).ToString("N2"));
        }

        dr3.Close();
        dr3.Dispose();

        string sql1 = @"Select Sum([PlotValue]) Total from Plots where PlotStatus != 'Completed'";
        SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
        SqlDataReader dr1 = cmd1.ExecuteReader();
        while (dr1.Read())
        {
            lblPlotStockValue.Text = "K" + (dr1.IsDBNull(0) ? null : dr1.GetDouble(0).ToString("N2"));
        }

        dr1.Close();
        dr1.Dispose();

    }

    public void LoadPendingWithdrawals()
    {

        string sql = @"Select Count(PlotNo) from PlotWithdrawal where Status = 'Pending'";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblWithdraws.Text = dr.GetInt32(0).ToString();
        }

        dr.Close();
        dr.Dispose();

    }

    public void LoadToday()
    {

        string sql = @"SELECT SUM(AmountPaid) 
FROM PlotPayments 
WHERE CAST([DatePaid] AS DATE) = CAST(GETDATE() AS DATE)
";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@user", lblSession.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblToday.Text = "K" + (dr.IsDBNull(0) ? null : dr.GetDouble(0).ToString("N2"));
        }

        dr.Close();
        dr.Dispose();

    }

    public void LoadthisWeek()
    {

        string sql = @"SELECT SUM(AmountPaid) 
FROM PlotPayments 
WHERE CAST([DatePaid] AS DATE) >= DATEADD(DAY, 1 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
  AND CAST([DatePaid] AS DATE) < DATEADD(DAY, 8 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
  ";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@user", lblSession.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblThisWeek.Text = "K" + (dr.IsDBNull(0) ? null : dr.GetDouble(0).ToString("N2"));
        }

        dr.Close();
        dr.Dispose();

    }

    //Site details
    public DataTable LoadSite()
    {

        DataTable dt = new DataTable();
        string connectionString = ConfigurationManager.ConnectionStrings["lms"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT
    S.PhysicalLocation,
    COUNT(P.PlotNo) as TotalPlots,
    COUNT(CASE WHEN P.OfferedTo <> 'none' THEN 1 END) AS Allocated,
    COUNT(CASE WHEN P.OfferedTo = 'none' THEN 1 END) AS Available,
	 S.Size AS SiteSize,
    SUM(TRY_CAST(P.PlotSize AS DECIMAL(18,2))) AS AllocatedLandSize,

    TRY_CAST(S.Size AS DECIMAL(18,2)) - ISNULL(SUM(TRY_CAST(P.PlotSize AS DECIMAL(18,2))), 0) - TRY_CAST(S.RoadSize AS DECIMAL(18,2)) AS RemainingSize,
    S.RoadSize, S.RemainingAcreage

FROM Sites S
LEFT JOIN Plots P
    ON S.SiteCode = P.SiteNo
 GROUP BY
    S.PhysicalLocation, S.Size,  S.RoadSize, S.RemainingAcreage ", connection);
           
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
        }

        return dt;
    }

    public void LoadSiteDetails()
    {
        DataTable generalReport = LoadSite();
        if (generalReport.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in generalReport.Rows)
            {
                sb.Append("<tr>");
                sb.Append("<td>" + row["PhysicalLocation"] + "</td>");
                sb.Append("<td>" + row["TotalPlots"] + "</td>");
                sb.Append("<td>" + row["Allocated"] + "</td>");
                sb.Append("<td>" + row["Available"] + "</td>");
                sb.Append("<td>" + row["SiteSize"] + "</td>");
                sb.Append("<td>" + row["RemainingAcreage"] + "</td>");
                sb.Append("<td>" + row["RoadSize"] + "</td>");
                sb.Append("<td>" + row["AllocatedLandSize"] + "</td>");
                sb.Append("<td>" + row["RemainingSize"] + "</td>");


                sb.Append("</tr>");
            }
            // Assuming you have a placeholder for the rows
            SiteDetails.InnerHtml = sb.ToString();
        }
        else
        {
            // Handle the case when no records are found
            SiteDetails.InnerHtml = "<tr><td colspan='7'>Site Details not Available.</td></tr>";
        }
    }
}