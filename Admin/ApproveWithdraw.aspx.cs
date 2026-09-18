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


    public void LoadPlots()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["lms"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(@" SELECT 
            PW.ID, S.PhysicalLocation, P.PlotNo,
            C.Fullname, P.AgreedPrice,  
			P.AmountPaid, P.Balance, PW.WithdrawReason, 
			PW.WithdrawDate FROM Plots AS P
			LEFT JOIN Clients AS C ON P.OfferedTo = C.ClientNo
			LEFT JOIN PlotWithdrawal AS PW on P.PlotNo = PW.PlotNo
			LEFT JOIN SITES AS S ON S.SiteCode = P.SiteNo
			Where PW.Status = @status ", connection);
            command.Parameters.AddWithValue("@status", "Pending");
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string PlotId = reader["ID"].ToString();
              
                Response.Write("<tr>");
                Response.Write("<td>" + reader["PhysicalLocation"] + "</td>");
                Response.Write("<td>" + reader["PlotNo"] + "</td>");
                Response.Write("<td>" + reader["Fullname"] + "</td>");
                Response.Write("<td>" + reader["AgreedPrice"] + "</td>");
                Response.Write("<td>" + reader["AmountPaid"] + "</td>");
                Response.Write("<td>" + reader["Balance"] + "</td>");
                Response.Write("<td>" + reader["WithdrawReason"] + "</td>");
                Response.Write("<td>" + reader["WithdrawDate"] + "</td>");
                Response.Write("<td>");
                Response.Write("<a href='PlotWithdrawal.aspx?id=" + PlotId + "' class='btn btn-primary btn-sm'> Select </a>");
                Response.Write("</td>");
                Response.Write("</tr>");
            }

            reader.Close();
        }
    }
}