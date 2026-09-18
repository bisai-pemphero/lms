<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeofOwnershipDocument.aspx.cs" Inherits="Test" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change of Ownership</title>
    <style>
        body {
            font-family: "Book Antiqua", Georgia, serif;
            margin: 30px;
            background-color: #fdfdfd;
            color: #333;
        }

        .container {
            width: 80%;
            margin: auto;
            background: #fff;
            padding: 25px 40px;
            border: 1px solid #ddd;
            border-radius: 6px;
            box-shadow: 0px 2px 6px rgba(0,0,0,0.1);
        }

        header {
            text-align: center;
            margin-bottom: 5px
        }

            header img {
                max-width: 100%;
                height: auto;
            }

        h1 {
            font-size: 16pt;
            margin-bottom: 5px;
            text-align: center;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        p {
            font-size: 12pt;
        }

        .field-label {
            display: inline-block;
            width: 260px;
            font-weight: bold;
        }

        .signature {
            margin-top: 40px;
        }

            .signature p {
                margin-bottom: 5px;
                font-size: 12pt;
            }

        .signature-line {
            display: inline-block;
            width: 50%;
            border-bottom: 1px solid #333;
            margin: 0 10px;
        }

        .footer {
            margin-top: 40px;
            text-align: left;
            font-size: 10pt;
            line-height: 1.5;
            color: #555;
        }

        .buttons {
            margin-top: 30px;
            text-align: center;
        }

        .btn {
            padding: 10px 20px;
            font-weight: bold;
            border: none;
            cursor: pointer;
            border-radius: 4px;
        }

        .btn-back {
            background-color: #666;
            color: #fff;
        }

        .btn-print {
            background-color: #009933;
            color: #fff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label runat="server" ID="lblCode" Visible="false"></asp:Label>

        <div class="container">
            <header>
                <img src="../assets/img/New innobuild letter head.png" alt="Company Letterhead" />
                <h1>Change of Ownership</h1>
                <hr />
            </header>

            <asp:Label runat="server" ID="lblError" ForeColor="Red"></asp:Label>

            <p>
                <span class="field-label">Name of Client/Customer:</span>
                <strong>
                    <asp:Label runat="server" ID="lblCustomer1"></asp:Label></strong>
            </p>

            <p>
                <span class="field-label">New Client/Customer:</span>
                <strong>
                    <asp:Label runat="server" ID="lblNewClient"></asp:Label></strong>
            </p>

            <p>
                <span class="field-label">Site Name:</span>
                <strong>
                    <asp:Label runat="server" ID="lblSiteName"></asp:Label></strong>
            </p>

            <p>
                <span class="field-label">Plot Number:</span>
                <strong>
                    <asp:Label runat="server" ID="lblPlotNo"></asp:Label></strong>
            </p>

            <p>
                <span class="field-label">Plot Size:</span>
                <strong>
                    <asp:Label runat="server" ID="lblPlotSize"></asp:Label></strong>
            </p>

            <p>
                <span class="field-label">Processing Fee:</span>
                <strong>
                    <asp:Label runat="server" ID="lblFee"></asp:Label></strong>
            </p>

            <p>
                <span class="field-label">Mode of Payment:</span>
                <strong>
                    <asp:Label runat="server" ID="lblPaymentMode"></asp:Label></strong>
            </p>

            <p>
                <span class="field-label">Proof of Payment/Receipt No.:</span>
                <strong>
                    <asp:Label runat="server" ID="lblProofofPayment"></asp:Label></strong>
            </p>

            <p>
                <span class="field-label">Reason for Change of Ownership:</span>
                <strong>
                    <asp:Label runat="server" ID="lblReason"></asp:Label></strong>
            </p>

            <div class="signature">
                <p>
                    Prepared by <span class="signature-line"></span>(Signature)<br />
                    <strong>
                        <asp:Label runat="server" ID="lblPreparedby"></asp:Label></strong>
                </p>

                <p>
                    Previous Customer <span class="signature-line"></span>(Signature)<br />
                    <strong>
                        <asp:Label runat="server" ID="lblPrevCustomer"></asp:Label></strong>
                </p>

                <p>
                    New Customer <span class="signature-line"></span>(Signature)<br />
                    <strong>
                        <asp:Label runat="server" ID="lblNewCustomer"></asp:Label></strong>
                </p>

                <p>
                    Checked by <span class="signature-line"></span>(Signature)<br />
                    <strong>
                        <asp:Label runat="server" ID="lblCheckedby"></asp:Label></strong>
                </p>

                <p>
                    Approved by <span class="signature-line"></span>(Signature)<br />
                    <strong>Billy Jonathan Chiwotha</strong>
                </p>
            </div>

            <p><strong>Attachments:</strong> National IDs for both Clients.</p>

            <div class="footer">
                <strong>Prepared by: Roberts & Franklin Law Consultants</strong><br />
                Area 4, Along Paul Kagame Road, Next to Regional Road Traffic Offices<br />
                P.O. Box 30195, Lilongwe<br />
                Cell: 0 888 558 542<br />
                Email: mbwanafrank@gmail.com
            </div>


        </div>

        <div class="buttons">
            <asp:Button ID="Button1" runat="server" Text="&lt; Back" CssClass="btn btn-back" OnClick="Button1_Click1" />
            &nbsp;
    <asp:Button ID="Button2" runat="server" Text="Print" CssClass="btn btn-print" OnClick="btnPrint_Click" />
        </div>
    </form>
</body>
</html>
