using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class Users : System.Web.UI.Page
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
            LoadRole();
            LoadLocation();
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

        string sql = "SELECT Distinct DistrictName FROM   Districts  ORDER BY DistrictName";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            drpLocation.Items.Add(dr.GetString(0));
        }

        dr.Close();

    }


    //Add new plot
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtname.Text == "")
            {
                lblError.Text = "Please enter fullname";
                txtname.Focus();
                return;
            }

            if (txtusername.Text == "")
            {
                lblError.Text = "Please enter Username!";
                txtusername.Focus();
                return;
            }
            if (txtpassword.Text == "")
            {
                lblError.Text = "Please enter password!";
                txtpassword.Focus();
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
                lblError.Text = "Please select Locations!";
                drpLocation.Focus();
                return;
            }


            string email = txtusername.Text.Trim();
            if (EmailValidator.IsValidEmail(email))
            {
                //Check if the username already exists
                string sql3 = "SELECT * FROM Users WHERE [Username]=@name";
                SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
                cmd3.Parameters.AddWithValue("@name", txtusername.Text.Trim());
                SqlDataReader dr3 = cmd3.ExecuteReader();
                if (dr3.HasRows)
                {
                    lblError.Text = "This User already exists, try changing Username!";
                    txtname.Focus();
                    dr3.Close();
                    dr3.Dispose();
                    return;
                }
                else
                {
                    dr3.Close();
                    dr3.Dispose();

                    //Insert into user table
                    string[] Roles = drpType.Text.Split('|');



                    string Userpassword = Encrypt(txtpassword.Text.Trim(), true);

                    //Insert into users table using parameterized query
                    string sql2 = @"INSERT INTO Users(Username, Password, Fullname, Location, RoleId,
                    Date_Created) VALUES(@Username, @Password, @Fullname, @location, @RoleId, GETDATE())";
                    SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
                    cmd2.Parameters.AddWithValue("@Username", txtusername.Text.Trim());
                    cmd2.Parameters.AddWithValue("@Password", Userpassword);
                    cmd2.Parameters.AddWithValue("@Fullname", txtname.Text.Trim());
                    cmd2.Parameters.AddWithValue("@RoleId", Roles[0]);
                    cmd2.Parameters.AddWithValue("@location", drpLocation.Text.Trim());
                    cmd2.ExecuteNonQuery();
                    cmd2.Dispose();

                    SendEmail();
                    Reset();
                   

                    lblError.Text = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#successModal').modal('show');", true);

                }
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
            lblError.Text = "There is an error " + ex;
        }
    }

    private void Reset()
    {
       txtname.Text = string.Empty;
        txtpassword.Text = string.Empty;
        txtusername.Text = string.Empty;
        drpLocation.SelectedIndex = -1;
        drpType.SelectedIndex = -1;
    }

    public void LoadRole()
    {
        try
        {
            drpType.Items.Clear();
            drpType.Items.Add("");

            string sql = @"SELECT Distinct RoleId, RoleName  FROM   Roles ORDER BY RoleId";
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

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#newPlot').modal('show');", true);
    }


    public void LoadSystemUsers()
    {

        string connectionString = ConfigurationManager.ConnectionStrings["lms"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(@"SELECT Distinct U.ID, U.[Username], U.[Fullname], R.[RoleName] FROM 
                                                 Users as U join Roles as R 
	                                            on U.RoleId=R.RoleId ORDER BY  Fullname", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string userId = reader.GetInt32(0).ToString(); 

                Response.Write("<tr>");
                Response.Write("<td>" + reader["Username"] + "</td>");
                Response.Write("<td>" + reader["Fullname"] + "</td>");
                Response.Write("<td>" + reader["RoleName"] + "</td>");
                Response.Write("<td>");
                Response.Write("<a href='EditUsers.aspx?id=" + userId + "' class='btn btn-block btn-primary btn-sm'> Update Details</a>");
                Response.Write("</td>"); 
                Response.Write("</tr>");
            }

            reader.Close();
        }
    }

    public void SendEmail()
    {
        try
        {
            string fromMail = "innobuildprivatelimited@gmail.com";
            string fromPassword = "uqhnbpbbfoinelky";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = "Welcome to Innobuild MIS - Account Registration";

            string mailAddress = txtusername.Text.Trim();
            message.To.Add(new MailAddress(mailAddress));

            string Client = txtname.Text.Trim();
            string Username = txtusername.Text.Trim();
            string login = lblLoginEmail.Text.Trim();
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
                
                <p>Dear <strong>" + Client +@"</strong>,</p>
                
                <p>Thank you for registering with <strong>Innobuild Private Limited Plot Management Portal</strong>. We're excited to have you on board!</p>
                
                <p>Your account has been successfully created. Below are your login credentials:</p>
                
                <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='margin: 20px 0; background-color: #f8f9fa; border-radius: 6px; padding: 15px;'>
                    <tr>
                        <td width='120' style='padding: 8px 0;'><strong>Username:</strong></td>
                        <td style='padding: 8px 0;'>" + Username + @"</td>
                    </tr>
                    <tr>
                        <td style='padding: 8px 0;'><strong>Password:</strong></td>
                        <td style='padding: 8px 0;'>" + password+ @"</td>
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
               <p>© " + DateTime.Now.Year +@" Innobuild Private Limited. All rights reserved.</p>
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

}