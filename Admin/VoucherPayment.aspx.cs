using System;
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
            string id = Request.QueryString["id"];
            lblVoucherNo.Text = id;

            LoadVoucherDetails(id);
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = @"Update [PaymentVoucher] set Status = @status, [ApprovedBy] = @approvedBy,
            [DateApproved] = GETDATE()  where [PaymentVoucherID] = @voucherNo";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@status", "Approved");
            cmd.Parameters.AddWithValue("@approvedBy", lblUser.Text.Trim());
            cmd.Parameters.AddWithValue("@voucherNo", lblVoucherNo.Text.Trim());
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#successModal').modal('show');", true); 
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Vouchers.aspx");
    }

  public void LoadVoucherDetails(string voucherId)
    {

        string sql = @"Select [PayeeName], [Amount], [AmountInWords], [PaymentMethod], 
        [ChequeNo], [BankName], [AccountNo], [Description], [ReferenceNo] 
        from [PaymentVoucher] where [PaymentVoucherID] = @voucherId";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@voucherId", voucherId);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            txtFullname.Text = dr.IsDBNull(0) ? null : dr.GetString(0);
            txtAmount.Text = dr.IsDBNull(1) ? null : dr.GetDouble(1).ToString();
            txtinWords.Text = dr.IsDBNull(2) ? null : dr.GetString(2);
            drpPaymentMode.Text = dr.IsDBNull(3) ? null : dr.GetString(3);
            txtChequeNo.Text = dr.IsDBNull(4) ? null : dr.GetString(4);
            txtBank.Text = dr.IsDBNull(5) ? null : dr.GetString(5);
            txtAccountNo.Text = dr.IsDBNull(6) ? null : dr.GetString(6);
            txtDescription.Text = dr.IsDBNull(7) ? null : dr.GetString(7);
            txtReference.Text = dr.IsDBNull(8) ? null : dr.GetString(8);

        }

        dr.Close();
        dr.Dispose();

     txtFullname.Focus();
    }


    
    
   
}