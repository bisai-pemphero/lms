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
            LoadLocation();
           
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

    public void LoadLocation()
    {
        drpLocation.Items.Clear();
        drpLocation.Items.Add("");
        string sql = "Select Distinct [District] from Sites";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@username", lblSession.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpLocation.Items.Add(dr.GetString(0).ToString());
        }

        dr.Close();
        dr.Dispose();
    }

    public void LoadStats()
    {
        try
        {
            lblExpected.Text = string.Empty;
            string sql = @" Select Sum(P.[AgreedPrice]) from Plots as P
        Join Sites as S on P.SiteNo = S.SiteCode
        where P.PlotStatus != 'Completed' 
        and S.District = @district";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@district", drpLocation.Text.Trim());
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                double expectedIncome = dr.IsDBNull(0) ? 0 : dr.GetDouble(0);
                lblExpected.Text = "K" + expectedIncome.ToString("N2");
            }
            dr.Close();
            dr.Dispose();


            lblCollected.Text = string.Empty;
            string sql1 = @" Select Sum(P.[AmountPaid]) from Plots as P
        Join Sites as S on P.SiteNo = S.SiteCode
        where P.PlotStatus != 'Completed' 
        and S.District = @district";
            SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
            cmd1.Parameters.AddWithValue("@district", drpLocation.Text.Trim());
            SqlDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                double collected = dr1.IsDBNull(0) ? 0 : dr1.GetDouble(0);
                lblCollected.Text = "K" + collected.ToString("N2");
            }
            dr1.Close();
            dr1.Dispose();


            lblBalance.Text = string.Empty;
            string sql2 = @" Select Sum(P.[Balance]) from Plots as P
        Join Sites as S on P.SiteNo = S.SiteCode
        where P.PlotStatus != 'Completed' 
        and S.District = @district";
            SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
            cmd2.Parameters.AddWithValue("@district", drpLocation.Text.Trim());
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                double balance = dr2.IsDBNull(0) ? 0 : dr2.GetDouble(0);
                lblBalance.Text = "K" + balance.ToString("N2");
            }
            dr2.Close();
            dr2.Dispose();


            lblOverdueAmount.Text = string.Empty;
            string sql3 = @"SELECT 
	        Sum(P.[Balance]) AS [OverdueAmount]
            FROM [Plots] as P
            Join Sites as S on P.SiteNo = S.SiteCode
            WHERE 
	        S.District = @district
	        AND P.[PlotStatus] != @status
            AND ISNUMERIC(P.[OfferPeriod]) = 1
            AND DATEADD(MONTH, CAST(P.[OfferPeriod] AS INT), P.[DateOffered]) < GETDATE()
            AND P.[Balance] > 0";
            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@district", drpLocation.Text.Trim());
            cmd3.Parameters.AddWithValue("@status", "Completed");
            SqlDataReader dr3 = cmd3.ExecuteReader();
            while (dr3.Read())
            {
                double overdue = dr3.IsDBNull(0) ? 0 : dr3.GetDouble(0);
                lblOverdueAmount.Text = "K" + overdue.ToString("N2");
            }
            dr3.Close();
            dr3.Dispose();
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#ErrorModal').modal('show');", true);
        }
    }


    //Load Sales Report
    public DataTable SalesReport()
    {
        if (drpLocation.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#ErrorModal').modal('show');", true);
        }

        if (txtStartDate.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#ErrorModal').modal('show');", true);
        }

        if (txtEndDate.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#ErrorModal').modal('show');", true);
        }

        DataTable dt = new DataTable();
        string connectionString = ConfigurationManager.ConnectionStrings["lms"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT C.[Fullname], 
            S.[PhysicalLocation],
            P.[PlotNo],  
	        FORMAT(P.[AmountPaid], '#,##0.00') as AmountPaid, 
            FORMAT(P.[NewBalance], '#,##0.00') as NewBalance,
	        P.[PaymentMode], 
            P.[DatePaid]
	        FROM [PlotPayments] as P join [Clients] as C on P.PaidBy = C.ClientNo 
	        join Sites as S on P.SiteNo=S.SiteCode 
	        where S.District = @location AND CONVERT(Date, P.[DatePaid]) BETWEEN CONVERT(Date,  @date1) 
	        AND CONVERT(Date, @date2) Order by P.[DatePaid]", connection);
            command.Parameters.AddWithValue("@location", drpLocation.Text.Trim());
            command.Parameters.AddWithValue("@date1", txtStartDate.Text.Trim());
            command.Parameters.AddWithValue("@date2", txtEndDate.Text.Trim());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
        }

        return dt;
    }

    public void LoadSalesReport()
    {
        DataTable generalReport = SalesReport();
        if (generalReport.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in generalReport.Rows)
            {
                sb.Append("<tr>");
                sb.Append("<td>" + row["Fullname"] + "</td>");
                sb.Append("<td>" + row["PhysicalLocation"] + "</td>");
                sb.Append("<td>" + row["PlotNo"] + "</td>");
                sb.Append("<td>" + row["AmountPaid"] + "</td>");
                sb.Append("<td>" + row["NewBalance"] + "</td>");
                sb.Append("<td>" + row["PaymentMode"] + "</td>");
                sb.Append("<td>" + row["DatePaid"] + "</td>");
                sb.Append("</tr>");
            }

            salesReportPlaceholder.InnerHtml = sb.ToString();
        }
        else
        {
            // Handle the case when no records are found
            salesReportPlaceholder.InnerHtml = "<tr><td colspan='7'>No sales done.</td></tr>";
        }
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        LoadSalesReport();
        LoadOverduePlots();
        LoadStats();

    }


    //Load Sales Report
    public DataTable OverduePlots()
    {
        if (drpLocation.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#ErrorModal').modal('show');", true);
        }

        if (txtStartDate.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#ErrorModal').modal('show');", true);
        }

        if (txtEndDate.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#ErrorModal').modal('show');", true);
        }

        DataTable dt = new DataTable();
        string connectionString = ConfigurationManager.ConnectionStrings["lms"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(@"SELECT 
	S.PhysicalLocation,
    P.[PlotNo],
    C.[Fullname],
    P.[DateOffered],
    P.[OfferPeriod],
    P.[AmountPaid],
    P.[Balance],
    CASE 
        WHEN ISNUMERIC(P.[OfferPeriod]) = 1 THEN 
            DATEADD(MONTH, CAST(P.[OfferPeriod] AS INT), P.[DateOffered])
        ELSE 
            NULL
    END AS [DueDate],
    CASE 
        WHEN ISNUMERIC(P.[OfferPeriod]) = 1 THEN 
            DATEDIFF(MONTH, DATEADD(MONTH, CAST(P.[OfferPeriod] AS INT), P.[DateOffered]), GETDATE())
        ELSE 
            NULL
    END AS [MonthsOverdue]
FROM [Plots] as P
Join Sites as S on P.SiteNo = S.SiteCode
Join Clients as C on P.OfferedTo = C.ClientNo
WHERE 
	S.District = @location
	AND P.[PlotStatus] != @status
    AND ISNUMERIC(P.[OfferPeriod]) = 1
    AND DATEADD(MONTH, CAST(P.[OfferPeriod] AS INT), P.[DateOffered]) < GETDATE()
    AND P.[Balance] > 0
ORDER BY [MonthsOverdue] DESC;", connection);
            command.Parameters.AddWithValue("@location", drpLocation.Text.Trim());
            command.Parameters.AddWithValue("@status", "Completed");
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
        }

        return dt;
    }

    public void LoadOverduePlots()
    {
        DataTable generalReport = OverduePlots();
        if (generalReport.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in generalReport.Rows)
            {
                sb.Append("<tr>");
                sb.Append("<td>" + row["PhysicalLocation"] + "</td>");
                sb.Append("<td>" + row["PlotNo"] + "</td>");
                sb.Append("<td>" + row["Fullname"] + "</td>");
                sb.Append("<td>" + row["DateOffered"] + "</td>");
                sb.Append("<td>" + row["OfferPeriod"] + "</td>");
                sb.Append("<td>" + row["AmountPaid"] + "</td>");
                sb.Append("<td>" + row["Balance"] + "</td>");
                sb.Append("<td>" + row["DueDate"] + "</td>");
                sb.Append("<td>" + row["MonthsOverdue"] + "</td>");                
                sb.Append("</tr>");
            }

            overduePlotsPlaceholder.InnerHtml = sb.ToString();
        }
        else
        {
            // Handle the case when no records are found
            overduePlotsPlaceholder.InnerHtml = "<tr><td colspan='9'>No Plots Overdue.</td></tr>";
        }
    }

}