using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dashboard : System.Web.UI.Page
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

        LoadUser();
        txtCurrentPassword.Focus();
       
    }

    public void LoadUser()
    {
        string sql = "select Fullname, Location, Password from Users where Username = @username";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@username", lblSession.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblUser.Text = dr.GetString(0).ToString();
            txtFullname.Text = dr.GetString(0).ToString();
            lblLocation.Text = dr.GetString(1).ToString();
            lblOldpassword.Text = dr.GetString(2).ToString();
        }

        dr.Close();
        dr.Dispose();
    }

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

        byte[] resultArray =
          cTransform.TransformFinalBlock(toEncryptArray, 0,
          toEncryptArray.Length);

        tdes.Clear();

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }






    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtCurrentPassword.Text == "")
        {
            lblError.Text = "Please enter the old password";
            lblSuccess.Text = "";
            txtCurrentPassword.Focus();
            return;
        }

        if (txtNewPassword.Text == "")
        {
            lblError.Text = "Please enter the new password";
            txtNewPassword.Text = "";
            lblSuccess.Text = "";
            return;
        }
        string Currentpassword = Encrypt(txtCurrentPassword.Text.Trim(), true);

        if (Currentpassword != lblOldpassword.Text.Trim())
        {
            lblError.Text = "Old Password Not Matching, Try Again";
            lblSuccess.Text = "";
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            return;
        }

        string sql = "UPDATE Users SET   Password='" + Encrypt(txtNewPassword.Text.Trim(), true) + "' WHERE   Username ='" + lblSession.Text.Trim() + "'";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.ExecuteNonQuery();
        cmd.Dispose();

        txtCurrentPassword.Text = "";
        txtNewPassword.Text = "";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#successModal').modal('show');", true);

    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("../UserLogin.aspx");
    }
}