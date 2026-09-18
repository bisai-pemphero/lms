using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

        LoadToday();
        LoadthisWeek();
        LoadthisMonth();
       
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
    public void LoadToday()
    {

        string sql = @"SELECT SUM(AmountPaid) 
FROM PlotPayments 
WHERE CAST([DatePaid] AS DATE) = CAST(GETDATE() AS DATE)
AND PostedBy = @user";
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
  AND PostedBy = @user";
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

    public void LoadthisMonth()
    {

        string sql = @"SELECT SUM(AmountPaid) 
FROM PlotPayments 
WHERE YEAR([DatePaid]) = YEAR(GETDATE()) 
  AND MONTH([DatePaid]) = MONTH(GETDATE())
  AND PostedBy = @user";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@user", lblSession.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblThismonth.Text = "K" +  (dr.IsDBNull(0) ? null : dr.GetDouble(0).ToString("N2"));
        }

        dr.Close();
        dr.Dispose();

    }

    

   
}