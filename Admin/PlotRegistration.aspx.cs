using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

            lblPlotNo.Text = id;

            LoadPlotDetails(id);

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
            lblDutyStation.Text = dr.GetString(1).ToString();
        }

        dr.Close();
        dr.Dispose();
    }
   
    public void LoadPlotDetails(string plotNo)
    {
        string sql1 = @"Select [PlotSize],[PlotValue],[NormalPrice],[DevelopmentCost], [PlotNo]
                        from Plots where ID = @Id";
        SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
        cmd1.Parameters.AddWithValue("@Id", plotNo);
        SqlDataReader dr1 = cmd1.ExecuteReader();
        while (dr1.Read())
        {
            txtSize.Text = dr1.GetString(0);
            txtPlotValue.Text = dr1.GetDouble(1).ToString();
            txtSellingPrice.Text = dr1.GetDouble(2).ToString();
            txtDevelopmentCost.Text = dr1.GetDouble(3).ToString();
            lblPlotNoDelete.Text = dr1.GetString(4).ToString();
        }

        dr1.Close();
        dr1.Dispose();
    }


    


   public void Reset()
    {
        txtSize.Text = string.Empty;
        txtSellingPrice.Text = string.Empty;
        txtPlotValue.Text = string.Empty;
        txtDevelopmentCost.Text = string.Empty;
    }

   
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewPlots.aspx");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {

            if (txtSellingPrice.Text == "")
            {
                lblError.Text = "Please enter Selling Price";
                lblSuccess.Text = "";
                txtSellingPrice.Focus();
                return;

            }
            if (txtPlotValue.Text == "")
            {
                lblError.Text = "Please enter Plot Value";
                lblSuccess.Text = "";
                txtPlotValue.Focus();
                return;
            }

           if (txtDevelopmentCost.Text == "") txtDevelopmentCost.Text = "0";

            double plotValue = Convert.ToDouble(txtPlotValue.Text.Trim());
            double sellingPrince = Convert.ToDouble(txtSellingPrice.Text.Trim());
            double DevCost = Convert.ToDouble(txtDevelopmentCost.Text.Trim());

            string plotNo = lblPlotNo.Text.Trim();

            //Update Plot register
            string sql = @"Update Plots set PlotValue =@plotValue, NormalPrice = @sellingPrice, 
                PlotSize =@size, DevelopmentCost =@devCost  where ID =@plotNo";
                SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@plotNo", plotNo);
            cmd.Parameters.AddWithValue("@plotValue", plotValue);
            cmd.Parameters.AddWithValue("@sellingPrice", sellingPrince);
            cmd.Parameters.AddWithValue("@size", txtSize.Text.Trim());
            cmd.Parameters.AddWithValue("@devCost", DevCost);
            cmd.ExecuteNonQuery();
                cmd.Dispose();

                Reset();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#UpdateModal').modal('show');", true);
            
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
           

            string plotNo = lblPlotNo.Text.Trim();
            string allocatedTo = "";

            // Query to check if the plot is allocated
            string sqlSelect = @"SELECT OfferedTo FROM Plots WHERE PlotNo = @plotNo";
            using (SqlCommand cmdSelect = new SqlCommand(sqlSelect, appconSQL2))
            {
                cmdSelect.Parameters.AddWithValue("@plotNo", lblPlotNoDelete.Text.Trim());

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
                            string sqlDelete = @"DELETE FROM Plots WHERE ID = @plotNo";
                            using (SqlCommand cmdDelete = new SqlCommand(sqlDelete, appconSQL2))
                            {
                                cmdDelete.Parameters.AddWithValue("@plotNo", plotNo);
                                cmdDelete.ExecuteNonQuery();
                            }

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
}