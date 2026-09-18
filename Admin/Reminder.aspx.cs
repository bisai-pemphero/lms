using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Customers : System.Web.UI.Page
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

    

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard.aspx");
    }

    public void LoadReminderLogs()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["lms"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(@"
     SELECT 
    P.PlotNo,
    S.PhysicalLocation,
    C.Fullname,
    C.PhoneNo,

    FORMAT(CAST(P.MonthlyInstallment AS DECIMAL(18,2)), 'N0') AS MonthlyInstallment,
    FORMAT(CAST(P.AgreedPrice AS DECIMAL(18,2)), 'N0') AS AgreedPrice,
    FORMAT(CAST(P.AmountPaid AS DECIMAL(18,2)), 'N0') AS AmountPaid,
    FORMAT(CAST(P.Balance AS DECIMAL(18,2)), 'N0') AS Balance,
    FORMAT(CAST(Pen.Charge AS DECIMAL(18,2)), 'N0') AS PenaltyCharge

FROM Plots P
INNER JOIN Clients C
    ON P.OfferedTo = C.ClientNo

INNER JOIN Penalties Pen
    ON CAST(P.MonthlyInstallment AS DECIMAL(18,2)) 
       BETWEEN CAST(Pen.FromRange AS DECIMAL(18,2)) 
       AND CAST(Pen.ToRange AS DECIMAL(18,2))

INNER JOIN Sites S
    ON P.SiteNo = S.SiteCode

WHERE CAST(P.OfferPeriod AS INT) > 12
AND CAST(P.Balance AS DECIMAL(18,2)) > 0
AND P.PlotStatus != 'Pending'

AND NOT EXISTS
(
    SELECT 1
    FROM PlotPayments PP
    WHERE PP.PlotNo = P.PlotNo
    AND PP.SiteNo = P.SiteNo
    AND MONTH(PP.DatePaid) = MONTH(GETDATE())
    AND YEAR(PP.DatePaid) = YEAR(GETDATE())
);
", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Response.Write("<tr>");

                Response.Write("<td>" + reader["PlotNo"] + "</td>");
                Response.Write("<td>" + reader["PhysicalLocation"] + "</td>");

                Response.Write("<td>" + reader["Fullname"] + "</td>");
                Response.Write("<td>" + reader["PhoneNo"] + "</td>");

                Response.Write("<td>" + reader["MonthlyInstallment"] + "</td>");
                Response.Write("<td>" + reader["AgreedPrice"] + "</td>");

                Response.Write("<td>" + reader["AmountPaid"] + "</td>");

                Response.Write("<td>" + reader["Balance"] + "</td>");
                Response.Write("<td>" + reader["PenaltyCharge"] + "</td>");

                Response.Write("</tr>");
            }

            reader.Close();
        }
    }

    
   

    
}