using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;

public partial class saleAgreement : System.Web.UI.Page
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

        LoadOfferDetails(id);
    }

  
    public void LoadOfferDetails(string id)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["lms"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(@"SELECT 
    C.Fullname, 
    C.Address, 
    S.PhysicalLocation,

    STUFF((
        SELECT ', ' + P2.PlotNo
        FROM SaleAgreementPlots P2
        WHERE P2.SaleAgreementId = SA.Id
        FOR XML PATH(''), TYPE
    ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS PlotNumbers,

    S.District, 
    S.Region,

    SUM(P.AmountPaid) AS TotalAmountPaid,

    SA.OfferDate,

    SUM(TRY_CAST(P.PlotSize AS DECIMAL(18,2))) AS TotalPlotSize,

    S.TA,
    S.Village

FROM Clients AS C

JOIN SaleAgreements AS SA 
    ON C.ClientNo = SA.ClientNo

JOIN SaleAgreementPlots AS SAP 
    ON SA.Id = SAP.SaleAgreementId

JOIN Sites AS S 
    ON S.SiteCode = SAP.SiteNo

JOIN Plots AS P 
    ON SAP.PlotNo = P.PlotNo

WHERE SAP.SaleAgreementId = @Id

GROUP BY 
    C.Fullname, 
    C.Address, 
    S.PhysicalLocation,
    S.District, 
    S.Region, 
    SA.OfferDate,
    S.TA, 
    S.Village,
    SA.Id;",  connection);
            command.Parameters.AddWithValue("@Id", id);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lblCustomer.Text = reader.GetString(0).ToUpper();
                lblCustomer2.Text = reader.GetString(0).ToUpper();
                lblPurchaser.Text = reader.GetString(0).ToUpper();
                lblCustomerDistrict.Text = reader.GetString(1);
                lblPlotLocation.Text = reader.GetString(2).ToUpper();
              //  lblSiteName2.Text = reader.GetString(2).ToUpper();
                lblDistrict.Text = reader.GetString(4).ToUpper();
               // lblSiteName_and_district_and_region.Text = reader.GetString(2).ToUpper() +" in " + reader.GetString(3) +", " + reader.GetString(4);
                lblAgreedPrice.Text = "K" + reader.GetDouble(6).ToString("N2");
                lblPlotPrice2.Text = "K" + reader.GetDouble(6).ToString("N2");
                lblDate.Text = reader.GetString(7);
                lblDate2.Text = reader.GetString(7);
                lblPlotSize.Text = reader.GetDecimal(8).ToString() + " Square Meters";
                lblTA.Text = "T/A " + reader.GetString(9).ToUpper(); 
                lblVillage.Text = reader.GetString(10).ToUpper() + " Village";
                lblPlotNumber.Text = reader.GetString(2).ToUpper() + ", " + reader.GetString(3).ToUpper() ;
                lblPlotNumber2.Text = "INNOBUILD/" + reader.GetString(2).ToUpper() + "/" + reader.GetString(3).ToUpper();
                lblPlotNumber3.Text = "INNOBUILD/" + reader.GetString(2).ToUpper() + "/" + reader.GetString(3).ToUpper();

                string input = reader.GetDouble(6).ToString();

                lblSaleAgreementId.Text = "";
                if (long.TryParse(input, out number))
                {
                    string words = ConvertNumberToWords(number);
                    lblAgreedPriceinWords.Text = words + " Kwacha";
                 //   lblPriceinWords2.Text = words + " Kwacha";
                }
                else
                {
                    lblAgreedPriceinWords.Text = "Amount in Words";
                  //  lblPriceinWords2.Text = "Amount in Words";
                }
            }

            reader.Close();
        }
    }

    private long number;
    protected string ConvertNumberToWords(long number)
    {

        if (number == 0)
        {
            return "Zero";
        }

        if (number < 0)
        {
            return "Negative " + ConvertNumberToWords(Math.Abs(number));
        }

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
            {
                words += "and ";
            }

            string[] unitsMap = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            string[] tensMap = { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            string[] teensMap = { "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

            if (number < 10)
            {
                words += unitsMap[number];
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

        return words;

    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterClientScriptBlock(this.GetType(), "key", "window.print()", true);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard.aspx");
    }
}
