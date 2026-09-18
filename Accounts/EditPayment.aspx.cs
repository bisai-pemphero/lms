using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Security.Policy;
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
            string id = Request.QueryString["id"];
           
            lblId.Text = id;


            LoadPlotDetails(id);

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
            lblDutyStation.Text = dr.GetString(1).ToString();
        }

        dr.Close();
        dr.Dispose();
    }

   public void Reset()
    {
        txtAmountPaying.Text = string.Empty;
        txtdiscount.Text = string.Empty;
        txtReference.Text = string.Empty;
    }

   
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("PlotPayment.aspx");
    }

   
  

  public void LoadPlotDetails(string Id)
    {
        double Balance = 0;
        double TotalPaid = 0;

        string sql = @"Select P.AmountPaid as Total, 
        P.Balance as CurrentBalance, 
	    PP.AmountPaid as Paid, PP.PaymentMode, 
        PP.PaymentReference,
	    C.[PhoneNo], C.[Email], PP.PlotNo, PP.[ReceiptNo], 
        C.Fullname, C.Address, S.PhysicalLocation
	    from Plots as P
        Join PlotPayments as PP on PP.PlotNo = P.PlotNo
        Join Clients as C on C.ClientNo = PP.PaidBy
        Join Sites as S on PP.SiteNo = S.SiteCode
        where PP.ID = @Id";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@Id", Id);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            TotalPaid = dr.GetDouble(0);
            Balance = dr.GetDouble(1);
            txtAmountPaying.Text = dr.GetDouble(2).ToString();
            drpPayMode.Text = dr.GetString(3);
            txtReference.Text = dr.GetString(4);
            lblPhoneNumber.Text = dr.GetString(5);
            lblEmail.Text = dr.GetString(6);
            lblPlotNo.Text = dr.GetString(7);
            lblReceiptNumber.Text = dr.GetString(8);
            lblFor.Text = dr.GetString(9).ToUpper();
            lblAddress.Text = dr.GetString(10);
            txtSite.Text = dr.GetString(11);

            double amountPaying = Convert.ToDouble(txtAmountPaying.Text);
            double paid = TotalPaid - amountPaying;

            double balanceforPlot = Balance + amountPaying;

            lblTotalPaid.Text = paid.ToString("N2");
            lblPreviousBalance.Text = balanceforPlot.ToString("N2");
        }

        dr.Close();
        dr.Dispose();

        txtAmountPaying.Focus();
    }

   

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        //update the receipt
        try
        {
            if (txtAmountPaying.Text == "")
            {
                lblError.Text = "Please enter Amount Paying Now";
                lblSuccess.Text = "";
                txtAmountPaying.Focus();
                return;
            }

            if (drpPayMode.Text == "")
            {
                lblError.Text = "Please select Payment Mode";
                lblSuccess.Text = "";
                drpPayMode.Focus();
                return;
            }


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
            int id = Convert.ToInt32(lblId.Text.Trim());

                string sql2 = @"UPDATE PlotPayments set CurrentBalance = @currentBalance,
                            AmountPaid = @paid,  NewBalance = @newBalance,
                            PaymentMode = @mode, PaymentReference =@reference where ID = @Id";
                SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
                cmd2.Parameters.AddWithValue("@Id", id);
                cmd2.Parameters.AddWithValue("@currentBalance", Balance);
                cmd2.Parameters.AddWithValue("@paid", AmountPaid);
                cmd2.Parameters.AddWithValue("@newBalance", NewBalance);
                cmd2.Parameters.AddWithValue("@mode", drpPayMode.Text.Trim());
                cmd2.Parameters.AddWithValue("@reference", txtReference.Text.Trim());
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

                sendEmail();

                string receiptNo = lblReceiptNumber.Text.Trim();

                Response.Redirect("Receipt.aspx?id=" + receiptNo);
        }
        catch (Exception ex)
        {
            lblError.Text = "There is an error " + ex;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        double TotalPaid = Convert.ToDouble(lblTotalPaid.Text.Trim());

        double Balance = Convert.ToDouble(lblPreviousBalance.Text.Trim());

        string PlotNo = lblPlotNo.Text.Trim();

        //update
        string sql = @"UPDATE  Plots SET AmountPaid= @newTotal, Balance= @balance, 
                 PlotStatus= 'Allocated' WHERE PlotNo = @plotNo";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@newTotal", TotalPaid);
        cmd.Parameters.AddWithValue("@balance", Balance);
        cmd.Parameters.AddWithValue("@plotNo", PlotNo);
        cmd.ExecuteNonQuery();
        cmd.Dispose();

        //update2
        string sql2a = @"UPDATE  PlotAllocations SET AmountPaid= @newTotal WHERE PlotNo =@plotNo";
        SqlCommand cmd2a = new SqlCommand(sql2a, appconSQL2);
        cmd2a.Parameters.AddWithValue("@newTotal", TotalPaid);
        cmd2a.Parameters.AddWithValue("@plotNo", PlotNo);
        cmd2a.ExecuteNonQuery();
        cmd2a.Dispose();

        //delete from plot payments
        string sql2 = "Delete from PlotPayments  where [ReceiptNo] = @receiptNo";
        SqlCommand cmd2 = new SqlCommand(sql2, appconSQL2);
        cmd2.Parameters.AddWithValue("@receiptNo", lblReceiptNumber.Text.Trim());
        cmd2.ExecuteNonQuery();
        cmd2.Dispose();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "myModalScript", "$('#DeleteModal').modal('show');", true);
    }


    public void sendEmail()
    {
        string fromMail = "innobuildprivatelimited@gmail.com";
        string fromPassword = "uqhnbpbbfoinelky";
        MailMessage message = new MailMessage();
        message.From = new MailAddress(fromMail);

        message.Subject = "PLOT PAYMENT UPDATED RECEIPT";
        string mailAddress = lblEmail.Text.Trim();

        message.To.Add(new MailAddress(mailAddress));

        string client = lblClient.Text.Trim();

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
            <h1>PLOT PAYMENT UPDATED RECEIPT</h1>
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
                <p>This payment was already made. This is just an updated Receipt. Please keep it for your records.</p>
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