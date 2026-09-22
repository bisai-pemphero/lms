using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
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
            LoadDistrict();

            string id = Request.QueryString["id"];
            LoadDetails(id);
            lblClientCode.Text = id;

            LoadUser();
           
          
        }
    }
    public void LoadDistrict()
    {
        drpDistrict.Items.Clear();
        drpDistrict.Items.Add("");

        string sql = "SELECT DistrictName FROM  Districts ORDER BY DistrictName";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpDistrict.Items.Add(dr.GetString(0));
        }

        dr.Close();
        dr.Dispose();

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
   
    public void LoadDetails(string clientId)
    {

        //display details
        string sql1 = @"Select [Fullname], [District], [Address], [PhoneNo], [PhoneNo2], 
            [Email], [Occupation], [IdentityNo], [ClientNo]	from Clients where ID = @Id";
        SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
        cmd1.Parameters.AddWithValue("@Id", clientId);
        SqlDataReader dr1 = cmd1.ExecuteReader();
        while (dr1.Read())
        {
            txtFullname.Text = dr1.GetString(0);
            drpDistrict.Text = dr1.GetString(1);
            txtAddress.Text = dr1.GetString(2);
            txtPrimaryContact.Text = dr1.GetString(3);
            txtPhoneNumber2.Text = dr1.GetString(4);
            txtEmail.Text = dr1.GetString(5);
            txtOccupation.Text = dr1.GetString(6);
            txtNationalId.Text = dr1.GetString(7);
            lblClientId.Text = dr1.GetString(8);

        }
        dr1.Close();
    }

   
  
 
    

   public void Reset()
    {
        txtAddress.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtFullname.Text = string.Empty;
        txtNationalId.Text = string.Empty;
        txtOccupation.Text = string.Empty;
      
        drpDistrict.SelectedIndex = -1;
        txtPhoneNumber2.Text = string.Empty;
        txtPrimaryContact.Text = string.Empty;
    }

   
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewCustomers.aspx");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {

            if (drpDistrict.Text == "")
            {

                lblError.Text = "Please select district";
                lblSuccess.Text = "";
                drpDistrict.Focus();
                return;

            }

            if (txtFullname.Text == "")
            {

                lblError.Text = "Please enter fullname";
                lblSuccess.Text = "";
                txtFullname.Focus();

                return;

            }
            if (txtAddress.Text == "")
            {

                lblError.Text = "Please enter address";
                lblSuccess.Text = "";
                txtAddress.Focus();

                return;

            }
            if (txtPrimaryContact.Text == "")
            {
                lblError.Text = "Please enter phone no";
                lblSuccess.Text = "";
                txtPrimaryContact.Focus();
                return;
            }
            if (txtOccupation.Text == "")
            {

                lblError.Text = "Please enter Occupation";
                lblSuccess.Text = "";
                txtOccupation.Focus();
                return;
            }

            string phoneNumber = txtPrimaryContact.Text.Trim();
            string formattedNumber = FormatPhoneNumber(phoneNumber);

            if (formattedNumber.StartsWith("Invalid"))
            {
                // Handle the error case
                lblError.Text = formattedNumber;
            }
            else
            {
                lblPhoneNumber2.Text = formattedNumber;
            }


            string customerId = lblClientCode.Text.Trim();

            string email = txtEmail.Text.Trim();
            if (EmailValidator.IsValidEmail(email))
            {
                //Update Customer register
                string sql = @"Update Clients set Fullname = @fullname, District =  @district, Address =  @address,
                    PhoneNo = @primaryContact, PhoneNo2= @phoneNumber2 , Email= @email,
                    Occupation = @occupation, IdentityNo = @identity where ID = @Id";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@fullname", txtFullname.Text.Trim());
            cmd.Parameters.AddWithValue("@district", drpDistrict.Text.Trim());
            cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@primaryContact", lblPhoneNumber2.Text.Trim());
            cmd.Parameters.AddWithValue("@phoneNumber2", txtPhoneNumber2.Text.Trim());
            cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@occupation", txtOccupation.Text.Trim());
            cmd.Parameters.AddWithValue("@identity", txtNationalId.Text.Trim());
            cmd.Parameters.AddWithValue("@Id", lblClientCode.Text.Trim());
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            Reset();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#UpdateModal').modal('show');", true);
            }
            else
            {
                lblError.Text = "Please enter valid email!";
                txtEmail.Focus();
                return;
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
          
            string clientId = lblClientCode.Text.Trim();
            string plots = "";

            //Check if already exist
            string sql3 = @"Select * from Plots where OfferedTo = @customer";
            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@customer", lblClientId.Text.Trim());
            SqlDataReader dr3 = cmd3.ExecuteReader();
            if (dr3.HasRows)
            {

                lblError.Text = "Customer cannot be deleted! Withdraw Plots First.";
                lblSuccess.Text = "";
                return;

            }
            else
            {
                dr3.Close();
                dr3.Dispose();

                // Delete customer
                string sqlDelete = @"DELETE FROM Clients WHERE ID = @clientId";
                using (SqlCommand cmdDelete = new SqlCommand(sqlDelete, appconSQL2))
                {
                    cmdDelete.Parameters.AddWithValue("@clientId", clientId);
                    cmdDelete.ExecuteNonQuery();
                }

                Reset();

                // Show success modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#DeleteModal').modal('show');", true);
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

    public string FormatPhoneNumber(string inputNumber)
    {
        string outputNumber = "";

        // Remove any non-digit characters from the input number
        foreach (char c in inputNumber)
        {
            if (char.IsDigit(c))
            {
                outputNumber += c;
            }
        }

        // Check if the output number is in the desired format (12 digits)
        if (outputNumber.Length == 12)
        {
            return outputNumber;
        }
        else if (outputNumber.Length == 10)
        {
            outputNumber = "265" + outputNumber.Substring(1);
            return outputNumber;
        }
        else
        {
            return "Invalid number format! Please enter a phone number in this format: 265789789789";
        }


    }

    public class EmailValidator
    {
        public static bool IsValidEmail(string email)
        {
            // Check if the email is null or empty
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Use MailAddress class for basic validation
            try
            {
                MailAddress mail = new MailAddress(email);
            }
            catch (FormatException)
            {
                return false;
            }

            // Additional check for a valid domain extension
            if (!HasValidDomainExtension(email))
                return false;

            return true;
        }

        private static bool HasValidDomainExtension(string email)
        {
            // Regex to check for a valid domain extension (e.g., .com, .org, .co.uk)
            string pattern = @"@[a-zA-Z0-9-]+(\.[a-zA-Z]{2,})+$";
            return Regex.IsMatch(email, pattern);
        }
    }


    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewCustomers.aspx");
    }
}