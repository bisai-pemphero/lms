<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OfferLetter.aspx.cs" EnableEventValidation="false" Inherits="OfferLetter" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Offer Letter</title>
    <style>
        /* A4 page setup */
@page {
    size: A4;
    margin: 10mm;
}

html, body {
    width: 210mm;
    min-height: 297mm;
    margin: 0;
    padding: 0;
}

body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    background-color: #f8f9fa;
    color: #333;
    line-height: 1.4;
    -webkit-print-color-adjust: exact;
    print-color-adjust: exact;
}

.a4-container {
    width: 190mm;
    min-height: 277mm;
    margin: 10px auto;
    padding: 10mm;
    box-sizing: border-box;
    background: white;
    box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);
}

/* Header styling */
.letterhead {
    text-align: center;
    padding-bottom: 10px;
    border-bottom: 2px solid #2c5aa0;
}

    .letterhead img {
        max-width: 100%;
        height: auto;
        margin-bottom: 5px;
    }

.document-title {
    font-size: 20px;
    font-weight: bold;
    color: #2c5aa0;
    text-transform: uppercase;
    letter-spacing: 1.5px;
}

/* Modern table styling */
.modern-table {
    width: 100%;
    border-collapse: collapse;
    margin: 5px 0;
    font-size: 13px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.05);
}

    .modern-table th {
        background-color: #2c5aa0;
        color: white;
        text-align: left;
        padding: 8px 10px;
        font-weight: bold;
        border: 1px solid #1e3f6d;
    }

    .modern-table td {
        padding: 8px 10px;
        border: 1px solid #ddd;
        vertical-align: top;
    }

    .modern-table tr:nth-child(even) {
        background-color: #f8f9fa;
    }

    .modern-table tr:hover {
        background-color: #f1f7ff;
    }

/* Content sections */
.content-section {
    margin: 12px 0;
    text-align: justify;
}

.conditions {
    margin: 15px 0;
    padding: 12px;
    background-color: #f8f9fa;
    border-left: 4px solid #2c5aa0;
}

    .conditions ol {
        padding-left: 18px;
        margin: 0;
    }

    .conditions li {
        margin-bottom: 8px;
    }

/* Signature area */
.signature-area {
    margin-top: 30px;
}

.signature-line {
    border-top: 1px solid #333;
    width: 250px;
    margin: 40px 0 5px 0;
}

.signature-label {
    font-weight: bold;
    font-size: 13px;
}

/* Acceptance section */
.acceptance-section {
    margin-top: 30px;
    padding: 12px;
    background-color: #f8f9fa;
    border-radius: 5px;
}

/* Buttons */
.action-buttons {
    text-align: center;
    margin-top: 20px;
    padding: 15px;
}

.action-button {
    background-color: #2c5aa0;
    color: white;
    border: none;
    padding: 10px 20px;
    font-size: 14px;
    border-radius: 4px;
    cursor: pointer;
    margin: 0 10px;
    transition: background-color 0.3s;
}

    .action-button:hover {
        background-color: #1e3f6d;
    }

.back-button {
    background-color: #6c757d;
}

    .back-button:hover {
        background-color: #5a6268;
    }

.info-label {
    font-weight: bold;
    width: 30%;
}

.text-center {
    text-align: center;
}

.mb-20 {
    margin-bottom: 20px;
}

.company-header {
    text-align: center;
    padding-bottom: 3mm;
}

    .company-header img {
        max-width: 100%;
        height: auto;
    }

/* Page break indicator for screen view */
.page-break {
    display: block;
    height: 0;
    overflow: hidden;
    margin: 20px 0;
    text-align: center;
    position: relative;
}

    .page-break::before {
        content: "--- Page Break ---";
        display: block;
        color: #2c5aa0;
        font-weight: bold;
        padding: 10px;
        border-top: 2px dashed #2c5aa0;
        border-bottom: 2px dashed #2c5aa0;
        background-color: #f1f7ff;
    }

/* Print styles */
@media print {

    html, body {
        width: 210mm;
        height: auto;
        background: white;
    }

    body {
        margin: 0;
        padding: 0;
    }

    .a4-container {
        width: 190mm;
        min-height: auto;
        margin: 0 auto;
        padding: 0;
        box-shadow: none;
        background: white;
    }

    .action-buttons {
        display: none !important;
    }

    .modern-table {
        box-shadow: none;
    }

    .page-break {
        page-break-before: always;
        break-before: page;
    }

    .page-break::before {
        display: none;
    }

    table,
    tr,
    td,
    th,
    .conditions,
    .signature-area,
    .acceptance-section {
        page-break-inside: avoid;
        break-inside: avoid;
    }

    p,
    li {
        orphans: 3;
        widows: 3;
    }
}
    </style>
</head>
<body>
    <div class="a4-container">
        <!-- Letterhead -->
        <div class="company-header">
            <img src="../assets/img/New innobuild letter head.png" alt="Innobuild Logo" />
        </div>

        <!-- Client Information Table -->
        <table class="modern-table">

            <tr>
                <td class="info-label">Dated</td>
                <td>:
                    <asp:Label ID="lblDated" runat="server">October 26, 2023</asp:Label></td>
            </tr>
            <tr>
                <td class="info-label">Reference</td>
                <td>:
                    <asp:Label runat="server" ID="lblRefNo" Style="font-weight: 700">INNO/OL/2023/1085</asp:Label></td>
            </tr>
            <tr>
                <td class="info-label">Client No</td>
                <td>:
                    <asp:Label runat="server" ID="lblClientNo">CL2023356</asp:Label></td>
            </tr>
            <tr>
                <td class="info-label">Fullname</td>
                <td>:
                    <asp:Label runat="server" ID="lblClientName">James Wilson</asp:Label></td>
            </tr>
            <tr>
                <td class="info-label">Address</td>
                <td>:
                    <asp:Label runat="server" ID="lblAdress">123 Main Street, Lilongwe, Malawi</asp:Label></td>
            </tr>
            <tr>
                <td class="info-label">Email</td>
                <td>:
                    <asp:Label runat="server" ID="lblEmail">123 Main Street, Lilongwe, Malawi</asp:Label></td>
            </tr>
            <tr>
                <td class="info-label">Contact Number</td>
                <td>:
                    <asp:Label runat="server" ID="lblPhone">+265 97 123 4567</asp:Label>

                </td>
            </tr>
        </table>

        <!-- Content Section -->
        <div class="content-section">
            <p>Dear Sir/Madam,</p>
            <p><strong>OFFER FOR THE SALE OF PLOT NUMBER:
                <asp:Label runat="server" ID="lblSite2">PL457</asp:Label>/<asp:Label runat="server" ID="lblPlotNo1">PL457</asp:Label></strong></p>
            <p>Reference is made to the above subject matter.</p>
            <p>Following your expression of interest to purchase a plot as detailed below, please be informed that we are offering you this plot with conditions outlined below:</p>
        </div>

        <!-- Plot Details Table -->
        <table class="modern-table">
            <tr>
                <th colspan="2">PLOT DETAILS</th>
            </tr>
            <tr>
                <td class="info-label">Location (Site Name)</td>
                <td>:
                    <asp:Label runat="server" ID="lblSite">Chiseka</asp:Label></td>
            </tr>
            <tr>
                <td class="info-label">Plot No</td>
                <td>:
                    <asp:Label runat="server" ID="lblPlotNo2">PL457</asp:Label></td>
            </tr>
            <tr>
                <td class="info-label">Plot Size</td>
                <td>:
                    <asp:Label runat="server" ID="lblPlotSize"> </asp:Label></td>
            </tr>
            <tr>
                <td class="info-label">Selling Price</td>
                <td>:
                    <asp:Label runat="server" ID="lblAgreedPrice">MWK 250,000</asp:Label></td>
            </tr>
            <tr>
                <td class="info-label">Sale Category</td>
                <td>:
                    <asp:Label runat="server" ID="lblCategory">Residential</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="info-label">Offer Date</td>
                <td>:
                    <asp:Label runat="server" ID="lblOfferDate">October 26, 2023</asp:Label></td>
            </tr>
            <tr>
                <td class="info-label">Commitment Period </td>
                <td>:
                    <asp:Label runat="server" ID="lblPeriod">24</asp:Label></td>
            </tr>
        </table>

        <!-- Page Break - Conditions will start on a new page -->
        <div class="page-break"></div>

        <!-- Conditions Section -->
        <div class="conditions">
            <p><strong>Conditions:</strong></p>
            <ol>
                <li>The Sale is strictly based on the details provided above (As IS Basis).</li>
                <li>The Offer Should be paid either in Full or in not more than the agreed Contract duration with the Initial payment not being less than 35% of the Offer Price. And the agreed contract duration should not exceed 24 months.</li>
                <li>The Buyer shall have right to start developing once full payment has been done and Sales Agreement has been Issued to him/her.</li>
                <li>If the Offer Price has not been settled in Full by the given period, the Seller has rights to reposes the plot and offer it to another interested buyer and the Accumulated amount Except the Legal Charges the first Buyer had paid by the repossession date shall be paid back to him/her less 20% as logistics charges. The refund shall be effected upon full payment of the Offer Price by the New Buyer.</li>
                <li>This Offer shall be considered Accepted by the Buyer through the payment of at least the initial payment of the offer price within the period of 24 hours (1 Day). However if the 24 hours for cash payment exceeds, then the reservation shall be cancelled and the plot shall be back on market.</li>
                <li>The initial payment shall be considered first installment for the plot purchase. All payments should always be deposited to the accounts numbers below or paid in Cash and get an electronic Receipt at Innobuild Office.</li>
                <li>Any delay in monthly payments shall attract the following penalties 5 days after due date:
                    <ul>
                        <li>Monthly payments below K100,000 shall incur a penalty of K25,000.</li>
                        <li>Monthly payments between K100,000 and K200,000 shall incur a penalty of K40,000.
                        </li>
                        <li>Monthly payments between K200,000 and K500,000 shall incur a penalty of K60,000.</li>
                    </ul>
                </li>
            </ol>
            <p><strong>Note:</strong> The Buyer should bring the Electronically Printed Deposit Slip to Innobuild Offices for Receipt.</p>
        </div>

        <!-- Signature Area -->
        <div class="signature-area">
            <p>Yours Faithfully,</p>
            <div class="signature-line"></div>
            <div class="signature-label">BILLY JONATHAN CHIWOTHA</div>
            <div class="signature-label">CHIEF EXECUTIVE OFFICER</div>
        </div>

        <!-- Acceptance Section -->
        <div class="acceptance-section">
            <p><strong>OFFER ACCEPTANCE BY THE BUYER:</strong></p>
            <p>I ......................................................................... hereby referred as the interested buyer for the above plot(s), willingly and legally accept the above Offer and will abide by the conditions and terms set in the offer.</p>
            <p>Date: ...................................... Signature: ...................................................................</p>

            <!-- Page Break - Next of Kin will start on a new page -->
            <div class="page-break"></div>

            <p class="mb-20"><strong>NEXT OF KIN DETAILS:</strong></p>

            <!-- Next of Kin Table -->
            <table class="modern-table">
                <thead>
                    <tr>
                        <th>NAME</th>
                        <th>RELATIONSHIP</th>
                        <th>CONTACT</th>
                    </tr>
                </thead>
                <% LoadNextofKinDetails(); %>
            </table>
        </div>

        <!-- Action Buttons -->
        <div class="action-buttons">
            <button type="button" onclick="window.print()" class="action-button">Print Offer Letter</button>
            <a href="Dashboard.aspx" type="button" class="action-button back-button">Back to Home</a>

        </div>
    </div>

    <!-- Important labels -->
    <asp:Label runat="server" ID="lblPlotNo" Visible="false"></asp:Label>

    <script>
        // Simple JavaScript for demonstration purposes
        document.addEventListener('DOMContentLoaded', function () {
            // Add today's date to the offer letter
            const today = new Date();
            const options = { year: 'numeric', month: 'long', day: 'numeric' };
            document.getElementById('lblDated').textContent = today.toLocaleDateString('en-US', options);
            document.getElementById('lblOfferDate').textContent = today.toLocaleDateString('en-US', options);
        });
    </script>
</body>
</html>
