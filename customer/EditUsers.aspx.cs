using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
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
            lblUserId.Text = id;

           

            LoadLocation();
            LoadUser();
            LoadRole();

            LoadSystemUsers(id);
        }
    }

    public void LoadUser()
    {
        string sql = "select Fullname from Users where Username = @username";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@username", lblSession.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblUser.Text = dr.GetString(0).ToString();
        }

        dr.Close();
        dr.Dispose();
    }

    public void LoadLocation()
    {
        drpLocation.Items.Clear();
        drpLocation.Items.Add("");

        string sql = "SELECT   DistrictName FROM   Districts  ORDER BY DistrictName";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpLocation.Items.Add(dr.GetString(0));
        }

        dr.Close();

    }

    public void LoadSystemUsers( string id)
    {
        string sql1 = "SELECT Username, Fullname, Location, RoleId FROM Users WHERE ID = @Id";
        SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
        cmd1.Parameters.AddWithValue("@Id", id);
        SqlDataReader dr1 = cmd1.ExecuteReader();
        while (dr1.Read())
        {
            txtusername.Text = dr1.GetString(0);
            txtname.Text = dr1.GetString(1);
            drpLocation.Text = dr1.GetString(2);
            drpType.SelectedIndex = dr1.GetInt32(3);
           
        }
        dr1.Close();
    }


    public void LoadRole()
    {
        try
        {
            drpType.Items.Clear();
            drpType.Items.Add("");

            string sql = @"SELECT  RoleId, RoleName  FROM   Roles ORDER BY RoleId";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                drpType.Items.Add(dr.GetInt32(0) + "|" + dr.GetString(1));
            }

            dr.Close();
        }
        catch (Exception ex)
        {
            lblError.Text = "Loading role error" + ex;
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

    //encrypt password
    public static string Encrypt(string toEncrypt, bool useHashing)
    {
        byte[] keyArray;
        byte[] toEncryptArray = System.Text.UTF8Encoding.UTF8.GetBytes(toEncrypt);

        System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();

        string key = "06061982";

        if (useHashing)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(key));


            hashmd5.Clear();
        }
        else
            keyArray = System.Text.UTF8Encoding.UTF8.GetBytes(key);

        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

        tdes.Key = keyArray;

        tdes.Mode = CipherMode.ECB;


        tdes.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = tdes.CreateEncryptor();

        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        tdes.Clear();

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public void UpdateEmail()
    {

        string fromMail = "innobuildprivatelimited@gmail.com";
        string fromPassword = "uqhnbpbbfoinelky";
        MailMessage message = new MailMessage();
        message.From = new MailAddress(fromMail);

        message.Subject = "PASSWORD UPDATE, INNOBUILD SYSTEM";
        string mailAddress = txtusername.Text.Trim();

        message.To.Add(new MailAddress(mailAddress));
        string Client = txtname.Text.Trim();

        string Username = txtusername.Text.Trim();
        string Password = txtpassword.Text.Trim();
        string login = lblLoginEmail.Text.Trim();


        message.Body = @"
<html>
  <body style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>
    <div style='width: 100%; max-width: 600px; margin: 0 auto; border: 1px solid #ddd; border-radius: 10px; overflow: hidden;'>

      <!-- Header with company logo -->
      <div style='background-color: white; padding: 20px; text-align: center;'>
        <img src='https://apps.innosoftmw.com/pp/assets/images/product%20palace.png' alt='Ching'onga' style='max-width: 150px; height: auto;' />
      </div>

      <!-- Email Content -->
      <div style='padding: 20px;'>
        <p>Dear <strong>" + Client + @"</strong>,</p>
        <p>Your login credintials have been updated. </p>
        <p>Your login credentials for accessing the portal have been changed to the following:</p>
        <ul style='list-style-type: none; padding-left: 0;'>
          <li><strong>Email/Username:</strong> " + Username + @"</li>
          <li><strong>Password:</strong> " + Password + @"</li>
        </ul>
        <p>Please click the link below to login to the portal:</p>
        <p><a href='https://apps.innosoftmw.com/pp/UserLogin.aspx' style='background-color: #007BFF; color: #fff; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>Login to Portal</a></p>
        <p>If you have any questions or need assistance, feel free to contact us on WhatsApp: <strong><a href='https://wa.me/265882196556' style='color: #007BFF;'>Pemphero Bisai</a></strong> or via email at <a href='mailto:pempherobisai@gmail.com'> Pemphero Bisai</a>.</p>
      </div>

      <!-- Footer -->
      <div style='background-color: #f4f4f4; padding: 10px; text-align: center; font-size: 12px;'>
        <p>&copy; Innobuild Private Limited. All rights reserved.</p>
      </div>
    </div>
  </body>
</html>";


        message.IsBodyHtml = true;

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromMail, fromPassword),
            EnableSsl = true,
        };

        smtpClient.Send(message);

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            
            string UserId = lblUserId.Text.Trim();


            string sql3 = "DELETE FROM Users WHERE [ID] = @Id";
            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@Id", UserId);
            cmd3.ExecuteNonQuery();
            cmd3.Dispose();

           
            lblError.Text = "";

          
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#DeleteModal').modal('show');", true);
        }
        catch (Exception ex)
        {
            // Handle the exception here, e.g., log it or display an error message.
            lblError.Text = "User deletion error!" + ex;

        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtusername.Text))
            {
                lblError.Text = "Please enter Email";

                txtusername.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtname.Text))
            {
                lblError.Text = "Please enter Fullname";

                txtname.Focus();
                return;
            }

            if (drpType.Text == "")
            {
                lblError.Text = "Please select role!";

                drpType.Focus();
                return;
            }

            if (drpLocation.Text == "")
            {
                lblError.Text = "Please select users district!";

                drpLocation.Focus();
                return;
            }

            string UserId = lblUserId.Text.Trim();

            string[] Roles = drpType.Text.Split('|');

            string Userpassword = Encrypt(txtpassword.Text.Trim(), true);

            string email = txtusername.Text.Trim();
            if (EmailValidator.IsValidEmail(email))
            {
                // Update user account using parameterized query
                string sql = @"UPDATE [Users] set [Username] = @username, [Password]= @password, 
                            [Fullname]=@fullname, Location =@location, [RoleId]=@roleId
                            where  [ID] = @Id";
                SqlCommand cmd = new SqlCommand(sql, appconSQL2);
                cmd.Parameters.AddWithValue("@Id", UserId);
                cmd.Parameters.AddWithValue("@username", txtusername.Text.Trim());
                cmd.Parameters.AddWithValue("@password", Userpassword);
                cmd.Parameters.AddWithValue("@fullname", txtname.Text.Trim());
                cmd.Parameters.AddWithValue("@roleId", Roles[0]);
                cmd.Parameters.AddWithValue("@location", drpLocation.Text.Trim());
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                UpdateEmail();
                lblError.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#UpdateModal').modal('show');", true);
            }
            else
            {
                lblError.Text = "Please enter valid email!";
                txtusername.Focus();
                return;
            }
        }
        catch (Exception ex)
        {
            // Handle the exception here, e.g., log it or display an error message.
            lblError.Text = "User update error." + ex;

        }
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("SystemUsers.aspx");
    }


}