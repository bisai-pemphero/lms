using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewCustomer : System.Web.UI.Page
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
            string id = Request.QueryString["status"];
            lblStatus.Text = id.ToUpper();
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

    public void LoadPlotDetails()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["lms"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(@"SELECT 
    S.PhysicalLocation, 
    P.PlotNo, 
    FORMAT(P.NormalPrice, 'N2') AS NormalPrice,
    C.Fullname, 
    FORMAT(P.AgreedPrice, 'N2') AS AgreedPrice,  
    FORMAT(P.AmountPaid, 'N2') AS AmountPaid, 
    FORMAT(P.Balance, 'N2') AS Balance, 
   P.DateOffered
FROM Plots AS P 
LEFT JOIN Clients AS C ON P.OfferedTo = C.ClientNo
LEFT JOIN Sites AS S ON S.SiteCode = P.SiteNo 
WHERE P.PlotStatus = @status", connection);
            command.Parameters.AddWithValue("@status", lblStatus.Text.Trim());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Response.Write("<tr>");
                Response.Write("<td>" + reader["PhysicalLocation"] + "</td>");
                Response.Write("<td>" + reader["PlotNo"] + "</td>");
                Response.Write("<td>" + reader["NormalPrice"] + "</td>");
                Response.Write("<td>" + reader["Fullname"] + "</td>");
                Response.Write("<td>" + reader["AgreedPrice"] + "</td>");
                Response.Write("<td>" + reader["AmountPaid"] + "</td>");
                Response.Write("<td>" + reader["Balance"] + "</td>");
                Response.Write("<td>" + reader["DateOffered"] + "</td>");
                Response.Write("</tr>");
            }

            reader.Close();
        }
    }
}