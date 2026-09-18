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

            string sql = @"SELECT Distinct  RoleId, RoleName  FROM   Roles ORDER BY RoleId";
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

    public void SendEmail()
    {
        try
        {
            string fromMail = "innobuildprivatelimited@gmail.com";
            string fromPassword = "uqhnbpbbfoinelky";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Welcome back to Innobuild MIS - Account Update";

            string mailAddress = txtusername.Text.Trim();
            message.To.Add(new MailAddress(mailAddress));

            string Client = txtname.Text.Trim();
            string Username = txtusername.Text.Trim();
            string password = txtpassword.Text.Trim();

            // Generate a temporary password for security (if not already hashed)
            string tempPassword = Guid.NewGuid().ToString().Substring(0, 8);
            // In practice, you should store this hashed value in your database

            message.Body = @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Account Registration</title>
</head>
<body style='margin: 0; padding: 0; font-family: Arial, Helvetica, sans-serif; font-size: 16px; line-height: 1.6; color: #333333; background-color: #f7f7f7;'>
    <table role='presentation' cellspacing='0' cellpadding='0' border='0' align='center' width='100%' style='max-width: 600px; margin: 0 auto; padding: 20px;'>
        <!-- Header -->
        <tr>
            <td style='padding: 20px 0; text-align: center;'>
                <img src='https://apps.innosoftmw.com/Innobuild/assets/img/Innobuild%20logo%20small.png' alt='Innobuild Private Limited' width='100' style='max-width: 100%; height: auto;' />
            </td>
        </tr>
        
        <!-- White Content Box -->
        <tr>
            <td style='background-color: #ffffff; padding: 30px; border-radius: 8px; box-shadow: 0 4px 10px rgba(0,0,0,0.05);'>
                <h1 style='font-size: 24px; color: #2c3e50; margin-top: 0;'>Welcome to Innobuild MIS!</h1>
                
                <p>Dear <strong>" + Client + @"</strong>,</p>
                
                <p>Welcome back to <strong>Innobuild Private Limited Plot Management Portal</strong>. We're excited to have you back on board!</p>
                
                <p>Your account has been successfully updated. Below are your login credentials:</p>
                
                <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='margin: 20px 0; background-color: #f8f9fa; border-radius: 6px; padding: 15px;'>
                    <tr>
                        <td width='120' style='padding: 8px 0;'><strong>Username:</strong></td>
                        <td style='padding: 8px 0;'>" + Username + @"</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px 0;'><strong>Password:</strong></td>
                        <td style='padding: 8px 0;'>" + password + @"</td>
                    </tr>
                </table>
                
                <p style='background-color: #fff8e1; padding: 12px; border-left: 4px solid #ffc107; margin: 20px 0;'>
                    <strong>Security Note:</strong> For your protection, we recommend changing your password after first login.
                </p>
                
                <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='margin: 25px 0; text-align: center;'>
                    <tr>
                        <td>
                            <a href='https://apps.innosoftmw.com/lms/UserLogin.aspx' style='background-color: #2c3e50; color: #ffffff; padding: 12px 30px; text-decoration: none; border-radius: 4px; display: inline-block; font-weight: bold;'>Access Your Account</a>
                        </td>
                    </tr>
                </table>
                
                <p>If you have any questions or need assistance, our support team is here to help:</p>
                
                <ul style='padding-left: 20px;'>
                    <li><a href='https://wa.me/265882196556' style='color: #3498db;'>Whatsapp</a></li>
                    <li><a href='mailto:pempherobisai@gmail.com' style='color: #3498db;'>Email</a></li>
                </ul>
                
                <p>Best regards,<br>IT</p>
            </td>
        </tr>
        
        <!-- Footer -->
        <tr>
            <td style='padding: 20px 0; text-align: center; font-size: 12px; color: #777;'>
                <p>This email was sent to " + Username + @". If you didn't request this account, please <a href='mailto:pempherobisai@gmail.com' style='color: #3498db;'>contact us</a> immediately.</p> <br/>
               <p>© " + DateTime.Now.Year + @" Innobuild Private Limited. All rights reserved.</p>
            </td>
        </tr>
    </table>
</body>
</html>";

            message.IsBodyHtml = true;

            // Add CC for admin notification if needed
            // message.CC.Add(new MailAddress("admin@company.com"));

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            // Add error handling for email sending
            smtpClient.Send(message);

            // Log successful email sending
            // Logger.Log($"Registration email sent to {Username}");
        }
        catch (Exception ex)
        {
            // Handle errors appropriately
            // Logger.Error($"Failed to send registration email: {ex.Message}");
            // Show user-friendly error message
        }
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

                SendEmail();
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