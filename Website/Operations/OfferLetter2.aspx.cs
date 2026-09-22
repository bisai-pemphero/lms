using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Web.UI;

public partial class OfferLetter : System.Web.UI.Page
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

        string id = Request.QueryString["id"];
        
        lblPlotNo.Text = id;

        LoadClientDetails(id);
        LoadPlotDetails(id);
        LoadOwnerDetails(id);
    }

    private long number;

    public void LoadClientDetails(string plotNo)
    {
        string sql = @"Select C.Fullname, C.Address, C.PhoneNo, C.PhoneNo2,
                     C.Email, C.ClientNo from Clients as C 
                     Join PlotAllocations as PA on PA.ClientNo = C.ClientNo
                     Where PA.PlotNo =  @plotNo ";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@plotno", plotNo);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblClientName.Text = dr.GetString(0).ToString();
            // lblClient2.Text = dr.GetString(0).ToString();
            lblAdress.Text = dr.GetString(1).ToString();
            lblPhone.Text = dr.GetString(2).ToString()  + " / " + dr.GetString(3).ToString();
            lblEmail.Text = dr.GetString(4).ToString();
            lblClientNo.Text = dr.GetString(5).ToString();
        }

        dr.Close();
        dr.Dispose();
    }

    public void LoadOwnerDetails(string plotNo)
    {
        string sql4 = @" Select S.[BankName], S.[AccountName],	S.[AccountNumber], S.[Branch], S.PreviousOwner from Sites as S
        Join Plots as P on S.SiteCode = P.SiteNo  Where P.PlotNo  =  @plotNo ";
        SqlCommand cmd4 = new SqlCommand(sql4, appconSQL2);
        cmd4.Parameters.AddWithValue("@plotno", plotNo);
        SqlDataReader dr4 = cmd4.ExecuteReader();
        while (dr4.Read())
        {
           lblBankName.Text = dr4.GetString(0);
            lblAccountName.Text = dr4.GetString(1);
            lblAccountNumber.Text = dr4.GetString(2);
            lblBranchName.Text = dr4.GetString(3);
            lblLandowner.Text = dr4.GetString(4);

        }

        dr4.Close();
        dr4.Dispose();
    }



    public void LoadPlotDetails(string plotNo)
    {
        string sql = @"SELECT 
        P.PlotNo, 
        P.PlotSize,
        S.PhysicalLocation, 
        P.AgreedPrice, 
        FORMAT(P.DateOffered, 'dd/MM/yyyy') AS Date,
        P.PriceCategory,
        P.OfferPeriod
        FROM Plots AS P
        JOIN Sites AS S ON S.SiteCode = P.SiteNo
        WHERE P.PlotNo = @plotNo";
        SqlCommand cmd = new SqlCommand(sql, appconSQL2);
        cmd.Parameters.AddWithValue("@plotno", plotNo);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lblPlotNo1.Text = dr.GetString(0).ToString();
            lblRefNo.Text = dr.GetString(0).ToString();
             lblPlotNo2.Text = dr.GetString(0).ToString();
            
            lblPlotSize.Text = dr.GetString(1).ToString() + " Square Meters";
            lblSite.Text = dr.GetString(2).ToString();
            lblSite2.Text = dr.GetString(2).ToString();
            lblAgreedPrice.Text = "MK" + dr.GetDouble(3).ToString("N2");
            //  lblPriceFigures2.Text = "MK" + dr.GetDouble(3).ToString("N2");
            lblDated.Text = dr.GetString(4).ToString();
            //lblOfferDate2.Text = dr.GetString(4).ToString();
            lblCategory.Text = dr.GetString(5).ToString();
            lblPeriod.Text = dr.GetString(6).ToString() + " Months";
            string input = dr.GetDouble(3).ToString();




            if (long.TryParse(input, out number))
            {
                string words = ConvertNumberToWords(number);
                //lblPriceinWords.Text =  words + " Kwacha";
                //lblPriceWords2.Text = words + " Kwacha" ;
            }
            else
            {
                //lblPriceinWords.Text = "Amount in Words";
                //lblPriceWords2.Text = "Amount in Words";
            }

        }

        dr.Close();
        dr.Dispose();
    }

    protected string ConvertNumberToWords(long number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Negative " + ConvertNumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000000) > 0)
        {
            words += ConvertNumberToWords(number / 1000000000) + " Billion ";
            number %= 1000000000;
        }

        if ((number / 1000000) > 0)
        {
            words += ConvertNumberToWords(number / 1000000) + " Million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += ConvertNumberToWords(number / 1000) + " Thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += ConvertNumberToWords(number / 100) + " Hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            string[] unitsMap =
            {
            "", "One", "Two", "Three", "Four",
            "Five", "Six", "Seven", "Eight", "Nine"
        };

            string[] tensMap =
            {
            "", "Ten", "Twenty", "Thirty", "Forty",
            "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
        };

            string[] teensMap =
            {
            "Eleven", "Twelve", "Thirteen", "Fourteen",
            "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
        };

            if (number < 10)
            {
                words += unitsMap[number];
            }
            else if (number == 10)
            {
                words += "Ten";
            }
            else if (number < 20)
            {
                words += teensMap[number - 11]; 
            }
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                {
                    words += "-" + unitsMap[number % 10];
                }
            }
        }

        return words.Trim(); 
    }


    public void LoadNextofKinDetails()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["lms"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(@" Select N.[Name], N.[Relationship], N.[Contact]
                from [Next_of_Kins]  as N Join Plots as P on P.OfferedTo = N.ClientNo
                where P.PlotNo = @plotNo",  connection);
            command.Parameters.AddWithValue("@plotNo", lblPlotNo.Text.Trim());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Response.Write("<tr>");
                Response.Write("<td>" + reader["Name"] + "</td>");
                Response.Write("<td>" + reader["Relationship"] + "</td>");
                Response.Write("<td>" + reader["Contact"] + "</td>");
                Response.Write("</tr>");
            }

            reader.Close();
        }
    }


   
}
