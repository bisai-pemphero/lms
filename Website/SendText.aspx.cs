using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using WhatsAppApi;
using WhatsAppApi.Parser;

public partial class UserLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Page load logic if needed
    }



    protected void btnSend_Click(object sender, EventArgs e)
    {
        sendSMS();

    }

    public string sendSMS()
    {
        String result;
        string apiKey = "Njg3NTZjNTQzNDM3NTUzODRkNmM3Mzc0NmM2MTM0MzQ=";
        string numbers = "265882196556"; // in a comma seperated list
        string message = "This is your message";
        string sender = "Jims Autos";

        String url = "https://api.txtlocal.com/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
        //refer to parameters to complete correct url string

        StreamWriter myWriter = null;
        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

        objRequest.Method = "POST";
        objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
        objRequest.ContentType = "application/x-www-form-urlencoded";
        try
        {
            myWriter = new StreamWriter(objRequest.GetRequestStream());
            myWriter.Write(url);
        }
        catch (Exception e)
        {
            return e.Message;
        }
        finally
        {
            myWriter.Close();
        }

        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        {
            result = sr.ReadToEnd();
            // Close and clean up the StreamReader
            sr.Close();
        }
        return result;
    }

}
