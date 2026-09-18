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

            LoadPayments(lblSiteCode.Text.Trim());

            string Update = CheckApproval(id);

            if(Update == "Paid")
            {
                btnSave.Visible = true;
                btnBack.Visible = false;
            }else
            {
                lblError.Text = "Sorry, you cannot edit this transanction! Approve first";
                btnSave.Visible = false;
                btnBack.Visible = true;

            }

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

    public void LoadPayments(string id)
    {
        string sql = "Select [InitialValue], [AmountPaid] from Sites where ID = @id";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@id", id);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblInitialValue.Text = dr.GetDouble(0).ToString();
            lblPaid.Text = dr.GetDouble(1).ToString();
        }

        dr.Close();
        dr.Dispose();
    }
    public string CheckApproval(string id)
    {
        string sql = @"SELECT Status 
                   FROM [PaymentVoucherCreditor] 
                   WHERE [PaymentVoucherID] = @Id";

        using (SqlCommand cmd = new SqlCommand(sql, appconSQL2))
        {
            cmd.Parameters.AddWithValue("@Id", id);

            object result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                return result.ToString();
            }

            return null; 
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            double amount = Convert.ToDouble(txtAmount.Text.Trim());

            double amountPaying = Convert.ToDouble(txtAmount.Text.Trim()); //paying now
            double initialValue = Convert.ToDouble(lblInitialValue.Text.Trim()); //plot value
            double paid = Convert.ToDouble(lblPaid.Text.Trim()); //previous payments
            double prev = Convert.ToDouble(lblPrevious.Text.Trim());

            double allPaid = amountPaying + (paid - prev);

            double newBalance = initialValue - allPaid;



            string sql = @"Update [PaymentVoucherCreditor] set [PayeeName] = @name, [Amount] = @amount, 
            [AmountInWords] = @inwords, [PaymentMethod] = @method, [ChequeNo] = @chequeNo,
            [BankName] = @bankName, [AccountNo] = @accountNo, [Description] = @description, 
            [ReferenceNo] = @referenceNo where [PaymentVoucherID] = @voucherNo";

            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@name", txtFullname.Text.Trim());
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@inwords", txtinWords.Text.Trim());
            cmd.Parameters.AddWithValue("@method", drpPaymentMode.Text.Trim());
            cmd.Parameters.AddWithValue("@chequeNo", txtChequeNo.Text.Trim());
            cmd.Parameters.AddWithValue("@bankName", txtBank.Text.Trim());
            cmd.Parameters.AddWithValue("@accountNo", txtAccountNo.Text.Trim());
            cmd.Parameters.AddWithValue("@description", txtDescription.Text.Trim());
            cmd.Parameters.AddWithValue("@referenceNo", txtReference.Text.Trim());
            cmd.Parameters.AddWithValue("@voucherNo", lblVoucherNo.Text.Trim());
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            string sql0 = @"Update Sites set AmountPaid = @amount, Balance = @balance where ID = @id";
            SqlCommand cmd0 = new SqlCommand(sql0, appconSQL2);
            cmd0.Parameters.AddWithValue("@amount", allPaid);
            cmd0.Parameters.AddWithValue("@balance", newBalance);
            cmd0.Parameters.AddWithValue("@id", lblSiteCode.Text.Trim());
            cmd0.ExecuteNonQuery();
            cmd0.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#successModal').modal('show');", true); 
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreditorVouchers.aspx");
    }

  public void LoadVoucherDetails(string voucherId)
    {

        string sql = @"Select [PayeeName], [Amount], [AmountInWords], [PaymentMethod]
      ,[ChequeNo], [BankName], [AccountNo], [Description], [ReferenceNo], [SiteCode]
        from [PaymentVoucherCreditor] where [PaymentVoucherID] = @voucherId";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@voucherId", voucherId);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            txtFullname.Text = dr.IsDBNull(0) ? null : dr.GetString(0);
            txtAmount.Text = dr.IsDBNull(1) ? null : dr.GetDouble(1).ToString();
            lblPrevious.Text = dr.IsDBNull(1) ? null : dr.GetDouble(1).ToString();
            txtinWords.Text = dr.IsDBNull(2) ? null : dr.GetString(2);
            drpPaymentMode.Text = dr.IsDBNull(3) ? null : dr.GetString(3);
            txtChequeNo.Text = dr.IsDBNull(4) ? null : dr.GetString(4);
            txtBank.Text = dr.IsDBNull(5) ? null : dr.GetString(5);
            txtAccountNo.Text = dr.IsDBNull(6) ? null : dr.GetString(6);
            txtDescription.Text = dr.IsDBNull(7) ? null : dr.GetString(7);
            txtReference.Text = dr.IsDBNull(8) ? null : dr.GetString(8);
            lblSiteCode.Text = dr.IsDBNull(9) ? null : dr.GetInt32(9).ToString();
        }

        dr.Close();
        dr.Dispose();

     txtFullname.Focus();
    }

}