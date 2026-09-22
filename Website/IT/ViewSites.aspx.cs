using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewSites : System.Web.UI.Page
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

            SqlCommand command = new SqlCommand(@"
SELECT 
    S.[District], 
    S.[PhysicalLocation], 
    S.[Size], 
    S.[PreviousOwner], 
    FORMAT(S.[InitialValue], '#,##0.00') as InitialValue,
    FORMAT(S.[AmountPaid], '#,##0.00') as AmountPaid,
    FORMAT(S.[Balance], '#,##0.00') as Balance, 
    COUNT(P.PlotNo) AS TotalPlots,
    FORMAT(SUM(COALESCE(P.NormalPrice, 0)), '#,##0.00') AS EstimateReturn
FROM 
    Sites AS S
LEFT JOIN 
    Plots AS P ON S.SiteCode = P.SiteNo
GROUP BY 
    S.District, 
    S.PhysicalLocation, 
    S.Size, 
    S.PreviousOwner, 
    S.InitialValue, 
    S.AmountPaid, 
    S.Balance;", connection);
            command.Parameters.AddWithValue("@district", lblLocation.Text.Trim());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Response.Write("<tr>");
                Response.Write("<td>" + reader["District"] + "</td>");
                Response.Write("<td>" + reader["PhysicalLocation"] + "</td>");
                Response.Write("<td>" + reader["Size"] + "</td>");
                Response.Write("<td>" + reader["PreviousOwner"] + "</td>");
                Response.Write("<td>" + reader["InitialValue"] + "</td>");
                Response.Write("<td>" + reader["AmountPaid"] + "</td>");
                Response.Write("<td>" + reader["Balance"] + "</td>");
                Response.Write("<td>" + reader["TotalPlots"] + "</td>");
                Response.Write("<td>" + reader["EstimateReturn"] + "</td>");
                Response.Write("</tr>");
            }

            reader.Close();
        }
    }
}