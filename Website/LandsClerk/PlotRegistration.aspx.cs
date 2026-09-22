using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Plots : System.Web.UI.Page
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

            lblPlotId.Text = id;

            LoadPlotDetails(id);

            LoadUser();

            LoadSquarePrice();
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
            lblDutyStation.Text = dr.GetString(1).ToString();
        }

        dr.Close();
        dr.Dispose();
    }

    public void LoadPlotDetails(string plotNo)
    {
        string sql1 = @"Select [PlotSize], [PlotNo], [PlotSketch] from Plots where ID = @Id";
        SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
        cmd1.Parameters.AddWithValue("@Id", plotNo);
        SqlDataReader dr1 = cmd1.ExecuteReader();
        while (dr1.Read())
        {
            txtSize.Text = dr1.GetString(0);
            lblPlotNo.Text = dr1.GetString(1).ToString();

            string[] plot = lblPlotNo.Text.Split('/');
            txtPlotNumber.Text = plot[1];
            lblSketch.Text = dr1.IsDBNull(2) ? "" : dr1.GetString(2);
        }

        dr1.Close();
        dr1.Dispose();
    }


    public void LoadSquarePrice()
    {

        string sqlPlot = @" Select S.[Price_Per_Square_Meter] from Sites as S
                            Join Plots as P on S.SiteCode = P.SiteNo
                            where P.ID = @id";
        SqlCommand cmdPlor = new SqlCommand(sqlPlot, appconSQL2);
        cmdPlor.Parameters.AddWithValue("@id", lblPlotId.Text.Trim());
        SqlDataReader drPlot = cmdPlor.ExecuteReader();
        while (drPlot.Read())
        {
            lblPricePerSquareMeter.Text = drPlot.GetDouble(0).ToString();
        }

        drPlot.Close();
        drPlot.Dispose();
    }
    public void Reset()
    {
        txtSize.Text = string.Empty;
    }


    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewPlots.aspx");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {

            if (fnSketchmap.FileContent == null)
            {
                return;
            }
            else
            {
                DeleteSketch(lblSketch.Text.Trim());
            }

            if ((fnSketchmap.PostedFile != null) && (fnSketchmap.PostedFile.ContentLength > 0))
            {
                string lblFormat = "";
                string fn = System.IO.Path.GetFileName(fnSketchmap.PostedFile.FileName);
                string SaveLocation = Server.MapPath("~\\assets\\docs") + "\\" + fn;

                lblFormat = fn.Substring(fn.Length - 3, 3);

                if (txtSize.Text == "")
                {
                    lblError.Text = "Please enter Plot Size";
                    lblSuccess.Text = "";
                    txtSize.Focus();
                    return;

                }
                if (txtPlotNumber.Text == "")
                {
                    lblError.Text = "Please enter Plot Number";
                    lblSuccess.Text = "";
                    txtPlotNumber.Focus();
                    return;

                }

                //calculate plot price based on entered Square meters
                double plotSize = Convert.ToDouble(txtSize.Text.Trim());
                double perSQM = Convert.ToDouble(lblPricePerSquareMeter.Text.Trim());

                double sellingPrince = plotSize * perSQM;

                string[] plot = lblPlotNo.Text.Split('/');
                string site = plot[0];

                string plotNo = site + "/" + txtPlotNumber.Text.Trim();

                //Update Plot register
                string sql = @"Update Plots set PlotValue =@plotValue, NormalPrice = @sellingPrice, 
            PlotSize =@size, [PlotNo] = @plotNo, PlotSketch = @sketch where ID =@id";
                SqlCommand cmd = new SqlCommand(sql, appconSQL2);
                cmd.Parameters.AddWithValue("@id", lblPlotId.Text.Trim());
                cmd.Parameters.AddWithValue("@plotValue", sellingPrince);
                cmd.Parameters.AddWithValue("@sellingPrice", sellingPrince);
                cmd.Parameters.AddWithValue("@size", txtSize.Text.Trim());
                cmd.Parameters.AddWithValue("@plotNo", plotNo);
                cmd.Parameters.AddWithValue("@sketch", fn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();


                string sql1 = @"Update PlotAllocations set PlotNo = @newplotNo where PlotNo = @oldPlotNo";
                SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
                cmd1.Parameters.AddWithValue("@oldPlotNo", lblPlotNo.Text.Trim());
                cmd1.Parameters.AddWithValue("@newplotNo", plotNo);
                cmd1.ExecuteNonQuery();
                cmd1.Dispose();


                fnSketchmap.PostedFile.SaveAs(SaveLocation);

                Reset();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#UpdateModal').modal('show');", true);

            }
            else
            {
                lblError.Text = "PLEASE SELECT FILE TO UPLOAD";
                fnSketchmap.Focus();
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }



    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string allocatedTo = "";

            // Query to check if the plot is allocated
            string sqlSelect = @"SELECT OfferedTo FROM Plots WHERE ID = @id";
            using (SqlCommand cmdSelect = new SqlCommand(sqlSelect, appconSQL2))
            {
                cmdSelect.Parameters.AddWithValue("@id", lblPlotId.Text.Trim());

                // Open the connection if it's not already open
                if (appconSQL2.State != ConnectionState.Open)
                {
                    appconSQL2.Open();
                }

                using (SqlDataReader drSelect = cmdSelect.ExecuteReader())
                {
                    if (drSelect.Read())
                    {
                        allocatedTo = drSelect.GetString(0);

                        if (allocatedTo == "none")
                        {
                            // Close the reader before executing the DELETE command
                            drSelect.Close();

                            // Delete the plot if it's not allocated
                            string sqlDelete = @"DELETE FROM Plots WHERE ID = @id";
                            using (SqlCommand cmdDelete = new SqlCommand(sqlDelete, appconSQL2))
                            {
                                cmdDelete.Parameters.AddWithValue("@id", lblPlotId.Text.Trim());
                                cmdDelete.ExecuteNonQuery();
                            }

                            DeleteSketch(lblSketch.Text.Trim());

                            Reset();

                            // Show success modal
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#DeleteModal').modal('show');", true);
                        }
                        else
                        {
                            lblError.Text = "This plot is already allocated to a Customer, please withdraw first!";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "There was an error: " + ex.Message;
        }
        finally
        {
            // Ensure the
        }
    }

    public void DeleteSketch(string sketch)
    {
        // physical paths
        string sketchPath = Server.MapPath("~/assets/docs/") + sketch;

        try
        {
            if (File.Exists(sketchPath))
            {
                File.Delete(sketchPath);
            }
        }
        catch (IOException ioEx)
        {
            // log or surface the error as needed
            lblError.Text = "File‑system error: " + ioEx.Message;
        }
    }

    protected void btnUpdateSketch_Click(object sender, EventArgs e)
    {
        DeleteSketch(lblSketch.Text.Trim());

        if ((fnSketchmap.PostedFile != null) && (fnSketchmap.PostedFile.ContentLength > 0))
        {
            string lblFormat = "";
            string fn = System.IO.Path.GetFileName(fnSketchmap.PostedFile.FileName);
            string SaveLocation = Server.MapPath("~\\assets\\docs") + "\\" + fn;

            lblFormat = fn.Substring(fn.Length - 3, 3);

            //Update Plot register
            string sql = @"Update Plots set PlotSketch = @sketch where ID =@id";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@id", lblPlotId.Text.Trim());
            cmd.Parameters.AddWithValue("@sketch", fn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            fnSketchmap.PostedFile.SaveAs(SaveLocation);

            Reset();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#UpdateModal').modal('show');", true);
        }
        else
        {
            lblError.Text = "PLEASE SELECT FILE TO UPLOAD";
            fnSketchmap.Focus();
        }
    }
}