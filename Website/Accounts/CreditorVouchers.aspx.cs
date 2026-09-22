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

            SqlCommand command = new SqlCommand(@"Select ID, PhysicalLocation, PreviousOwner, 
            [InitialValue], [AmountPaid], [Balance] from Sites where [Price_Per_Square_Meter] > 0", connection);
           
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string Id = reader["ID"].ToString();
               
                
                Response.Write("<tr>");
                Response.Write("<td>" + reader["PhysicalLocation"] + "</td>");
                Response.Write("<td>" + reader["PreviousOwner"] + "</td>");
                Response.Write("<td>" + reader["InitialValue"] + "</td>");
                Response.Write("<td>" + reader["AmountPaid"] + "</td>");
                Response.Write("<td>" + reader["Balance"] + "</td>");

                Response.Write("<td>");
                Response.Write("<a href='CreditorVoucherPayment.aspx?id=" + Id + "' class='btn btn-primary btn-sm'> Pay</a>");
                Response.Write("</td>");


                Response.Write("</tr>");
            }

            reader.Close();
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            double amount = Convert.ToDouble(txtAmount.Text.Trim());

            

            if (txtFullname.Text == "")
            {

                lblError.Text = "Please enter fullname";
                lblSuccess.Text = "";
                txtFullname.Focus();

                return;

            }
            if (txtAmount.Text == "")
            {

                lblError.Text = "Please enter Amount";
                lblSuccess.Text = "";
                txtAmount.Focus();

                return;

            }
            
            if (txtinWords.Text == "")
            {
                lblError.Text = "Please enter Amount in Words";
                lblSuccess.Text = "";
                txtinWords.Focus();
                return;
            }
            if (drpPaymentMode.Text == "")
            {

                lblError.Text = "Please enter Payment Mode";
                lblSuccess.Text = "";
                drpPaymentMode.Focus();
                return;
            }
            
            //Check if already exist
            string sql3 = @"Select * from PaymentVoucher where PayeeName = @name 
            and Amount = @amount and PaymentMethod = @method and DatePrepared = GETDATE();";
            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@name", txtFullname.Text);
            cmd3.Parameters.AddWithValue("@amount", amount);
            cmd3.Parameters.AddWithValue("@method", drpPaymentMode.Text.Trim());
            SqlDataReader dr3 = cmd3.ExecuteReader();
            if (dr3.HasRows)
            {

                lblError.Text = "Payment Voucher already Exists, try editing!";
                lblSuccess.Text = "";
                return;

            }
            else
            {
                dr3.Close();
                dr3.Dispose();

               
                    //insert into   customer register
                    string sql = @"Insert into [PaymentVoucher] ([PayeeName], [Amount], 
                    [AmountInWords], [PaymentMethod] ,[ChequeNo], [BankName], [AccountNo],
                    [Description], [ReferenceNo], [PreparedBy])
	                VALUES (@name, @amount, @inwords, @method, @chequeNo, @bankName, 
                    @accountNo, @description, @referenceNo, @preparedBy)";
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
                    cmd.Parameters.AddWithValue("@preparedBy", lblUser.Text.Trim());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    Reset();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#successModal').modal('show');", true);

               
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }

    public void Reset()
    {
        txtReference.Text = string.Empty;
        txtinWords.Text = string.Empty;
        txtFullname.Text = string.Empty;
        txtDescription.Text = string.Empty;
        txtChequeNo.Text = string.Empty;
        drpPaymentMode.SelectedIndex = -1;
        txtAmount.Text = string.Empty;
        txtBank.Text = string.Empty;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#newCustomer').modal('show');", true);
    }

    
}