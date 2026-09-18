<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleAgreement.aspx.cs" EnableEventValidation="false" Inherits="saleAgreement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SALE AGREEMENT</title>
   

    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }

        .container {
            width: 80%;
            margin: auto;
        }

        header {
            text-align: center;
            margin-bottom: 20px;
        }

        h1 {
            font-size: 24pt;
            font-family: Book Antiqua,Palatino,Palatino Linotype,Palatino LT STD,Georgia,serif;
        }

        h2 {
            font-size: 20pt;
            margin-top: 100px;
            font-family: Book Antiqua,Palatino,Palatino Linotype,Palatino LT STD,Georgia,serif;
        }

        h3 {
            font-size: 20px;
            margin-top: 25px;
            font-family: Book Antiqua,Palatino,Palatino Linotype,Palatino LT STD,Georgia,serif;
        }

        p {
            font-size: 11pt;
            line-height: 1.5;
            margin-bottom: 10px;
            font-family: Book Antiqua,Palatino,Palatino Linotype,Palatino LT STD,Georgia,serif;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        th, td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
        }

        .footer {
            margin-top: 20px;
            text-align: center;
        }

        .signature {
            margin-top: 40px;
        }

            .signature p {
                margin-top: 20px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
       

        <div class="container">
            <header>
                <asp:Label runat="server" ID="lblPlotNo" Visible="false"></asp:Label>
                
                <h1 style="margin-top: 200px;">DATED THIS</h1>
                <p>
                    <asp:Label runat="server" ID="lblDate" Font-Size="20pt"></asp:Label></p>

                <h1 style="margin-top: 100px;">SALE AGREEMENT</h1>
                <h2>BETWEEN</h2>
                <p><strong><u>
                    <asp:Label runat="server" ID="lblCustomer" Font-Size="20pt"></asp:Label></u></strong></p>
                <p><strong>
                    <asp:Label runat="server" ID="lblPlotNumber" Font-Size="20pt"></asp:Label>
                </strong></p>
                <br />
                <p style="font-size: 20pt"><strong>AND</strong></p>
                <br />
                <p style="font-size: 20pt"><strong><u>BILLY JONATHAN CHIWOTHA</u><br />
                </strong></p>
                <p style="font-size: 16pt"><strong>(Represented by Innobuild Private Limited company)</strong></p>
            </header>

            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />

            <h3>SALE AGREEMENT</h3>

            <p>THIS AGREEMENT dated this
                <asp:Label runat="server" ID="lblDate2"> </asp:Label><asp:Label runat="server" ID="lblDateWords" Visible="false"> </asp:Label> <asp:Label runat="server" ID="lblDateInWords"> </asp:Label>
                 is made between Billy Jonathan Chiwotha (represented by Innobuild of, P.O. Box 1967, Lilongwe in the Republic of Malawi (hereinafter called “the Vendor”) of the one part and <asp:Label runat="server" ID="lblCustomer2"></asp:Label> of <asp:Label runat="server" ID="lblCustomerDistrict"></asp:Label> (hereinafter called “the purchaser”) of the other part.</p>

            <p><strong>WHEREAS</strong> the vendor is the owner of the property (An open plot number
                <asp:Label runat="server" ID="lblPlotNumber2"></asp:Label>
                of
                <asp:Label runat="server" ID="lblPlotSize"></asp:Label>
                located at 
                <asp:Label runat="server" ID="lblPlotLocation"></asp:Label> <asp:Label runat="server" ID="lblVillage"></asp:Label> <asp:Label runat="server" ID="lblTA"></asp:Label> 
                in 
                <asp:Label runat="server" ID="lblDistrict"></asp:Label>
                District in the Republic of Malawi and the Vendor has agreed to sell the said property to the Purchaser (s) at a consideration of
                <asp:Label runat="server" ID="lblAgreedPrice" Visible="true"></asp:Label><asp:Label runat="server" ID="lblAgreedPriceMk"></asp:Label>
                
               (<asp:Label runat="server" ID="lblAgreedPriceinWords"></asp:Label>)</p>
           

            <p>
                <strong>AND WHEREAS </strong>it has been agreed that the Vendor shall sell and the Purchaser shall purchase the property by paying the full payment.
         
           <br />
                <br />
                <strong>1.	PURCHASE PRICE </strong>
                <br />
                <br />

                &nbsp;&nbsp;&nbsp;<strong> (a)</strong>	The purchase price for the property has been amounted to
                <asp:Label runat="server" ID="lblPlotPrice2"></asp:Label>
                which, has been
                <br />
                &nbsp;&nbsp;&nbsp;fully paid by the purchaser   and is the legal owner of the plot with reference number
                <asp:Label runat="server" ID="lblPlotNumber3"></asp:Label>
                <br />
                &nbsp;&nbsp;&nbsp; 
            <br />
                <br />
                <strong>2.	OCCUPATION AND RISK AND PROFIT</strong>
                <br />
                <br />
                &nbsp;&nbsp;&nbsp;<strong> (a)</strong>	The Purchaser hereby acknowledges that:
                <br />
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(i)	They are fully acquainted with the property, building and improvements, boundaries
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; and beacons, the terms and conditions of the sale and any encumbrances or servitudes  to
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; which the property is subject.<br />

                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(ii)	The property is sold as it stands and not with or subject to any condition or
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; warranty expressed or implied, as to extent, condition, nature or fitness.<br />
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>(b)</strong> 	Property and risk shall pass upon payment of the Purchase Price herein.
                <br />
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>(c)</strong>	The Vendor hereby acknowledge that:<br />
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(i)	He has not offered and shall not offer for sale the said property to any other person for as
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  long as the Purchaser has performed his obligations under this agreement.<br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(ii)	He shall not remove all fittings fitted to the land as stands part of the land 
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(iii)	He shall cease to benefit in any means from the purchased property upon full payment
                of the purchase price.
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(iv)	He shall coordinate and help the purchaser during Title Deed processing
                <br />
            </p>
            <br />
            <br />
            <br />
            <br />
            <br />
            <p><strong>IN WITNESS WHEREOF</strong> the said parties hereto have hereunto set their respective hands the day and year first before written.</p>

            <div class="signature">
                <p>SIGNED by the said &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)
                    <br />
                    <strong>Billy Jonathan Chiwotha </strong></p>

                <p>As vendor&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)  ______________________________________________</p>
                <p>In the presence of:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>FUMBANI CHIRAMBO</strong></p>
                <p>Sale Agreement Number:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label ID="lblSaleAgreementId" runat="server" Visible="true"></asp:Label> </p>
                <br />
                <p><strong>WITNESS:</strong></p>
                <p><strong>ADDRESS: </strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; P.O.BOX 1967-LILONGWE</p>
                <p><strong>OCCUPATION:</strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;OPERATIONS MANAGER</p>
            </div>

            <div class="signature">
                <p>
                    <strong>SIGNED</strong> by the said &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)
            <br />
                    <strong>Purchaser</strong> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)  ______________________________________________<br />
                    <strong><asp:Label runat="server" ID="lblPurchaser"></asp:Label> </strong><br />
                    In the presence of:
                </p>
                <p><strong>WITNESS:</strong>  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)  ______________________________________________<br />
                </p>
                <p><strong>ADDRESS:</strong> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)  ______________________________________________<br />
                </p>
                <p><strong>OCCUPATION:</strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)  ______________________________________________<br />
                </p>
                <p><strong>CELL PHONE NUMBER:</strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)  ______________________________________________<br />
                </p>
            </div>
        </div>
       
            <p style="width: 746px">
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click"/>
&nbsp;
        <asp:Button ID="Button2" runat="server" BackColor="#009933" style="font-weight: 700" Text="Print" OnClick="btnPrint_Click"/>
        &nbsp;
               
            </p>
       
     
    </form>
</body>
</html>
