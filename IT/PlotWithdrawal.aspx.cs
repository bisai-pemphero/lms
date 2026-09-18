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

public partial class Plotwithdrawal : System.Web.UI.Page
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

            lblPlotNo.Text = id;

            LoadPlotDetails();

            txtWithdrawal.Focus();


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

    public void LoadPlotDetails()
    {
        string sql = @"Select P.[PlotSize], P.[AgreedPrice], 
                      C.Fullname, P.[AmountPaid], C.[ClientNo], P.[SiteNo], Convert(Date, P.[DateOffered]) as Date from Plots as P
                        Join Clients as C on P.[OfferedTo] = C.[ClientNo]
                        where P.PlotNo = @plotNo";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@plotNo", lblPlotNo.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
           lblPlotSize.Text = "Plot Size: " + dr.GetString(0).ToString();
           lblSellingPrice.Text = "Price: MK" + dr.GetDouble(1).ToString("N2");
            lblPrice2.Text = dr.GetDouble(1).ToString();
            lblClientName.Text = dr.GetString(2).ToString();
            lblPaid.Text = "Paid: MK " + dr.GetDouble(3).ToString("N2");
            lblPaid2.Text = dr.GetDouble(3).ToString();

            double price = dr.GetDouble(1);
            double paid = dr.GetDouble(3);

            txtRefundAmount.Text = paid.ToString();

            lblClientCode.Text = dr.GetString(4).ToString();
            lblSiteCode.Text = dr.GetString(5).ToString();
            lblDateOffered.Text = dr.GetDateTime(6).ToString();
        }

        dr.Close();
        dr.Dispose();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtWithdrawal.Text == "")
            {
                lblError.Text = "Please specify the reason for withdrawing this plot!";
                lblSuccess.Text = "";
                txtWithdrawal.Focus();
                return;
            }

            string PlotNo = lblPlotNo.Text;


            double price = Convert.ToDouble(lblPrice2.Text.Trim());
            double paid = Convert.ToDouble(lblPaid2.Text.Trim());
            double refund = Convert.ToDouble(txtRefundAmount.Text.Trim());

            double balance = price - paid;



            string sqlWithdraw = @"Insert into [PlotWithdrawal] ([PlotNo], [SiteNo],[OfferedTo],
                                    [AgreedPrice],[AmountPaid],[Balance],[RefundAmount],
                                    [WithdrawReason],[WithdrawDate],[PostedBy],[CollectedBy],
                                    [DateAllocated])
	            VALUES (@plotNo, @siteNo, @client, @price, @paid, @balance, @refund, @reason,
	                    GETDATE(), @user, @collectedBy,  @dateAllocated)";
            SqlCommand cmdWithdraw = new SqlCommand(sqlWithdraw, appconSQL2);
            cmdWithdraw.Parameters.AddWithValue("@plotNo", PlotNo);
            cmdWithdraw.Parameters.AddWithValue("@siteNo", lblSiteCode.Text.Trim());
            cmdWithdraw.Parameters.AddWithValue("@client", lblClientCode.Text.Trim());
            cmdWithdraw.Parameters.AddWithValue("@price", price);
            cmdWithdraw.Parameters.AddWithValue("@paid", paid);
            cmdWithdraw.Parameters.AddWithValue("@balance", balance);
            cmdWithdraw.Parameters.AddWithValue("@refund", refund);
            cmdWithdraw.Parameters.AddWithValue("@reason", txtWithdrawal.Text.Trim());
            cmdWithdraw.Parameters.AddWithValue("@user", lblSession.Text.Trim());
            cmdWithdraw.Parameters.AddWithValue("@dateAllocated", lblDateOffered.Text.Trim());
            cmdWithdraw.Parameters.AddWithValue("@collectedBy", lblClientName.Text.Trim());
            cmdWithdraw.ExecuteNonQuery();
            cmdWithdraw.Dispose();

            //update Plots
            string sql = @"UPDATE  Plots SET AmountPaid='0', Balance='0', PlotStatus='Available',
                    OfferedTo='none', DateOffered=NULL, FullPaymentDate = NULL,
                    AgreedPrice='0' WHERE PlotNo ='" + PlotNo + "'";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            //delete from plot Allocations
            string sql2a = "DELETE FROM  PlotAllocations   WHERE PlotNo ='" + PlotNo + "'";
            SqlCommand cmd2a = new SqlCommand(sql2a, appconSQL2);
            cmd2a.ExecuteNonQuery();
            cmd2a.Dispose();

            //delete from Sale Agreements
            string sqlAgreements = "DELETE FROM  SaleAgreements   WHERE PlotNo ='" + PlotNo + "'";
            SqlCommand cmdAgreement = new SqlCommand(sqlAgreements, appconSQL2);
            cmdAgreement.ExecuteNonQuery();
            cmdAgreement.Dispose();

            //delete from Plot Payments
            string sqlPayments = "DELETE FROM  PlotPayments   WHERE PlotNo ='" + PlotNo + "'";
            SqlCommand cmdPayments = new SqlCommand(sqlPayments, appconSQL2);
            cmdPayments.ExecuteNonQuery();
            cmdPayments.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#successModal').modal('show');", true);
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }


    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("OfferLetterDocuments.aspx");
    }
}