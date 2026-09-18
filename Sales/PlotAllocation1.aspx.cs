using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Net.Http.Headers;
using System.Net.Http;

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
            LoadUser();

            string id = Request.QueryString["id"];

            string[] strings = id.Split(new char[] { '|' });

            lblClientCode.Text = strings[0];
            lblPlotNo.Text = strings[1];

            string[] plots = lblPlotNo.Text.Split('/');
            lblSiteCode.Text = plots[0];

            LoadPlotDetails();
            LoadOwner();
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

    public void LoadPlotDetails()
    {
        string sql = @" Select P.[PlotSize], P.[PlotValue], S.PhysicalLocation from Plots as P
                        Join Sites as S on P.SiteNo = S.SiteCode
                        where P.PlotNo = @plotNo ";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@plotNo", lblPlotNo.Text.Trim());
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
           lblPlotSize.Text = dr.GetString(0).ToString();
           lblSellingPrice.Text = dr.GetDouble(1).ToString("N2");
           lblSiteName.Text = dr.GetString(2).ToString();
        }

        dr.Close();
        dr.Dispose();

        string sql1 = @"Select Fullname, Address, Email, PhoneNo from Clients where ClientNo = @clientNo";
        SqlCommand cmd1 = new SqlCommand(sql1, appconSQL2);
        cmd1.Parameters.AddWithValue("@clientNo", lblClientCode.Text.Trim());
        SqlDataReader dr1 = cmd1.ExecuteReader();
        while (dr1.Read())
        {
            lblClientName.Text = dr1.GetString(0);
            lblAdress.Text = dr1.GetString(1);
            lblEmail.Text = dr1.GetString(2);
            lblPhone.Text = dr1.GetString(3);

        }

        dr1.Close();
        dr1.Dispose();
    }


    public void LoadOwner()
    {
        string sql3 = @"Select [OwnedbyInnobuild] from Sites Where [SiteCode] = @siteCode ";
        SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
        cmd3.Parameters.AddWithValue("@siteCode", lblSiteCode.Text.Trim());
        SqlDataReader dr3 = cmd3.ExecuteReader();
        while (dr3.Read())
        {
            lblOwner.Text = dr3.GetString(0).ToString();
        }

        dr3.Close();
        dr3.Dispose();
    }

    //async
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtAgreedPrice.Text == "0")
            {
                lblError.Text = "Please specify the plot price";
                lblSuccess.Text = "";
                txtAgreedPrice.Focus();
                return;
            }

            if (txtDuration.Text == "")
            {
                lblError.Text = "Please enter the contract duration";
                lblSuccess.Text = "";
                txtDuration.Focus();
                return;
            }

            if (drpCategory.Text == "")
            {
                lblError.Text = "Please select Sale condition!";
                lblSuccess.Text = "";
                drpCategory.Focus();
                return;
            }
            if (txtMonthlyInstallment.Text == "")
            {
                lblError.Text = "Please enter monthly Plot Installment!";
                lblSuccess.Text = "";
                txtMonthlyInstallment.Focus();
                return;
            }

            string PlotNo = lblPlotNo.Text;
            string siteCode = lblSiteCode.Text;
            string ClientNo = lblClientCode.Text;
            
            double Value = Convert.ToDouble(lblSellingPrice.Text.Trim());
            double NormalPrice = Convert.ToDouble(lblSellingPrice.Text.Trim());
            double PromotionPrice = Convert.ToDouble(txtAgreedPrice.Text.Trim());
            double AgreedPrice = Convert.ToDouble(txtAgreedPrice.Text.Trim());
            double MonthlyInstallment = Convert.ToDouble(txtMonthlyInstallment.Text.Trim());

            //insert into allocation
            string sql2 = @"INSERT INTO PlotAllocations(PlotNo, SiteNo, PriceCategory, AgreedPrice,
                            Amountpaid, ClientNo, DateAllocated, PostedBy)
                            VALUES(@plotNo, @siteNo, @category, @agreedPrice,
                            @paid, @clientNo, GETDATE(), @user)";

            SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
            cmd2.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd2.Parameters.AddWithValue("@siteNo", siteCode);
            cmd2.Parameters.AddWithValue("@category", drpCategory.Text.Trim());
            cmd2.Parameters.AddWithValue("@agreedPrice", AgreedPrice);
            cmd2.Parameters.AddWithValue("@paid", 0);
            cmd2.Parameters.AddWithValue("@clientNo", ClientNo);
            cmd2.Parameters.AddWithValue("@user", lblSession.Text.Trim());
          
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();

            string sql3 = @"Update Plots set PriceCategory = @category, PlotStatus = @status,
                          OfferedTo = @offeredTo,  DateOffered = GETDATE(), AgreedPrice = @agreedPrice, 
                          Balance = @agreedPrice, OfferPeriod = @period, 
                          MonthlyInstallment = @installment where PlotNo = @plotNo";

            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd3.Parameters.AddWithValue("@category", drpCategory.Text.Trim());
            cmd3.Parameters.AddWithValue("@agreedPrice", AgreedPrice);
            cmd3.Parameters.AddWithValue("@status", "Pending");
            cmd3.Parameters.AddWithValue("@offeredTo", ClientNo);
            cmd3.Parameters.AddWithValue("@period", txtDuration.Text.Trim());
            cmd3.Parameters.AddWithValue("@installment", MonthlyInstallment);
            cmd3.ExecuteNonQuery();
            cmd3.Dispose();

           // sendEmail();
           // await SendSmsAsync();

            //choosing offer letter
            if (lblOwner.Text == "True")
            {
              
                Response.Redirect("OfferLetter.aspx?id=" + PlotNo);
            }
            else
            {
               
                Response.Redirect("OfferLetter2.aspx?id=" + PlotNo);
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }
   public void Reset()
    {
        //
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("PlotAllocation.aspx");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
           
            if (txtAgreedPrice.Text == "0")
            {
                lblError.Text = "Please specify the plot price";
                lblSuccess.Text = "";
                txtAgreedPrice.Focus();
                return;
            }
         
            string PlotNo = "plot number";
            string siteCode = string.Empty;
            string ClientNo = string.Empty;
            string Fullname = string.Empty;

            double AgreedPrice = Convert.ToDouble(txtAgreedPrice.Text.Trim());


            //insert into allocation
            string sql2 = @"Update PlotAllocations set PriceCategory = @category,
                            CommitmentPeriod = @commPeriod, AgreedPrice =@agreedPrice, 
                            ClientNo =@clientNo, ClientFullname = @fullname, 
                            ContractDuration =@duration where PlotNo =@plotNo";
            SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
            cmd2.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd2.Parameters.AddWithValue("@category", drpCategory.Text.Trim());
          
            cmd2.Parameters.AddWithValue("@agreedPrice", AgreedPrice);
            cmd2.Parameters.AddWithValue("@clientNo", ClientNo);
            cmd2.Parameters.AddWithValue("@fullname", Fullname);
          
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();

            Reset();

            //replace with redirect to offer letter
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
            string PlotNo = "plot number";
            string siteCode = string.Empty;
            string ClientNo = string.Empty;
            string Fullname = string.Empty;


            // Delete customer
            string sqlDelete = @"DELETE FROM PlotAllocations WHERE PlotNo = @plotNo";
            using (SqlCommand cmdDelete = new SqlCommand(sqlDelete, appconSQL2))
            {
                cmdDelete.Parameters.AddWithValue("@plotNo", PlotNo);
                cmdDelete.ExecuteNonQuery();
            }

            //update plots
            string sql = @"UPDATE Plots SET PlotStatus = @status, OfferedTo= @offeredTo,   
            CommitmentPeriod= @commitment, SoldTo= @soldTo, AgreedPrice=@agreedPrice,
            Balance= @balance, DateOffered = NULL, OfferExpiryDate = NULL WHERE PlotNo = @plotNo";
            SqlCommand cmd = new SqlCommand(sql, appconSQL2);
            cmd.Parameters.AddWithValue("@status", "Available");
            cmd.Parameters.AddWithValue("@offeredTo", "none");
            cmd.Parameters.AddWithValue("@commitment", 0);
            cmd.Parameters.AddWithValue("@soldTo", "none");
            cmd.Parameters.AddWithValue("@agreedPrice", 0);
            cmd.Parameters.AddWithValue("@paid", 0);
            cmd.Parameters.AddWithValue("@balance", 0);
            cmd.Parameters.AddWithValue("@plotNo", PlotNo);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

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
            //Ensure the
        }
    }


    private async Task<string> SendSmsAsync()
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://206.225.81.36:8989");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/messaging/sendsms");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiI2NTUiLCJvaWQiOjY1NSwidWlkIjoiN2ZhMDZlOGMtMzgzZS00ZjU5LWJmNjQtY2M1YjE3ZjA1ZmFjIiwiYXBpZCI6NTA2LCJpYXQiOjE3NjcxOTA4MDEsImV4cCI6MjEwNzE5MDgwMX0.VrLYjezPfU-WZXPyvlhU2-VKCZ3iRMFWfOkN-fzqpsJOw8EdbL1N2y0VsQU70YxooZ6QcGWkhczyo7AOHXZeJg");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //send here

            string customer = lblClientName.Text.Trim();
            string Phone = lblPhone.Text.Trim();

            string description = lblSiteName.Text.ToUpper() + "/" + lblPlotNo.Text.Trim();
            string price = "K" + txtAgreedPrice.Text.Trim();

            string duration = txtDuration.Text.Trim() + " Months";
            string plotSize = lblPlotSize.Text.Trim() + " Square Meters";

            string Message = "Okondedwa " + customer + " mwagula Plot " +description  + " pamtengo wa " + price + ". Malowa ndi " + plotSize + ". Mupereka kwa " + duration + ". Zikomo podalira Innobuild kuti mupeze Plot." ;

            string theTo = lblPhone.Text;
            string theMessage = Message;

            var json = "{ \"to\": \"" + theTo + "\", \"message\": \"" + theMessage + "\", \"from\": \"Innobuild\" }";

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string res = await response.Content.ReadAsStringAsync();
                return "SMS sent successfully: " + res;
            }
            else
            {
                string err = await response.Content.ReadAsStringAsync();
                return "Failed to send SMS. Status:" + response.StatusCode + "<br/>Details:" + err;
            }
        }
    }

    public void sendEmail()
    {
        string fromMail = "innobuildprivatelimited@gmail.com";
        string fromPassword = "uqhnbpbbfoinelky";
        MailMessage message = new MailMessage();
        message.From = new MailAddress(fromMail);

        message.Subject = "PLOT ALLOCATION OFFER - INNOBUILD PRIVATE LIMITED";
        string mailAddress = lblEmail.Text.Trim();

        message.To.Add(new MailAddress(mailAddress));

        string client = lblClientName.Text.Trim();
        string address = lblAdress.Text.Trim();
        string contact = lblPhone.Text.Trim();
        string offerNo = lblPlotNo.Text.Trim();
        DateTime today = DateTime.Now;
        string emailTo = lblEmail.Text.Trim();
        
        string description = "INNOBUILD /" + lblSiteName.Text.ToUpper() + "/" + lblPlotNo.Text.Trim();
        string price =  "K" + txtAgreedPrice.Text.Trim();

        string duration = txtDuration.Text.Trim() + " Months";
        string plotSize = lblPlotSize.Text.Trim() + " Square Meters";

        message.Body = @"
    <!DOCTYPE html>
<html>
<head>
    <style>
        body { 
            font-family: 'Segoe UI', Arial, sans-serif; 
            color: #333; 
            line-height: 1.6; 
            margin: 0; 
            padding: 20px; 
            background-color: #f5f7f9; 
        }
        .email-container { 
            max-width: 700px; 
            margin: 0 auto; 
            background: white; 
        }
        .letterhead { 
            background-color: #2c3e50; 
            padding: 25px 30px; 
            color: white; 
            border-radius: 5px 5px 0 0;
            text-align: center;
        }
        .letterhead h1 { 
            margin: 0; 
            font-size: 24px; 
            font-weight: 600;
        }
        .content { 
            padding: 30px; 
            border: 1px solid #e0e6ed;
            border-top: none;
            border-radius: 0 0 5px 5px;
        }
        .logo-container {
            text-align: center;
            margin-bottom: 20px;
        }
        .logo-container img { 
            max-width: 150px; 
            height: auto; 
        }
        .letter-date {
            text-align: right;
            margin-bottom: 20px;
            color: #7f8c8d;
        }
        .client-address {
            margin-bottom: 25px;
            line-height: 1.8;
        }
        .subject-line {
            font-weight: 600;
            font-size: 18px;
            color: #2c3e50;
            margin: 25px 0 15px;
            text-align: center;
        }
        .letter-body {
            margin-bottom: 25px;
            line-height: 1.8;
        }
        .offer-details {
            background-color: #f8f9fa;
            border-radius: 5px;
            padding: 20px;
            margin: 20px 0;
        }
        .detail-row {
            display: flex;
            margin-bottom: 12px;
            padding-bottom: 12px;
            border-bottom: 1px solid #e8ecee;
        }
        .detail-row:last-child {
            margin-bottom: 0;
            padding-bottom: 0;
            border-bottom: none;
        }
        .detail-label {
            flex: 1;
            font-weight: 600;
            color: #2c3e50;
        }
        .detail-value {
            flex: 1;
            color: #2c3e50;
        }
        .acceptance-note {
            background-color: #e8f4fd;
            padding: 15px;
            border-left: 4px solid #3498db;
            margin: 20px 0;
            border-radius: 3px;
        }
        .signature-area {
            margin-top: 40px;
        }
        .signature-line {
            border-top: 1px solid #2c3e50;
            width: 250px;
            margin: 30px 0 10px;
        }
        .contact-info {
            text-align: center;
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid #e0e6ed;
            color: #7f8c8d;
        }
        .footer {
            text-align: center;
            margin-top: 30px;
            color: #95a5a6;
            font-size: 12px;
            padding: 15px;
            border-top: 1px solid #e0e6ed;
        }
        .highlight {
            color: #3498db;
            font-weight: 600;
        }
        .currency {
            font-weight: 600;
        }
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='letterhead'>
            <h1>INNOBUILD PRIVATE LIMITED</h1>
            <p>Making Malawi for Malawians First</p>
        </div>
        
        <div class='content'>
            <div class='logo-container'>
                <img src='https://apps.innosoftmw.com/Innobuild/assets/img/Innobuild%20logo%20small.png' alt='Innobuild' />
            </div>

            <div class='letter-date'>" + today.ToString("MMMM dd, yyyy") + @"</div>

            <div class='client-address'>
                <strong>" + client + @"</strong><br/>
                " + address + @"<br/>
                " + contact + @"<br/>
                " + emailTo + @"
            </div>

            <div class='subject-line'>
                OFFER OF PLOT ALLOCATION" + "<br/> " + description + @"
            </div>

            <div class='letter-body'>
                <p>Dear " + client + @",</p>

                <p>We are pleased to inform you that your application for a plot in our premium development has been successful. After careful consideration, we are delighted to offer you the following plot allocation:</p>

                <div class='offer-details'>
                    <div class='detail-row'>
                        <div class='detail-label'>Offer Reference Number:</div>
                        <div class='detail-value highlight'>" + offerNo + @"</div>
                    </div>
                    <div class='detail-row'>
                        <div class='detail-label'>Plot Details:</div>
                        <div class='detail-value'>" + description + @"</div>
                    </div>
                    <div class='detail-row'>
                        <div class='detail-label'>Plot Size:</div>
                        <div class='detail-value'>" + plotSize + @"</div>
                    </div>
                    <div class='detail-row'>
                        <div class='detail-label'>Contract duration:</div>
                        <div class='detail-value'>" + duration + @"</div>
                    </div>
                    <div class='detail-row'>
                        <div class='detail-label'>Total Plot Price:</div>
                        <div class='detail-value'><span class='currency'>MWK</span> " + price + @"</div>
                    </div>
                   
                </div>

                <p>This offer is valid for 24 hrs from the date of this letter. To secure your plot, we require your formal acceptance and commitment to the payment plan as discussed.</p>
                        
                <div class='acceptance-note'>
                    <strong>Important:</strong> Please contact our sales office within 24 Hrs to complete the necessary documentation and discuss the payment schedule.
                </div>

                <p>We believe this investment will provide you with an excellent opportunity to build your dream home in a well-planned community with modern amenities and infrastructure.</p>
            </div>
            
            <div class='signature-area'>
                <p>Yours sincerely,</p>
                <div class='signature-line'></div>
                <p><strong>Sales Manager</strong><br/>
                Innobuild Private Limited</p>
            </div>

            <div class='contact-info'>
                <p>For any queries or to accept this offer, please contact:<br/><br/>
                <strong>Phone:</strong> 265 991 148 500<br/>
                <strong>Email:</strong> innobuildprivatelimited@gmail.com</p>
            </div>
            
            <div class='footer'>
                <p>© " + DateTime.Now.Year + @" Innobuild Private Limited. All rights reserved.</p>
            </div>
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
}