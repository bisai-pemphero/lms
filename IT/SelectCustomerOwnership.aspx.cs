using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
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
            string id = Request.QueryString["id"];
            lblPlotNo.Text = id;

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

    public void LoadCustomerDetails()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["lms"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(@"SELECT 
    C.ClientNo,
    C.Fullname, 
    C.District, 
    C.Address, 
    C.PhoneNo, 
    C.Email, 
    S.PhysicalLocation, 
    P.PlotNo
FROM 
    Clients AS C
LEFT JOIN 
    Users AS U ON U.Username = C.PostedBy
LEFT JOIN 
    Plots AS P ON P.OfferedTo = C.ClientNo
LEFT JOIN 
    Sites AS S ON P.SiteNo = S.SiteCode
WHERE 
    U.Location = @district OR U.Location IS NULL;


  ", connection);
            command.Parameters.AddWithValue("@district", lblLocation.Text.Trim());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string CustomerId = reader["ClientNo"].ToString();
                string PlotId = lblPlotNo.Text.Trim();

                Response.Write("<tr>");
                Response.Write("<td>" + reader["Fullname"] + "</td>");
                Response.Write("<td>" + reader["District"] + "</td>");
                Response.Write("<td>" + reader["Address"] + "</td>");
                Response.Write("<td>" + reader["PhoneNo"] + "</td>");
                Response.Write("<td>" + reader["Email"] + "</td>");
                Response.Write("<td>" + reader["PhysicalLocation"] + "</td>");
                Response.Write("<td>" + reader["PlotNo"] + "</td>");
                Response.Write("<td>");
                Response.Write("<a href='ChangeofOwnership1.aspx?id=" + CustomerId +"|" +PlotId + "' class='btn btn-primary btn-sm'> Select </a>");
                Response.Write("</td>");
                Response.Write("</tr>");
            }

            reader.Close();
        }
    }


}