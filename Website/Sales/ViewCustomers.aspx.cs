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
            LoadClientCode();
            LoadDistrict();
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

            SqlCommand command = new SqlCommand(@"SELECT 
    C.ID,
    C.Fullname, 
    C.Address, 
    C.PhoneNo, 
    S.PhysicalLocation, 
    P.PlotNo,
    C.ClientNo
FROM 
    Clients AS C
LEFT JOIN 
    Users AS U ON U.Username = C.PostedBy
LEFT JOIN 
    Plots AS P ON P.OfferedTo = C.ClientNo
LEFT JOIN 
    Sites AS S ON P.SiteNo = S.SiteCode;


  ", connection);
            command.Parameters.AddWithValue("@district", lblLocation.Text.Trim());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string Id = reader["ID"].ToString();
                string clientId = reader["ClientNo"].ToString();

                Response.Write("<tr>");
                Response.Write("<td>" + reader["Fullname"] + "</td>");
                Response.Write("<td>" + reader["Address"] + "</td>");
                Response.Write("<td>" + reader["PhoneNo"] + "</td>");
                Response.Write("<td>" + reader["PhysicalLocation"] + "</td>");
                Response.Write("<td>" + reader["PlotNo"] + "</td>");
                Response.Write("<td>");
                Response.Write("<a href='CustomerRegistration.aspx?id=" + Id + "' class='btn btn-primary btn-sm'> Edit</a>");
                Response.Write("</td>");
                Response.Write("<td>");
                Response.Write("<a href='AddNextofKin.aspx?id=" + clientId + "' class='btn btn-primary btn-sm'> Next of Kin</a>");
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
            if (txtEmail.Text == "")
            {

                lblError.Text = "Please enter clients email address";
                lblSuccess.Text = "";
                txtEmail.Focus();
                return;
            }
            if (txtOccupation.Text == "")
            {

                lblError.Text = "Please enter Occupation";
                lblSuccess.Text = "";
                txtOccupation.Focus();
                return;
            }
            if (drpDistrict.Text == "")
            {

                lblError.Text = "Please select district";
                lblSuccess.Text = "";
                drpDistrict.Focus();
                return;

            }

            if (lblClientCode.Text == "")
            {
                LoadClientCode();
                return;
            }
            if (lblPhoneNumber2.Text == "")
            {
                lblError.Text = "Please enter phone no";
                lblSuccess.Text = "";
                txtPrimaryContact.Focus();
                return;
            }

            //Check if already exist
            string sql3 = "SELECT * FROM Clients WHERE Fullname= @fullname and PhoneNo = @phoneNo and Address = @address";
            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@fullname", txtFullname.Text);
            cmd3.Parameters.AddWithValue("@phoneNo", lblPhoneNumber2.Text.Trim());
            cmd3.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
            SqlDataReader dr3 = cmd3.ExecuteReader();
            if (dr3.HasRows)
            {

                lblError.Text = "Customer already registered!";
                lblSuccess.Text = "";
                return;

            }
            else
            {
                dr3.Close();
                dr3.Dispose();

                string email = txtEmail.Text.Trim();
                if (EmailValidator.IsValidEmail(email))
                {
                    //insert into   customer register
                    string sql = @"INSERT INTO  Clients(ClientNo, Fullname, District, Address,
                    PhoneNo, PhoneNo2, Email, Occupation, IdentityNo, PostedBy) VALUES
                    (@clientNo, @fullname, @district, @address, @primaryContact, @phoneNumber2, 
                    @email, @occupation, @identity, @user)";
                    SqlCommand cmd = new SqlCommand(sql, appconSQL2);
                    cmd.Parameters.AddWithValue("@clientNo", lblClientCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@fullname", txtFullname.Text.Trim());
                    cmd.Parameters.AddWithValue("@district", drpDistrict.Text.Trim());
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@primaryContact", lblPhoneNumber2.Text.Trim());
                    cmd.Parameters.AddWithValue("@phoneNumber2", txtPhoneNumber2.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@occupation", txtOccupation.Text.Trim());
                    cmd.Parameters.AddWithValue("@identity", txtNationalId.Text.Trim());
                    cmd.Parameters.AddWithValue("@user", lblSession.Text.Trim());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //update site code in vsequnce
                    string sql2 = "UPDATE ValueSequence SET    ClientNo= ClientNo + 1 WHERE ID=1";
                    SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
                    cmd2.ExecuteNonQuery();
                    cmd2.Dispose();

                    Reset();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#successModal').modal('show');", true);

                }
                else
                {
                    lblError.Text = "Please enter valid email!";
                    txtEmail.Focus();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
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

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#newCustomer').modal('show');", true);
    }

    public string FormatPhoneNumber(string inputNumber)
    {
        if (string.IsNullOrWhiteSpace(inputNumber))
        {
            return "Invalid number format!";
        }

        // Remove spaces, dashes, brackets, etc.
        string cleanedNumber = "";

        foreach (char c in inputNumber)
        {
            if (char.IsDigit(c))
            {
                cleanedNumber += c;
            }
        }

     
        if (cleanedNumber.Length == 10 && cleanedNumber.StartsWith("0"))
        {
            cleanedNumber = "265" + cleanedNumber.Substring(1);
        }

        if (cleanedNumber.Length >= 10 && cleanedNumber.Length <= 15)
        {
            return cleanedNumber;
        }

        return "Invalid number format! Please enter a valid local or international phone number.";
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

    public void LoadClientCode()
    {
        string sql = "SELECT ClientNo + 1 AS Next FROM ValueSequence";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblClientCode.Text = "INC0" + dr.GetInt32(0).ToString();
        }

        dr.Close();
        dr.Dispose();
    }

    public void LoadDistrict()
    {
        drpDistrict.Items.Clear();
        drpDistrict.Items.Add("");

        string sql = "SELECT Distinct DistrictName FROM  Districts ORDER BY DistrictName";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpDistrict.Items.Add(dr.GetString(0));
        }

        dr.Close();
        dr.Dispose();

    }
}