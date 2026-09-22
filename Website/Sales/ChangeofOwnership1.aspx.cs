using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Policy;
using System.Text.RegularExpressions;
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

            string id = Request.QueryString["id"];

            string[] strings = id.Split(new char[] { '|' });

            lblClientCode.Text = strings[0];
            lblSaleAgreementId.Text = strings[1];


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


    public class SaleAgreementPlot
    {
        public string PlotNo { get; set; }
    }

    public List<SaleAgreementPlot> LoadPlots(string id)
    {
        List<SaleAgreementPlot> plots = new List<SaleAgreementPlot>();

        string sql = @"SELECT PlotNo
                   FROM SaleAgreementPlots
                   WHERE SaleAgreementId = @Id";

        using (SqlCommand cmd = new SqlCommand(sql, appconSQL2))
        {
            cmd.Parameters.AddWithValue("@Id", id);

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    plots.Add(new SaleAgreementPlot
                    {
                        PlotNo = dr["PlotNo"].ToString()
                    });
                }
            }
        }

        return plots;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if(txtProof.Text =="")
            {
                lblError.Text = "Please enter the Proof of payment!";
                return;
            }
            //check if already exists
           
            string sqlProof = @"Select * from [ChangeofOwnership] where [ProofofPayment] = @proof";
            SqlCommand cmdProof = new SqlCommand(sqlProof, appconSQL2);
            cmdProof.Parameters.AddWithValue("@proof", txtProof.Text.Trim());
            SqlDataReader drProof = cmdProof.ExecuteReader();
            if (drProof.HasRows)
            {
                Response.Redirect("ChangeofOwnershipDocument.aspx?id=" + txtProof.Text.Trim());
            }
            else
            {
                drProof.Close();
                drProof.Dispose();

                //end here
                string NewCustomer = lblClientCode.Text.Trim();
               
                string ownershipProof = txtProof.Text.Trim();
                string proof = "";

                string sql0 = "Select [ReceiptNo] from [ChangeofOwershipPayments] where  ReceiptNo = @proof";
                SqlCommand cmd0 = new SqlCommand(sql0, appconSQL2);
                cmd0.Parameters.AddWithValue("@proof", txtProof.Text.Trim());

                SqlDataReader dr0 = cmd0.ExecuteReader();
                while (dr0.Read())
                {
                    proof = dr0.GetString(0);
                }

                dr0.Close();
                dr0.Dispose();

                if (txtProof.Text.Trim() == proof)
                {
                    //insert into Change of Ownership
                    string sql = @"Insert into [ChangeofOwnership] ([Previous_Owner], [Current_Owner],
                        [SaleAgreementId], ProofofPayment)
                    select [ClientNo], @newCustomer, @saId, @proof from [SaleAgreements]  where [Id] = @saId";
                    SqlCommand cmd = new SqlCommand(sql, appconSQL2);
                    cmd.Parameters.AddWithValue("@newCustomer", NewCustomer);
                    cmd.Parameters.AddWithValue("@saId", lblSaleAgreementId.Text.Trim());
                    cmd.Parameters.AddWithValue("@proof", txtProof.Text.Trim());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //Update Sale Agreements
                    string sql4 = "Update [SaleAgreements] set [ClientNo] = @NewClient where [Id] = @Id";
                    SqlCommand cmd4 = new SqlCommand(sql4, appconSQL2);
                    cmd4.Parameters.AddWithValue("@NewClient", NewCustomer);
                    cmd4.Parameters.AddWithValue("@Id", lblSaleAgreementId.Text.Trim());
                    cmd4.ExecuteNonQuery();
                    cmd4.Dispose();

                    List<SaleAgreementPlot> plots = LoadPlots(lblSaleAgreementId.Text.Trim());

                    foreach (var plot in plots)
                    {
                        string PlotNo = plot.PlotNo;

                        //update Plots
                        string sql3 = "Update Plots Set [OfferedTo] = @NewCustomerId where PlotNo = @plotNo";
                        SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
                        cmd3.Parameters.AddWithValue("@NewCustomerId", NewCustomer);
                        cmd3.Parameters.AddWithValue("@plotNo", PlotNo);
                        cmd3.ExecuteNonQuery();
                        cmd3.Dispose();

                        //update plot payments
                        string sql2a = " Update [PlotPayments] set PaidBy = @Customer where [PlotNo] = @plotNo";
                        SqlCommand cmd2a = new SqlCommand(sql2a, appconSQL2);
                        cmd2a.Parameters.AddWithValue("@Customer", NewCustomer);
                        cmd2a.Parameters.AddWithValue("@plotNo", PlotNo);
                        cmd2a.ExecuteNonQuery();
                        cmd2a.Dispose();

                        //Update plot allocations
                        string sql2 = "Update [PlotAllocations] Set [ClientNo] = @NewCustomerId where PlotNo = @plotNo";
                        SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
                        cmd2.Parameters.AddWithValue("@NewCustomerId", NewCustomer);
                        cmd2.Parameters.AddWithValue("@plotNo", PlotNo);
                        cmd2.ExecuteNonQuery();
                        cmd2.Dispose();
                    }

                    Response.Redirect("ChangeofOwnershipDocument.aspx?id=" + ownershipProof);
                }
                else
                {
                    lblError.Text = "Please enter the proof of change of Ownership";
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }

  
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard.aspx");
    }

}