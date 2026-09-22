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
using System.Xml.Linq;

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
            lblClientCode.Text = id;

            LoadNextofKin(id);
            LoadUser();
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

    public void LoadNextofKin(string Id)
    {
        drpUpsert.Items.Clear();
        drpUpsert.Items.Add("");
        drpUpsert.Items.Add("ADD NEW");

        string sql = @"Select Id, Name from Next_of_Kins where ClientNo = @clientNo";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@clientNo", lblClientCode.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {

            drpUpsert.Items.Add(dr.GetInt32(0) + "|" + dr.GetString(1));
        }

        dr.Close();
        dr.Dispose();
    }

    public void Reset()
    {
        txtRelationship.Text = string.Empty;
        txtPrimaryContact.Text = string.Empty;
        txtFullname.Text = string.Empty;
        lblPhoneNumber2.Text = string.Empty;
        lblError.Text = string.Empty;
        lblSuccess.Text = string.Empty;
    }

   
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewCustomers.aspx");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {

            if (drpUpsert.Text == "")
            {

                lblError.Text = "Please select Action";
                lblSuccess.Text = "";
                drpUpsert.Focus();
                return;

            }

            if (txtFullname.Text == "")
            {

                lblError.Text = "Please enter fullname";
                lblSuccess.Text = "";
                txtFullname.Focus();

                return;

            }
            if (txtRelationship.Text == "")
            {

                lblError.Text = "Whats the relationship with the client?";
                lblSuccess.Text = "";
                txtRelationship.Focus();

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

            string[] nextofKin = drpUpsert.Text.Split('|');
            int nextofKinId = Convert.ToInt32(nextofKin[0]);

            //Update next of Kin
            string sql = @"Update Next_of_Kins  set [ClientNo] = @clientNo, [Name] = @name,
                            [Relationship] = @relationship, [Contact] = @primaryContact 
                            where Id = @Id";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@clientNo", customerId);
            cmd.Parameters.AddWithValue("@name", txtFullname.Text.Trim());
            cmd.Parameters.AddWithValue("@relationship", txtRelationship.Text.Trim());
            cmd.Parameters.AddWithValue("@primaryContact", lblPhoneNumber2.Text.Trim());
            cmd.Parameters.AddWithValue("@Id", nextofKinId);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            Reset();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#UpdateModal').modal('show');", true);
            
           
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

            string[] nextofKin = drpUpsert.Text.Split('|');
            int nextofKinId = Convert.ToInt32(nextofKin[0]);

            // Delete customer
            string sqlDelete = @"Delete from [Next_of_Kins] where Id = @Id";
            using (SqlCommand cmdDelete = new SqlCommand(sqlDelete, appconSQL2))
            {
                cmdDelete.Parameters.AddWithValue("@Id", nextofKinId);
                cmdDelete.ExecuteNonQuery();
            }

            Reset();

            // Show success modal
            ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#DeleteModal').modal('show');", true);
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

    protected void drpUpsert_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";

            if (drpUpsert.Text == "")
            {
                Reset();
                btnSave.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
            else if (drpUpsert.Text == "ADD NEW")
            {
                Reset();
                txtFullname.Focus();
                btnSave.Enabled = true; ;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                btnSave.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;

                string[] nextofKin = drpUpsert.Text.Split('|');
                int nextofKinId = Convert.ToInt32(nextofKin[0]);

                //display details
                string sql1 = @"Select Name,Relationship,Contact from Next_of_Kins where Id = @Id";
                SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
                cmd1.Parameters.AddWithValue("@Id", nextofKinId);
                SqlDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    txtFullname.Text = dr1.GetString(0);
                    txtRelationship.Text = dr1.GetString(1);
                    txtPrimaryContact.Text = dr1.GetString(2);
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (drpUpsert.Text == "")
            {
                lblError.Text = "Please select Action";
                lblSuccess.Text = "";
                drpUpsert.Focus();
                return;
            }

            if (txtFullname.Text == "")
            {
                lblError.Text = "Please enter fullname";
                lblSuccess.Text = "";
                txtFullname.Focus();
                return;
            }
            if (txtRelationship.Text == "")
            {
                lblError.Text = "Whats the relationship with the client?";
                lblSuccess.Text = "";
                txtRelationship.Focus();
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

            //Check if already exist
            string sql3 = @"Select * from Next_of_Kins where Name = @name and 
                                Contact = @phone and ClientNo = @clientNo";
            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@name", txtFullname.Text);
            cmd3.Parameters.AddWithValue("@phone", lblPhoneNumber2.Text);
            cmd3.Parameters.AddWithValue("@clientNo", lblClientCode.Text);
            SqlDataReader dr3 = cmd3.ExecuteReader();
            if (dr3.HasRows)
            {
                lblError.Text = "Next of Kin already Saved!";
                return;
            }
            else
            {
                dr3.Close();
                dr3.Dispose();
                //insert into Customer register
                string sql = @"Insert into Next_of_Kins ([ClientNo],[Name],[Relationship],[Contact])
                            Values (@clientNo, @name, @relationship, @primaryContact)";
                SqlCommand cmd = new SqlCommand(sql, appconSQL2);
                cmd.Parameters.AddWithValue("@clientNo", customerId);
                cmd.Parameters.AddWithValue("@name", txtFullname.Text.Trim());
                cmd.Parameters.AddWithValue("@relationship", txtRelationship.Text.Trim());
                cmd.Parameters.AddWithValue("@primaryContact", lblPhoneNumber2.Text.Trim());
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                LoadNextofKin(lblClientCode.Text.Trim());
                Reset();

                // Show success modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#SuccessModal').modal('show');", true);
            }
        }
        catch (Exception ex) {
            lblError.Text = "There is an error " + ex;
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewCustomers.aspx");
    }
}