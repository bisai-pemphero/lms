<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleAgreement.aspx.cs" EnableEventValidation="false" Inherits="saleAgreement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SALE AGREEMENT</title>

    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Book Antiqua', Palatino, 'Palatino Linotype', 'Palatino LT STD', Georgia, serif;
            margin: 0;
            padding: 20px;
            background: #f2f2f2;
        }

        .container {
            max-width: 1100px;
            margin: 0 auto;
            background: #fff;
            padding: 40px 60px;
            box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);
        }

        .page {
            page-break-after: always;
            padding-bottom: 30px;
            margin-bottom: 20px;
            border-bottom: 2px dashed #ccc;
        }

        .page:last-child {
            page-break-after: avoid;
            border-bottom: none;
            margin-bottom: 0;
            padding-bottom: 0;
        }

        h1 {
            font-size: 28pt;
            font-family: 'Book Antiqua', Palatino, 'Palatino Linotype', 'Palatino LT STD', Georgia, serif;
            letter-spacing: 2px;
        }

        h2 {
            font-size: 22pt;
            font-family: 'Book Antiqua', Palatino, 'Palatino Linotype', 'Palatino LT STD', Georgia, serif;
            margin: 20px 0 10px;
        }

        h3 {
            font-size: 20pt;
            font-family: 'Book Antiqua', Palatino, 'Palatino Linotype', 'Palatino LT STD', Georgia, serif;
            text-align: center;
            margin: 10px 0 25px;
            text-decoration: underline;
        }

        p {
            font-size: 12pt;
            line-height: 1.6;
            margin-bottom: 8px;
            font-family: 'Book Antiqua', Palatino, 'Palatino Linotype', 'Palatino LT STD', Georgia, serif;
            text-align: justify;
        }

        .center {
            text-align: center;
        }

        .mt-10 {
            margin-top: 10px;
        }
        .mt-20 {
            margin-top: 20px;
        }
        .mt-40 {
            margin-top: 40px;
        }
        .mt-60 {
            margin-top: 60px;
        }
        .mt-80 {
            margin-top: 80px;
        }
        .mt-100 {
            margin-top: 100px;
        }
        .mt-140 {
            margin-top: 140px;
        }
        .mt-160 {
            margin-top: 160px;
        }
        .mt-180 {
            margin-top: 180px;
        }

        .mb-5 {
            margin-bottom: 5px;
        }
        .mb-10 {
            margin-bottom: 10px;
        }

        .underline {
            text-decoration: underline;
        }

        .bold {
            font-weight: bold;
        }

        .sig-block {
            margin-top: 35px;
        }

        .sig-block p {
            margin-bottom: 4px;
        }

        .sig-line {
            display: inline-block;
            width: 250px;
            border-bottom: 1px solid #000;
            margin-left: 8px;
            height: 20px;
        }

        .sig-label {
            display: inline-block;
            width: 160px;
        }

        .page-footer {
            text-align: center;
            margin-top: 60px;
            font-size: 10pt;
            color: #666;
        }

        .print-actions {
            width: 100%;
            text-align: center;
            margin-top: 30px;
            padding: 15px 0;
            border-top: 2px solid #eee;
        }

        .print-actions input {
            padding: 10px 30px;
            font-size: 14px;
            font-weight: bold;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            margin: 0 8px;
        }

        .btn-back {
            background: #6c757d;
            color: #fff;
        }

        .btn-print {
            background: #28a745;
            color: #fff;
        }

        /* Title page specific centering */
        .title-page {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            padding: 40px 0;
        }

        .title-page h1 {
            margin: 0;
            padding: 0;
        }

        .title-page p {
            margin: 0;
            padding: 0;
            text-align: center;
        }

        @media print {
            body {
                background: #fff;
                padding: 0;
                margin: 0;
            }

            .container {
                box-shadow: none;
                padding: 30px 45px;
                max-width: 100%;
            }

            .page {
                page-break-after: always;
                border-bottom: none;
                padding-bottom: 10px;
                margin-bottom: 0;
                margin-top:40px;
            }

            .page:last-child {
                page-break-after: avoid;
            }

            .print-actions {
                display: none !important;
            }

            .no-print {
                display: none !important;
            }

            p {
                font-size: 11.5pt;
            }

            h1 {
                font-size: 26pt;
            }
            h2 {
                font-size: 20pt;
            }
            h3 {
                font-size: 18pt;
            }

            .title-page {
                min-height: 90vh;
            }
        }

        @media (max-width: 768px) {
            .container {
                padding: 20px;
            }
            h1 {
                font-size: 22pt;
            }
            h2 {
                font-size: 18pt;
            }
            .sig-line {
                width: 120px;
            }
            .sig-label {
                width: 120px;
            }
            .title-page {
                min-height: auto;
               
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="container">

            <!-- ==================== PAGE 1 ==================== -->
            <div class="page">

                <div class="center title-page">
                    <asp:Label runat="server" ID="lblPlotNo" Visible="false"></asp:Label>

                    <h1 style="margin-top: 40px;">DATED THIS</h1>
                    <p style="font-size: 22pt; font-weight: bold; margin-top: 5px;">
                        <asp:Label runat="server" ID="lblDate"></asp:Label>
                    </p>

                    <h1 style="margin-top: 70px;">SALE AGREEMENT</h1>
                    <h2 style="margin-top: 30px;">BETWEEN</h2>

                    <p style="font-size: 22pt; font-weight: bold; margin-top: 20px;">
                        <u><asp:Label runat="server" ID="lblCustomer"></asp:Label></u>
                    </p>
                    <p style="font-size: 22pt; font-weight: bold; margin-top: -2px;">
                        <asp:Label runat="server" ID="lblPlotNumber"></asp:Label>
                    </p>

                    <p style="font-size: 22pt; font-weight: bold; margin-top: 25px;">AND</p>

                    <p style="font-size: 22pt; font-weight: bold; margin-top: 20px;">
                        <u>BILLY JONATHAN CHIWOTHA</u>
                    </p>
                    <p style="font-size: 16pt; font-weight: bold; margin-top: 2px;">
                        (Represented by Innobuild Private Limited company)
                    </p>
                </div>
            </div>
            <!-- /PAGE 1 -->


            <!-- ==================== PAGE 2 ==================== -->
            <div class="page">

                <h3>SALE AGREEMENT</h3>

                <p>
                    <span class="bold">THIS AGREEMENT</span> dated this
                    <asp:Label runat="server" ID="lblDate2"></asp:Label>
                    <asp:Label runat="server" ID="lblDateWords" Visible="false"></asp:Label>
                    <asp:Label runat="server" ID="lblDateInWords"></asp:Label>
                    is made between Billy Jonathan Chiwotha (represented by Innobuild of, P.O. Box 1967, Lilongwe in the Republic of Malawi (hereinafter called "the Vendor") of the one part and
                    <asp:Label runat="server" ID="lblCustomer2"></asp:Label>
                    of
                    <asp:Label runat="server" ID="lblCustomerDistrict"></asp:Label>
                    (hereinafter called "the purchaser") of the other part.
                </p>

                <p>
                    <span class="bold">WHEREAS</span> the vendor is the owner of the property (An open plot number
                    <asp:Label runat="server" ID="lblPlotNumber2"></asp:Label>
                    of
                    <asp:Label runat="server" ID="lblPlotSize"></asp:Label>
                    located at
                    <asp:Label runat="server" ID="lblPlotLocation"></asp:Label>
                    <asp:Label runat="server" ID="lblVillage"></asp:Label>
                    <asp:Label runat="server" ID="lblTA"></asp:Label>
                    in
                    <asp:Label runat="server" ID="lblDistrict"></asp:Label>
                    District in the Republic of Malawi and the Vendor has agreed to sell the said property to the Purchaser(s) at a consideration of
                    <asp:Label runat="server" ID="lblAgreedPrice" Visible="true"></asp:Label>
                    <asp:Label runat="server" ID="lblAgreedPriceMk"></asp:Label>
                    (<asp:Label runat="server" ID="lblAgreedPriceinWords"></asp:Label>).
                </p>

                <p>
                    <span class="bold">AND WHEREAS</span> it has been agreed that the Vendor shall sell and the Purchaser shall purchase the property by paying the full payment.
                </p>

                <p style="margin-top: 15px;">
                    <span class="bold">1. PURCHASE PRICE</span>
                </p>

                <p style="margin-left: 20px;">
                    <span class="bold">(a)</span> The purchase price for the property has been amounted to
                    <asp:Label runat="server" ID="lblPlotPrice2"></asp:Label>
                    which has been fully paid by the purchaser and is the legal owner of the plot with reference number
                    <asp:Label runat="server" ID="lblPlotNumber3"></asp:Label>.
                </p>

                <p style="margin-top: 15px;">
                    <span class="bold">2. OCCUPATION AND RISK AND PROFIT</span>
                </p>

                <p style="margin-left: 20px;">
                    <span class="bold">(a)</span> The Purchaser hereby acknowledges that:
                </p>

                <p style="margin-left: 50px;">
                    (i) They are fully acquainted with the property, building and improvements, boundaries and beacons, the terms and conditions of the sale and any encumbrances or servitudes to which the property is subject.
                </p>

                <p style="margin-left: 50px;">
                    (ii) The property is sold as it stands and not with or subject to any condition or warranty expressed or implied, as to extent, condition, nature or fitness.
                </p>

                <p style="margin-left: 20px;">
                    <span class="bold">(b)</span> Property and risk shall pass upon payment of the Purchase Price herein.
                </p>

                <p style="margin-left: 20px;">
                    <span class="bold">(c)</span> The Vendor hereby acknowledge that:
                </p>

                <p style="margin-left: 50px;">
                    (i) He has not offered and shall not offer for sale the said property to any other person for as long as the Purchaser has performed his obligations under this agreement.
                </p>

                <p style="margin-left: 50px;">
                    (ii) He shall not remove all fittings fitted to the land as stands part of the land.
                </p>

                <p style="margin-left: 50px;">
                    (iii) He shall cease to benefit in any means from the purchased property upon full payment of the purchase price.
                </p>

                <p style="margin-left: 50px;">
                    (iv) He shall coordinate and help the purchaser during Title Deed processing.
                </p>

              

            </div>
            <!-- /PAGE 2 -->


            <!-- ==================== PAGE 3 ==================== -->
            <div class="page">

                <p>
                    <span class="bold">IN WITNESS WHEREOF</span> the said parties hereto have hereunto set their respective hands the day and year first before written.
                </p>

                <!-- VENDOR SIGNATURE -->
                <div class="sig-block">
                    <p>
                        <span class="bold">SIGNED</span> by the said
                        <span style="display:inline-block; width:180px; border-bottom:1px solid #000; margin-left:8px;">&nbsp;</span>
                    </p>
                    <p><span class="bold">Billy Jonathan Chiwotha</span></p>
                    <p>
                        As vendor
                        <span style="display:inline-block; width:280px; border-bottom:1px solid #000; margin-left:15px;">&nbsp;</span>
                    </p>
                    <p>
                        In the presence of: <span class="bold">FUMBANI CHIRAMBO</span>
                    </p>
                    <p>
                        <asp:Label ID="lblSaleAgreementId" runat="server" Visible="false"></asp:Label>
                    </p>
                    <p><span class="bold">WITNESS:</span></p>
                    <p><span class="bold">ADDRESS:</span> P.O. BOX 1967 - LILONGWE</p>
                    <p><span class="bold">OCCUPATION:</span> OPERATIONS MANAGER</p>
                </div>

                <!-- PURCHASER SIGNATURE -->
                <div class="sig-block" style="margin-top: 40px;">
                    <p>
                        <span class="bold">SIGNED</span> by the said
                        <span style="display:inline-block; width:180px; border-bottom:1px solid #000; margin-left:8px;">&nbsp;</span>
                    </p>
                    <p>
                        <span class="bold">Purchaser</span>
                        <span style="display:inline-block; width:280px; border-bottom:1px solid #000; margin-left:15px;">&nbsp;</span>
                    </p>
                    <p><span class="bold"><asp:Label runat="server" ID="lblPurchaser"></asp:Label></span></p>
                    <p>In the presence of:</p>
                    <p>
                        <span class="bold">WITNESS:</span>
                        <span style="display:inline-block; width:250px; border-bottom:1px solid #000; margin-left:10px;">&nbsp;</span>
                    </p>
                    <p>
                        <span class="bold">ADDRESS:</span>
                        <span style="display:inline-block; width:250px; border-bottom:1px solid #000; margin-left:10px;">&nbsp;</span>
                    </p>
                    <p>
                        <span class="bold">OCCUPATION:</span>
                        <span style="display:inline-block; width:235px; border-bottom:1px solid #000; margin-left:10px;">&nbsp;</span>
                    </p>
                    <p>
                        <span class="bold">CELL PHONE NUMBER:</span>
                        <span style="display:inline-block; width:190px; border-bottom:1px solid #000; margin-left:10px;">&nbsp;</span>
                    </p>
                </div>

              
            </div>
            <!-- /PAGE 3 -->

            <!-- ==================== PRINT BUTTONS ==================== -->
            <div class="print-actions no-print">
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="btn-back" />
                <asp:Button ID="Button2" runat="server" BackColor="#009933" Style="font-weight: 700;" Text="Print" OnClick="btnPrint_Click" CssClass="btn-print" />
            </div>
                
          
        </div>

    </form>
</body>
</html>