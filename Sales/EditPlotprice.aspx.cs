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
        string sql1 = @"Select [PlotSize], [PlotNo], [NormalPrice]  from Plots where ID = @Id";
        SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
        cmd1.Parameters.AddWithValue("@Id", plotNo);
        SqlDataReader dr1 = cmd1.ExecuteReader();
        while (dr1.Read())
        {
            txtSize.Text = dr1.GetString(0);
            lblPlotNo.Text = dr1.GetString(1).ToString();

            string[] plot = lblPlotNo.Text.Split('/');
            txtPlotNumber.Text = plot[1];
            txtPrice.Text = dr1.GetDouble(2).ToString();
           
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
        Response.Redirect("PlotAllocation.aspx");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {

            if (txtPrice.Text == "")
            {
                lblError.Text = "Please enter new Plot Price";
                lblSuccess.Text = "";
                txtPrice.Focus();
                return;
            }

            double sellingPrice = Convert.ToDouble(txtPrice.Text);

            //Update Plot register
            string sql = @"Update Plots set [PlotValue] = @plotvalue, [NormalPrice] = @plotvalue, [PromotionPrice] = @plotvalue where ID =@id";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@id", lblPlotId.Text.Trim());
            cmd.Parameters.AddWithValue("@plotValue", sellingPrice);
            cmd.ExecuteNonQuery();
                cmd.Dispose();

            Reset();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#UpdateModal').modal('show');", true);
            
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex.Message;
        }
    }



  
}