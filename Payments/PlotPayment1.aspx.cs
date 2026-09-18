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
            lblPlotNo.Text = id;

            LoadPlotDetails(id);

            LoadUser();
            LoadReceiptNo();
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

    protected async void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
           

            if (txtAmountPaying.Text == "")
            {
                lblError.Text = "Please enter Amount Paying Now";
                lblSuccess.Text = "";
                txtAmountPaying.Focus();
                return;
            }
            if (lblReceiptNumber.Text == "")
            {
                lblError.Text = "Please enter Innobuild ReceiptNo";
                lblSuccess.Text = "";
                lblReceiptNumber.Focus();
                return;
            }

            if (drpPayMode.Text == "")
            {
                lblError.Text = "Please select Payment Mode";
                lblSuccess.Text = "";
                drpPayMode.Focus();
                return;
            }

            if (txtDate.Text == "")
            {
                lblError.Text = "Please enter Date";
                lblSuccess.Text = "";
                txtDate.Focus();
                return;
            }




            string[] Customer = lblClient.Text.Split('|');
            string ClientNo = Customer[0];
            string Fullname = Customer[1];


            double TotalPaid = Convert.ToDouble(lblTotalPaid.Text.Trim());
            double AmountPaid = Convert.ToDouble(txtAmountPaying.Text.Trim());

            double Discount;

            if (string.IsNullOrEmpty(txtdiscount.Text))
            {
                Discount = 0;
            }
            else
            {
                Discount = Convert.ToDouble(txtdiscount.Text.Trim());
            }

            double Balance = Convert.ToDouble(lblPreviousBalance.Text.Trim());


            double NewAmount = AmountPaid + Discount;


            double NewBalance = Balance - NewAmount;

            double NewTotal = TotalPaid + AmountPaid;

            if (AmountPaid > Balance)
            {
                lblError.Text = " Please check the amount entered if its not greater than Balance!";
                txtAmountPaying.Focus();
                return;
            }

            string PlotNo = lblPlotNo.Text.Trim();

            string[] SiteNo = lblPlotNo.Text.Split('/');

            DateTime paymentDate = DateTime.Parse(txtDate.Text);

            //Check if already exist
            string sql3 = @"SELECT * FROM  PlotPayments WHERE PlotNo = @plotno and PaidBy = @paidby 
                            and PaymentReference =@reference and  AmountPaid = @amount 
                            and CONVERT(DATE, DatePaid) = @date ";
            SqlCommand cmd3 = new SqlCommand(sql3, appconSQL2);
            cmd3.Parameters.AddWithValue("@plotno", PlotNo);
            cmd3.Parameters.AddWithValue("@paidby", ClientNo);
            cmd3.Parameters.AddWithValue("@amount", AmountPaid);
            cmd3.Parameters.AddWithValue("@reference", txtReference.Text);
            cmd3.Parameters.AddWithValue("@date", paymentDate); 

            SqlDataReader dr3 = cmd3.ExecuteReader();
            if (dr3.HasRows)
            {

                lblError.Text = "This transanction is already saved! Please check reprint receipt! or contact system Administrator";
                lblSuccess.Text = "";
                txtAmountPaying.Focus();
                return;

            }
            else
            {
                dr3.Close();
                dr3.Dispose();

                string sql2 = @"INSERT INTO PlotPayments(PlotNo, SiteNo, CurrentBalance, AmountPaid,
                                NewBalance, PaidBy, PaymentMode, PaymentReference, DatePaid, 
                                PostedBy, ReceiptNo)VALUES(@plotNo, @siteNo, @currentBalance, @paid, 
                                @newBalance, @paidby, @mode, @reference, @date, @user, @receiptNo)";
                SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
                cmd2.Parameters.AddWithValue("@plotNo", PlotNo);
                cmd2.Parameters.AddWithValue("@siteNo", SiteNo[0]);
                cmd2.Parameters.AddWithValue("@currentBalance", Balance);
                cmd2.Parameters.AddWithValue("@paid", AmountPaid);
                cmd2.Parameters.AddWithValue("@newBalance", NewBalance);
                cmd2.Parameters.AddWithValue("@paidby", ClientNo);
                cmd2.Parameters.AddWithValue("@mode", drpPayMode.Text.Trim());
                cmd2.Parameters.AddWithValue("@reference", txtReference.Text.Trim());
                cmd2.Parameters.AddWithValue("@user", lblSession.Text.Trim());
                cmd2.Parameters.AddWithValue("@receiptNo", lblReceiptNumber.Text.Trim());
                cmd2.Parameters.AddWithValue("@date", paymentDate);
                cmd2.ExecuteNonQuery();
                cmd2.Dispose();

                //update
                string sql = @"UPDATE  Plots SET AmountPaid= @newTotal, Balance= @balance, 
                 PlotStatus= 'Allocated' WHERE PlotNo = @plotNo";
                SqlCommand cmd = new SqlCommand(sql, appconSQL2);
                cmd.Parameters.AddWithValue("@newTotal", NewTotal);
                cmd.Parameters.AddWithValue("@balance", NewBalance);
                cmd.Parameters.AddWithValue("@plotNo", PlotNo);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //update2
                string sql2a = @"UPDATE  PlotAllocations SET AmountPaid= @newTotal WHERE PlotNo =@plotNo";
                SqlCommand cmd2a = new SqlCommand(sql2a, appconSQL2);
                cmd2a.Parameters.AddWithValue("@newTotal", NewTotal);
                cmd2a.Parameters.AddWithValue("@plotNo", PlotNo);
                cmd2a.ExecuteNonQuery();
                cmd2a.Dispose();

                //updated all completed
                string sql8 = @"UPDATE  Plots SET  PlotStatus='Completed', FullPaymentDate=GETDATE()
                                WHERE AmountPaid >= AgreedPrice AND AgreedPrice > 0";
                SqlCommand cmd8 = new SqlCommand(sql8, appconSQL2);
                cmd8.ExecuteNonQuery();
                cmd8.Dispose();

                //Update Receipt number
                string sql10 = "UPDATE ValueSequence SET ReceiptNo= ReceiptNo + 1 WHERE ID=1";
                SqlCommand cmd10 = new SqlCommand(sql10, appconSQL2);
                cmd10.ExecuteNonQuery();
                cmd10.Dispose();

                await SendSmsAsync();
                sendEmail();

                string receiptNo = lblReceiptNumber.Text.Trim();
              
                Response.Redirect("Receipt.aspx?id=" + receiptNo);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#successModal').modal('show');", true);
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }

   public void Reset()
    {
        txtAmountPaying.Text = string.Empty;
        txtdiscount.Text = string.Empty;
        txtReference.Text = string.Empty;
        txtDate.Text = string.Empty;
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("PlotPayment.aspx");
    }

  public void LoadPlotDetails(string plotNo)
    {
        string[] SiteNo = lblPlotNo.Text.Split('/');

        string sql = @"Select P.[OfferedTo], C.Fullname, P.[AmountPaid], 
                    P.[Balance], C.Email, C.[PhoneNo], C.Address,S.PhysicalLocation from Plots as P
                    Join Clients as C on C.ClientNo = P.OfferedTo 
					Join Sites as S on S.SiteCode = P.SiteNo
					where P.[PlotNo] = @plotNo";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@plotNo", plotNo);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblClient.Text = dr.GetString(0) + "|" + dr.GetString(1);
            lblTotalPaid.Text = dr.GetDouble(2).ToString();
            lblPreviousBalance.Text = dr.GetDouble(3).ToString("N2");
            lblBalance.Text = "K" + dr.GetDouble(3).ToString("N2");
            lblEmail.Text = dr.GetString(4).ToString();
            lblPhoneNumber.Text = dr.GetString(5).ToString();
            lblFor.Text = dr.GetString(1).ToUpper() + "  - PLOT #:" + SiteNo[1].ToUpper();
            lblAddress.Text = dr.GetString(6).ToString();
            txtSite.Text = dr.GetString(7).ToString();
            txtPlotNo.Text = plotNo;
        }

        dr.Close();
        dr.Dispose();

        txtAmountPaying.Focus();
    }


    public void LoadReceiptNo()
    {
        string sql = "SELECT   ReceiptNo + 1 AS Next FROM    ValueSequence";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblReceiptNumber.Text = "INBR0" + dr.GetInt32(0).ToString();
        }

        dr.Close();
        dr.Dispose();
    }


    //send sms

    private async Task<string> SendSmsAsync()
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://206.225.81.36:8989");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/messaging/sendsms");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiI2NTUiLCJvaWQiOjY1NSwidWlkIjoiN2ZhMDZlOGMtMzgzZS00ZjU5LWJmNjQtY2M1YjE3ZjA1ZmFjIiwiYXBpZCI6NTA2LCJpYXQiOjE3NjcxOTA4MDEsImV4cCI6MjEwNzE5MDgwMX0.VrLYjezPfU-WZXPyvlhU2-VKCZ3iRMFWfOkN-fzqpsJOw8EdbL1N2y0VsQU70YxooZ6QcGWkhczyo7AOHXZeJg");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //send here

            string[] CustomerName = lblClient.Text.Split('|');
         
            string customer = CustomerName[1];
          
            string invoiceNo = lblReceiptNumber.Text.Trim();

            DateTime today = DateTime.Now;

            string description = txtSite.Text.Trim() + "/" + lblPlotNo.Text.Trim();

            double TotalPaid = Convert.ToDouble(lblTotalPaid.Text.Trim());
            double AmountPaid = Convert.ToDouble(txtAmountPaying.Text.Trim());

            double Discount;

            if (string.IsNullOrEmpty(txtdiscount.Text))
            {
                Discount = 0;
            }
            else
            {
                Discount = Convert.ToDouble(txtdiscount.Text.Trim());
            }

            double Balance = Convert.ToDouble(lblPreviousBalance.Text.Trim());


            double NewAmount = AmountPaid + Discount;


            double NewBalance = Balance - NewAmount;

            double NewTotal = TotalPaid + AmountPaid;

            string price = "K" + (NewTotal + NewBalance).ToString("N2");


            string Message = "Okondedwa " + customer + " mwalipira K"  + AmountPaid + " For Plot No.:  " + description + ". Ndalama yonse imene mwapereka ndi K" + NewTotal + " Balance yotsala ndi K" + NewBalance ;

            string theTo = lblPhoneNumber.Text.Trim();
            string theMessage = Message;
            string senderId = "Innobuild";

            var json = "{ \"to\": \"" + theTo + "\", \"message\": \"" + theMessage + "\", \"from\": \"Innobuild\" }";

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                //save sent message
                string sql2 = @"Insert into [SentMessages] ([SenderId], [Contact], [TextMessage], 
                [Status], [DateSent]) Values (@senderId, @contact, @message, @status, GETDATE())";
                SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
                cmd2.Parameters.AddWithValue("@senderId", senderId);
                cmd2.Parameters.AddWithValue("@contact", theTo);
                cmd2.Parameters.AddWithValue("@message", Message);
                cmd2.Parameters.AddWithValue("@status", "Sent");
                cmd2.ExecuteNonQuery();
                cmd2.Dispose();

                string res = await response.Content.ReadAsStringAsync();
                return "SMS sent successfully: " + res;
            }
            else
            {
                //save Unsent message
                string sql2b = @"Insert into [SentMessages] ([SenderId], [Contact], [TextMessage], 
                [Status], [DateSent]) Values (@senderId, @contact, @message, @status, GETDATE())";
                SqlCommand cmd2b = new SqlCommand(sql2b, appconSQL2);
                cmd2b.Parameters.AddWithValue("@senderId", senderId);
                cmd2b.Parameters.AddWithValue("@contact", theTo);
                cmd2b.Parameters.AddWithValue("@message", Message);
                cmd2b.Parameters.AddWithValue("@status", "Not Sent");
                cmd2b.ExecuteNonQuery();
                cmd2b.Dispose();

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

        message.Subject = "PLOT PAYMENT RECEIPT";
        string mailAddress = lblEmail.Text.Trim();

        message.To.Add(new MailAddress(mailAddress));

        string[] CustomerName = lblClient.Text.Split('|');

        string client = CustomerName[1];

        string address = lblAddress.Text.Trim();

        string contact = lblPhoneNumber.Text.Trim();

        string invoiceNo = lblReceiptNumber.Text.Trim();

        DateTime today = DateTime.Now;

        string description = "Payment for Plot No. " + txtSite.Text.Trim() + "/" + lblPlotNo.Text.Trim();


        double TotalPaid = Convert.ToDouble(lblTotalPaid.Text.Trim());
        double AmountPaid = Convert.ToDouble(txtAmountPaying.Text.Trim());

        double Discount;

        if (string.IsNullOrEmpty(txtdiscount.Text))
        {
            Discount = 0;
        }
        else
        {
            Discount = Convert.ToDouble(txtdiscount.Text.Trim());
        }

        double Balance = Convert.ToDouble(lblPreviousBalance.Text.Trim());


        double NewAmount = AmountPaid + Discount;


        double NewBalance = Balance - NewAmount;

        double NewTotal = TotalPaid + AmountPaid;

        string total = "MWK" + NewAmount;

        string paymentMode = drpPayMode.Text.Trim();
        string reference = txtReference.Text.Trim();
       
        string emailTo = lblEmail.Text.Trim();


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
        .header { 
            background-color: #2c3e50; 
            padding: 25px 30px; 
            color: white; 
            border-radius: 5px 5px 0 0;
        }
        .header h1 { 
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
            text-align: right;
            margin-bottom: 20px;
        }
        .logo-container img { 
            max-width: 150px; 
            height: auto; 
        }
        .invoice-info {
            display: flex;
            justify-content: space-between;
            margin-bottom: 25px;
            flex-wrap: wrap;
        }
        .info-column {
            flex: 1;
            min-width: 250px;
            margin-bottom: 15px;
        }
        .info-label {
            font-weight: 600;
            color: #7f8c8d;
            font-size: 13px;
            text-transform: uppercase;
            margin-bottom: 5px;
            letter-spacing: 0.5px;
        }
        .info-value {
            font-size: 16px;
            color: #2c3e50;
            margin-bottom: 15px;
        }
        .section {
            margin-bottom: 25px;
        }
        .section-title {
            font-weight: 600;
            font-size: 18px;
            color: #2c3e50;
            padding-bottom: 8px;
            margin-bottom: 15px;
            border-bottom: 1px solid #e0e6ed;
        }
        .payment-details {
            background-color: #f8f9fa;
            border-radius: 5px;
            padding: 20px;
            margin-bottom: 25px;
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
            text-align: right;
            color: #2c3e50;
        }
        .total-row {
            font-weight: 700;
            font-size: 16px;
            color: #2c3e50;
            margin-top: 10px;
            padding-top: 10px;
            border-top: 2px solid #e0e6ed;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 25px;
        }
        th {
            text-align: left;
            padding: 12px 15px;
            background-color: #f8f9fa;
            color: #2c3e50;
            font-weight: 600;
            border-bottom: 2px solid #e0e6ed;
        }
        td {
            padding: 12px 15px;
            border-bottom: 1px solid #e0e6ed;
        }
        .receipt-number {
            text-align: center;
            margin: 25px 0;
            padding: 15px;
            background-color: #f8f9fa;
            border-radius: 5px;
            border: 1px dashed #dce1e5;
        }
        .receipt-number .info-label {
            margin-bottom: 8px;
        }
        .receipt-id {
            font-family: 'Courier New', monospace;
            font-size: 18px;
            letter-spacing: 2px;
            font-weight: 700;
            color: #2c3e50;
        }
        .thank-you {
            text-align: center;
            margin: 30px 0 20px;
            padding-top: 20px;
            border-top: 1px solid #e0e6ed;
            font-style: italic;
            color: #7f8c8d;
            font-size: 16px;
        }
        .contact {
            text-align: center;
            margin-top: 15px;
            font-size: 14px;
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
        <div class='header'>
            <h1>PLOT PAYMENT RECEIPT</h1>
        </div>
        
        <div class='content'>
            <div class='logo-container'>
                <img src='https://apps.innosoftmw.com/Innobuild/assets/img/Innobuild%20logo%20small.png' alt='Innobuild' />
            </div>

            <div class='invoice-info'>
                <div class='info-column'>
                    <div class='info-label'>Issued on</div>
                    <div class='info-value'>" + today.ToString("MMMM dd, yyyy") + @"</div>
                    
                    <div class='info-label'>Payment Reference</div>
                    <div class='info-value'>" + reference + @"</div>
                </div>
                
                <div class='info-column'>
                    <div class='info-label'>Payment Mode</div>
                    <div class='info-value'>" + paymentMode + @"</div>
                    
                    <div class='info-label'>Receipt Number</div>
                    <div class='info-value highlight'>" + invoiceNo + @"</div>
                </div>
            </div>

            <div class='section'>
                <div class='section-title'>Client Information</div>
                <div class='info-value'>" + client + @"</div>
                <div class='info-value'>" + address + @"</div>
                <div class='info-value'>" + contact + @"</div>
                <div class='info-value'>" + emailTo + @"</div>
            </div>

            <div class='section'>
                <div class='section-title'>Payment Details</div>
                <div class='payment-details'>
                    <div class='detail-row'>
                        <div class='detail-label'>Description - - - </div>
                        <div class='detail-value'>" + description + @"</div>
                    </div>
                    <div class='detail-row'>
                        <div class='detail-label'>Amount Paid - - - </div>
                        <div class='detail-value'><span class='currency'>MWK</span>" + NewAmount.ToString("N2") + @"</div>
                    </div>
                    <div class='detail-row'>
                        <div class='detail-label'>Discount - - - </div>
                        <div class='detail-value'><span class='currency'>MWK</span>" + Discount.ToString("N2") + @"</div>
                    </div>
                    <div class='detail-row'>
                        <div class='detail-label'>Total Plot Price - - - </div>
                        <div class='detail-value'><span class='currency'>MWK</span>" + (NewTotal + NewBalance).ToString("N2") + @"</div>
                    </div>
                    <div class='detail-row'>
                        <div class='detail-label'>Total Paid on Plot - - - </div>
                        <div class='detail-value'><span class='currency'>MWK</span>" + NewTotal.ToString("N2") + @"</div>
                    </div>
                    <div class='detail-row total-row'>
                        <div class='detail-label'>Current Balance - - - </div>
                        <div class='detail-value'><span class='currency'>MWK</span> " + NewBalance.ToString("N2") + @"</div>
                    </div>
                </div>
            </div>

            <div class='receipt-number'>
                <div class='info-label'>Payment Reference Number </div>
                <div class='receipt-id'>" + invoiceNo + @"</div>
            </div>

            <div class='thank-you'>
                <p>Thank you for your payment!</p>
            </div>

            <div class='contact'>
                <p>If you have any questions about your plot payment, please contact us at 265 991 148 500</p>
            </div>
            
            <div class='footer'>
                <p>This is an automatically generated receipt. Please keep it for your records.</p>
                <p>© " + DateTime.Now.Year + @" Innobuild. All rights reserved.</p>
            </div>
        </div>
    </div>
</body>
</html>
            
";

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