using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
            string id = Request.QueryString["id"];
            lblClientNo.Text = id;
            LoadUser();
            LoadClientInfo();
        }
    }

    public void LoadUser()
    {
        string sql = "select Fullname from Users where Username = @username";
        using (SqlCommand cmd = new SqlCommand(sql, appconSQL2))
        {
            cmd.Parameters.AddWithValue("@username", lblSession.Text.Trim());
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    lblUser.Text = dr.GetString(0).ToString();
                }
            }
        }
    }

    private void LoadClientInfo()
    {
     
        string sql = "SELECT ClientNo, FullName FROM Clients WHERE ClientNo = @clientNo";
        using (SqlCommand cmd = new SqlCommand(sql, appconSQL2))
        {
            cmd.Parameters.AddWithValue("@clientNo", lblClientNo.Text.Trim());
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    lblClient.Text = dr["FullName"].ToString();
                   
                }
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
      
        lblError.Text = string.Empty;

     
        using (SqlTransaction transaction = appconSQL2.BeginTransaction())
        {
            try
            {
                string year = DateTime.Now.ToString("yyyy");
                string date = DateTime.Now.ToString("dddd, dd MMMM yyyy");
                string clientNo = lblClientNo.Text.Trim();

                // 1. Check if SaleAgreement already exists for this client
                string checkAgreementSql = @"SELECT COUNT(*) FROM SaleAgreements 
                                        WHERE ClientNo = @clientNo AND OfferYear = @year";
                using (SqlCommand checkCmd = new SqlCommand(checkAgreementSql, appconSQL2, transaction))
                {
                    checkCmd.Parameters.AddWithValue("@clientNo", clientNo);
                    checkCmd.Parameters.AddWithValue("@year", year);

                    int existingCount = (int)checkCmd.ExecuteScalar();
                    if (existingCount > 0)
                    {
                        lblError.Text = "A Sale Agreement for this client already exists for the current year!";
                        transaction.Rollback();
                        return;
                    }
                }

                // 2. Get the plots that need to be included - PASS THE TRANSACTION
                List<Plots> plots = GetPlotsForAgreement(clientNo, transaction);

                if (plots.Count == 0)
                {
                    lblError.Text = "No completed plots found for this client to create an agreement!";
                    transaction.Rollback();
                    return;
                }

                // 3. Check if any of these plots already have agreements
                foreach (var plot in plots)
                {
                    string checkPlotSql = @"SELECT COUNT(*) FROM SaleAgreementPlots 
                                       WHERE SiteNo = @siteNo AND PlotNo = @plotNo";
                    using (SqlCommand checkPlotCmd = new SqlCommand(checkPlotSql, appconSQL2, transaction))
                    {
                        checkPlotCmd.Parameters.AddWithValue("@siteNo", plot.SiteNo);
                        checkPlotCmd.Parameters.AddWithValue("@plotNo", plot.PlotNo);

                        int plotExists = (int)checkPlotCmd.ExecuteScalar();
                        if (plotExists > 0)
                        {
                            lblError.Text = "Plot " + plot.PlotNo + " Site " + plot.SiteNo + " already has an agreement!";
                            transaction.Rollback();
                            return;
                        }
                    }
                }

                // 4. Insert the SaleAgreement header
                string insertAgreementSql = @"INSERT INTO [SaleAgreements] ([ClientNo], [OfferDate], [OfferYear], [Createdby], [Createdon])
                                         VALUES (@client, @date, @year, @user, GETDATE());
                                         SELECT SCOPE_IDENTITY();";

                int agreementId;
                using (SqlCommand insertCmd = new SqlCommand(insertAgreementSql, appconSQL2, transaction))
                {
                    insertCmd.Parameters.AddWithValue("@client", clientNo);
                    insertCmd.Parameters.AddWithValue("@date", date);
                    insertCmd.Parameters.AddWithValue("@year", year);
                    insertCmd.Parameters.AddWithValue("@user", lblSession.Text.Trim());

                    agreementId = Convert.ToInt32(insertCmd.ExecuteScalar());
                }

                // 5. Insert all plots for this agreement
                foreach (var plot in plots)
                {
                    string insertPlotSql = @"INSERT INTO SaleAgreementPlots 
                                        (SaleAgreementId, SiteNo, PlotNo)
                                        VALUES (@agreementId, @siteNo, @plotNo)";

                    using (SqlCommand plotCmd = new SqlCommand(insertPlotSql, appconSQL2, transaction))
                    {
                        plotCmd.Parameters.AddWithValue("@agreementId", agreementId);
                        plotCmd.Parameters.AddWithValue("@siteNo", plot.SiteNo);
                        plotCmd.Parameters.AddWithValue("@plotNo", plot.PlotNo);
                     
                        plotCmd.ExecuteNonQuery();
                    }
                }

               
                transaction.Commit();


                Session["AgreementId"] = agreementId.ToString();

             
                Response.Redirect("SaleAgreement.aspx?id=" + agreementId);
            }
            catch (Exception ex)
            {
               
                try { transaction.Rollback(); }
                catch { /* If rollback fails, we can't do much */ }

                lblError.Text = "Error creating Sale Agreement: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;

                // Log the error
                LogError(ex);
            }
        }
    }

 
    private List<Plots> GetPlotsForAgreement(string clientNo, SqlTransaction transaction)
    {
        List<Plots> plots = new List<Plots>();

        string query = @"
        SELECT P.PlotNo, P.SiteNo 
        FROM Plots AS P 
        WHERE P.OfferedTo = @clientNo 
        AND P.PlotStatus = @status
        AND NOT EXISTS (
            SELECT 1 FROM SaleAgreementPlots SAP 
            WHERE SAP.SiteNo = P.SiteNo 
            AND SAP.PlotNo = P.PlotNo
        )";

      
        using (SqlCommand command = new SqlCommand(query, appconSQL2, transaction))
        {
            command.Parameters.AddWithValue("@clientNo", clientNo);
            command.Parameters.AddWithValue("@status", "Completed");

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    plots.Add(new Plots
                    {
                        SiteNo = reader["SiteNo"].ToString(),
                        PlotNo = reader["PlotNo"].ToString(),
                    });
                }
            }
        }
        return plots;
    }

    private void LogError(Exception ex)
    {
        try
        {
          
            string logDirectory = Server.MapPath("~/Logs/");
            if (!System.IO.Directory.Exists(logDirectory))
            {
                System.IO.Directory.CreateDirectory(logDirectory);
            }

            string logPath = System.IO.Path.Combine(logDirectory, "ErrorLog.txt");
            string logMessage = string.Format("{0}: {1}\n{2}\n\n",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ex.Message,
                ex.StackTrace);

            System.IO.File.AppendAllText(logPath, logMessage);
        }
        catch
        {
            // If logging fails, at least don't crash the application
        }
    }

   
    protected void Page_Unload(object sender, EventArgs e)
    {
        // Ensure connections are closed
        if (appconSQL2 != null && appconSQL2.State == ConnectionState.Open)
        {
            appconSQL2.Close();
            appconSQL2.Dispose();
        }
    }

    public class Plots
    {
        public string SiteNo { get; set; }
        public string PlotNo { get; set; }
    }
}