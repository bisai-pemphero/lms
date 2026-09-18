<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeofOwnershipDocument.aspx.cs" Inherits="Test" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change of Ownership</title>

    <style>
        * {
            box-sizing: border-box;
        }

        body {
            font-family: "Book Antiqua", Georgia, serif;
            margin: 0;
            background: #e9e9e9;
            color: #222;
            font-size: 12pt;
        }

        .document {
            width: 210mm;
            min-height: 297mm;
            margin: 10px auto;
            background: #fff;
            padding: 15mm;
            border: 1px solid #ccc;
        }

        .header {
            text-align: center;
            margin-bottom: 8px;
        }

            .header img {
                width: 100%;
                height: auto;
            }

        .title {
            text-align: center;
            font-size: 18pt;
            font-weight: bold;
            text-transform: uppercase;
            letter-spacing: 1px;
            margin: 8px 0;
            border-bottom: 2px solid #000;
            padding-bottom: 5px;
        }

        table.details {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }

            table.details td {
                border: 1px solid #000;
                padding: 6px 8px;
                vertical-align: top;
            }

                table.details td.label {
                    width: 32%;
                    font-weight: bold;
                    background: #f5f5f5;
                }

        .section-title {
            font-size: 13pt;
            font-weight: bold;
            margin-top: 15px;
            margin-bottom: 5px;
            border-bottom: 1px solid #000;
            padding-bottom: 3px;
        }

        table.signatures {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }

            table.signatures td {
                width: 50%;
                padding: 14px 10px;
                vertical-align: bottom;
            }

        .line {
            border-bottom: 1px solid #000;
            height: 28px;
            margin-bottom: 4px;
        }

        .sig-title {
            font-weight: bold;
            font-size: 11pt;
        }

        .attachments {
            margin-top: 10px;
            font-size: 11pt;
        }

        .footer {
            margin-top: 12px;
            border-top: 1px solid #000;
            padding-top: 8px;
            text-align: center;
            font-size: 10pt;
            line-height: 1.4;
        }

        .buttons {
            text-align: center;
            margin: 15px;
        }

        .btn {
            padding: 10px 25px;
            border: none;
            color: white;
            cursor: pointer;
            font-weight: bold;
            border-radius: 4px;
        }

        .btn-back {
            background: #666;
        }

        .btn-print {
            background: #0a8f3d;
        }



        @media print {
            @page {
                size: A4 portrait;
                margin: 12mm 10mm; /* Adjust these values as needed */
            }

            * {
                -webkit-print-color-adjust: exact !important;
                print-color-adjust: exact !important;
                color-adjust: exact !important;
            }

            html, body {
                width: 100%;
                margin: 0;
                padding: 0;
                background: white;
            }

            .document {
                width: 100%;
                max-width: none;
                margin: 0;
                padding: 12mm 10mm; /* Match @page margins for consistency */
                border: none;
                box-shadow: none;
                background: #fff;
                min-height: auto;
            }

            .buttons,
            input[type=button],
            input[type=submit],
            button,
            .btn {
                display: none !important;
            }
        }
    </style>

</head>

<body>

    <form id="form1" runat="server">

        <asp:Label ID="lblCode" runat="server" Visible="false"></asp:Label>

        <div class="document">

            <div class="header">
                <img src="../assets/img/New innobuild letter head.png" />
            </div>

            <div class="title">
                CHANGE OF OWNERSHIP FORM
            </div>

            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>

            <table class="details">

                <tr>
                    <td class="label">Previous Client</td>
                    <td><strong>
                        <asp:Label ID="lblCustomer1" runat="server"></asp:Label></strong></td>
                </tr>

                <tr>
                    <td class="label">New Client</td>
                    <td><strong>
                        <asp:Label ID="lblNewClient" runat="server"></asp:Label></strong></td>
                </tr>

                <tr>
                    <td class="label">Site Name</td>
                    <td>
                        <asp:Label ID="lblSiteName" runat="server"></asp:Label></td>
                </tr>

                <tr>
                    <td class="label">Plot Number</td>
                    <td>
                        <asp:Label ID="lblPlotNo" runat="server"></asp:Label></td>
                </tr>

                <tr>
                    <td class="label">Plot Size</td>
                    <td>
                        <asp:Label ID="lblPlotSize" runat="server"></asp:Label></td>
                </tr>

                <tr>
                    <td class="label">Processing Fee</td>
                    <td>
                        <asp:Label ID="lblFee" runat="server"></asp:Label></td>
                </tr>

                <tr>
                    <td class="label">Payment Mode</td>
                    <td>
                        <asp:Label ID="lblPaymentMode" runat="server"></asp:Label></td>
                </tr>

                <tr>
                    <td class="label">Receipt No.</td>
                    <td>
                        <asp:Label ID="lblProofofPayment" runat="server"></asp:Label></td>
                </tr>

                <tr>
                    <td class="label">Reason for Change</td>
                    <td>
                        <asp:Label ID="lblReason" runat="server"></asp:Label></td>
                </tr>

            </table>

            <div class="section-title">
                Signatures
            </div>

            <table class="signatures">

                <tr>

                    <td>
                        <div class="line"></div>
                        <div class="sig-title">Prepared By</div>
                        <asp:Label ID="lblPreparedby" runat="server"></asp:Label>
                    </td>

                    <td>
                        <div class="line"></div>
                        <div class="sig-title">Previous Customer</div>
                        <asp:Label ID="lblPrevCustomer" runat="server"></asp:Label>
                    </td>

                </tr>

                <tr>

                    <td>
                        <div class="line"></div>
                        <div class="sig-title">New Customer</div>
                        <asp:Label ID="lblNewCustomer" runat="server"></asp:Label>
                    </td>

                    <td>
                        <div class="line"></div>
                        <div class="sig-title">Checked By</div>
                        <asp:Label ID="lblCheckedby" runat="server"></asp:Label>
                    </td>

                </tr>

                <tr>

                    <td>
                        <div class="line"></div>
                        <div class="sig-title">Approved By</div>
                        Billy Jonathan Chiwotha
                    </td>

                    <td></td>

                </tr>

            </table>

            <div class="attachments">
                <strong>Attachments:</strong>
                Copies of National IDs for both Clients.
            </div>

            <div class="footer">
                <strong>Prepared by Roberts & Franklin Law Consultants</strong><br />
                Area 4, Along Paul Kagame Road, Next to Regional Road Traffic Offices<br />
                P.O. Box 30195, Lilongwe • Cell: 0888 558 542<br />
                Email: mbwanafrank@gmail.com
            </div>

        </div>

        <div class="buttons">

            <asp:Button
                ID="Button1"
                runat="server"
                Text="← Back"
                CssClass="btn btn-back"
                OnClick="Button1_Click1" />

            &nbsp;&nbsp;

    <asp:Button
        ID="Button2"
        runat="server"
        Text="Print"
        CssClass="btn btn-print"
        OnClick="btnPrint_Click" />

        </div>

        <asp:Label ID="lblSaleAgreementId" runat="server" Visible="false"></asp:Label>

    </form>

</body>
</html>
