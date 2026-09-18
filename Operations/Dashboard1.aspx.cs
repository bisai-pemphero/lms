using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.Script.Services;

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

        if (!IsPostBack)
        {
            LoadUser();
            LoadTotalSites();
            LoadTotalPlots();
            LoadAvailablePlots();
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

    public void LoadTotalSites()
    {
        string sql = @"Select Count(PhysicalLocation) from Sites where District = @district";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@district", lblLocation.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblTotalSites.Text = dr.GetInt32(0).ToString();
        }

        dr.Close();
        dr.Dispose();
    }

    public void LoadTotalPlots()
    {
        string sql = @"Select Count(P.PlotNo) from Plots as P Join Sites as S  
        on P.SiteNo = S.SiteCode where S.District = @district";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@district", lblLocation.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblTotalPlots.Text = dr.GetInt32(0).ToString();
        }

        dr.Close();
        dr.Dispose();
    }

    public void LoadAvailablePlots()
    {
        string sql = @"Select Count(P.PlotNo) from Plots as P Join Sites as S  
        on P.SiteNo = S.SiteCode where S.District = @district and P.PlotStatus = @status";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@district", lblLocation.Text.Trim());
        cmd.Parameters.AddWithValue("@status", "Available");
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblAvailablePlots.Text = dr.GetInt32(0).ToString();
        }

        dr.Close();
        dr.Dispose();
    }

    // Method to get plot status distribution for the chart
    public DataTable GetPlotStatusDistribution()
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(appconStr))
        {
            string query = @"SELECT PlotStatus, COUNT(*) as Count 
                             FROM Plots 
                             WHERE SiteNo IN (SELECT SiteCode FROM Sites WHERE District = @district)
                             GROUP BY PlotStatus";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@district", lblLocation.Text.Trim());
                conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }
        return dt;
    }

    // Method to get sales trend data
    public DataTable GetSalesTrendData()
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(appconStr))
        {
            string query = @"SELECT 
                                MONTH(DateRegistered) as Month, 
                                COUNT(*) as PlotsSold,
                                SUM(ISNULL(AmountPaid, 0)) as Revenue
                             FROM Plots 
                             WHERE PlotStatus = 'Sold' 
                                AND YEAR(DateRegistered) = YEAR(GETDATE())
                                AND SiteNo IN (SELECT SiteCode FROM Sites WHERE District = @district)
                             GROUP BY MONTH(DateRegistered)
                             ORDER BY MONTH(DateRegistered)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@district", lblLocation.Text.Trim());
                conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }
        return dt;
    }

    // Method to get average price by district
    public DataTable GetPriceByZoneData()
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(appconStr))
        {
            string query = @"SELECT S.District, AVG(ISNULL(P.AgreedPrice, 0)) as AveragePrice 
                             FROM Sites as S
                             JOIN Plots as P ON P.SiteNo = S.SiteCode
                             WHERE S.District = @district
                             GROUP BY S.District";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@district", lblLocation.Text.Trim());
                conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }
        return dt;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetChartData()
    {
        Dashboard page = new Dashboard();
        page.readConf();
        page.dbconnect();

        // Set the location from session (you might need to pass this as a parameter)
        if (HttpContext.Current.Session["USER"] != null)
        {
            page.lblSession.Text = HttpContext.Current.Session["USER"].ToString();
            page.LoadUser(); // This will set the location
        }

        return new
        {
            statusData = page.GetPlotStatusDistributionForChart(),
            trendData = page.GetSalesTrendDataForChart(),
            zoneData = page.GetPriceByZoneDataForChart()
        };
    }

    // Helper methods to format data for charts
    private object GetPlotStatusDistributionForChart()
    {
        DataTable dt = GetPlotStatusDistribution();
        var labels = new List<string>();
        var values = new List<int>();

        foreach (DataRow row in dt.Rows)
        {
            labels.Add(row["PlotStatus"].ToString());
            values.Add(Convert.ToInt32(row["Count"]));
        }

        return new { labels = labels, values = values };
    }

    private object GetSalesTrendDataForChart()
    {
        DataTable dt = GetSalesTrendData();
        var monthNames = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        var plotsSold = new int[12];
        var revenue = new decimal[12];

        foreach (DataRow row in dt.Rows)
        {
            int monthIndex = Convert.ToInt32(row["Month"]) - 1;
            if (monthIndex >= 0 && monthIndex < 12)
            {
                plotsSold[monthIndex] = Convert.ToInt32(row["PlotsSold"]);
                revenue[monthIndex] = Convert.ToDecimal(row["Revenue"]);
            }
        }

        return new
        {
            labels = monthNames,
            plotsSold = plotsSold,
            revenue = Array.ConvertAll(revenue, x => (double)x / 1000) // Convert to thousands
        };
    }

    private object GetPriceByZoneDataForChart()
    {
        DataTable dt = GetPriceByZoneData();
        var labels = new List<string>();
        var values = new List<double>();

        foreach (DataRow row in dt.Rows)
        {
            labels.Add(row["District"].ToString());
            values.Add(Convert.ToDouble(row["AveragePrice"]));
        }

        return new { labels = labels, values = values };
    }
}