<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VoucherPrintCreditor.aspx.cs"
    Inherits="VoucherPrint" EnableEventValidation="false" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Payment Voucher</title>

    <style>
        @page {
            size: A4;
            margin: 5mm;
        }

        html, body {
            width: 210mm;
            min-height: 297mm;
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: #f3f3f3;
            color: #333;
            -webkit-print-color-adjust: exact;
            print-color-adjust: exact;
            font-size: 12px;
        }

        .a4-container {
            width: 195mm;
            min-height: 282mm;
            margin: 5px auto;
            background: #fff;
            padding: 5mm;
            box-sizing: border-box;
            box-shadow: 0 0 15px rgba(0,0,0,.15);
        }

         .company-header {
            text-align: center;
            margin-bottom: 10px;
            border-bottom: 3px solid #1c4d8c;
            padding-bottom: 10px;
        }

        .company-header img {
            max-width: 100%;
            height: auto;
        }

        .title {
            text-align: center;
            font-size: 20px;
            font-weight: bold;
            color: #1c4d8c;
            letter-spacing: 1px;
            margin: 8px 0;
        }

        .section-title {
            background: #1c4d8c;
            color: white;
            padding: 4px 10px;
            font-size: 11px;
            font-weight: bold;
            margin-top: 8px;
        }

        .modern-table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 6px;
        }

        .modern-table td {
            border: 1px solid #ddd;
            padding: 4px 6px;
            font-size: 12px;
        }

        .modern-table tr:nth-child(even) {
            background: #fafafa;
        }

        .label {
            width: 30%;
            font-weight: bold;
            background: #f8f9fa;
            font-size: 12px;
        }

        .amount-box {
            background: #eef7ff;
            border: 2px solid #1c4d8c;
            padding: 6px;
            text-align: center;
            margin-top: 4px;
        }

        .amount-box h2 {
            margin: 0;
            color: #1c4d8c;
            font-size: 20px;
        }

        .amount-box span {
            font-size: 12px;
        }

        .description-box {
            border: 1px solid #ddd;
            min-height: 60px;
            padding: 6px 8px;
            font-size: 12px;
            line-height: 1.5;
        }

        @media print {

            body {
                background: white;
            }

            .a4-container {
                box-shadow: none;
                margin: 0;
                padding: 4mm;
                width: 100%;
                min-height: auto;
            }

            .action-buttons {
                display: none;
            }
        }

        .action-buttons {
    text-align: center;
    margin: 30px 0;
}

.btn {
    display: inline-block;
    padding: 12px 28px;
    margin: 0 8px;
    border: none;
    border-radius: 6px;
    font-size: 15px;
    font-weight: 600;
    font-family: Arial, Helvetica, sans-serif;
    text-decoration: none;
    cursor: pointer;
    transition: all 0.3s ease;
}

/* Print Button */
.btn-print {
    background: #007bff;
    color: #fff;
}

.btn-print:hover {
    background: #0056b3;
    transform: translateY(-2px);
    box-shadow: 0 4px 10px rgba(0,123,255,0.3);
}

/* Back Button */
.btn-back {
    background: #6c757d;
    color: #fff;
}

.btn-back:hover {
    background: #545b62;
    transform: translateY(-2px);
    box-shadow: 0 4px 10px rgba(108,117,125,0.3);
}

.btn:active {
    transform: scale(0.98);
}

@media print {
    .action-buttons {
        display: none;
    }
}
    </style>

</head>

<body>

<form id="form1" runat="server">

<div class="a4-container">

    <!-- Company Header -->

    <div class="company-header">
        <img src="../assets/img/New innobuild letter head.png" />
    </div>

    <div class="title">
        PAYMENT VOUCHER
    </div>

    <!-- Voucher Information -->

    <div class="section-title">
        VOUCHER INFORMATION
    </div>

    <table class="modern-table">

        <tr>

            <td class="label">Voucher No</td>

            <td>
                <asp:Label ID="lblVoucherNo" runat="server"></asp:Label>
            </td>

            <td class="label">Status</td>

            <td>
                <asp:Label ID="lblStatus" runat="server"></asp:Label>
            </td>

        </tr>

        <tr>

            <td class="label">Reference No</td>

            <td>
                <asp:Label ID="lblReferenceNo" runat="server"></asp:Label>
            </td>

            <td class="label">Date Prepared</td>

            <td>
                <asp:Label ID="lblDatePrepared" runat="server"></asp:Label>
            </td>

        </tr>

    </table>

    <!-- Payee Information -->

    <div class="section-title">
        PAYEE INFORMATION
    </div>

    <table class="modern-table">

        <tr>

            <td class="label">Payee Name</td>

            <td colspan="3">
                <asp:Label ID="lblPayeeName" runat="server"></asp:Label>
            </td>

        </tr>

        <tr>

            <td class="label">Payment Method</td>

            <td>
                <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label>
            </td>

            <td class="label">Cheque No</td>

            <td>
                <asp:Label ID="lblChequeNo" runat="server"></asp:Label>
            </td>

        </tr>

        <tr>

            <td class="label">Bank Name</td>

            <td>
                <asp:Label ID="lblBankName" runat="server"></asp:Label>
            </td>

            <td class="label">Account No</td>

            <td>
                <asp:Label ID="lblAccountNo" runat="server"></asp:Label>
            </td>

        </tr>

    </table>

    <!-- Payment Details -->

    <div class="section-title">
        PAYMENT DETAILS
    </div>

    <table class="modern-table">

        <tr>

            <td class="label">Amount</td>

            <td colspan="3">

                <div class="amount-box">

                    <h2 runat="server" id="lblAmount">
                       
                       
                    </h2>

                    <span>Total Payment Amount</span>

                </div>

            </td>

        </tr>

        <tr>

            <td class="label">
                Amount In Words
            </td>

            <td colspan="3">

                <strong>
                    <asp:Label ID="lblAmountWords" runat="server"></asp:Label>
                </strong>

            </td>

        </tr>

        <tr>

            <td class="label">
                Description
            </td>

            <td colspan="3">

                <div class="description-box">

                    <asp:Label ID="lblDescription" runat="server"></asp:Label>

                </div>

            </td>

        </tr>

    </table>

        <!-- Authorization Section -->

    <div class="section-title">
        AUTHORIZATION
    </div>

    <table class="modern-table">

        <tr>
            <td class="label">Prepared By</td>
            <td>
                <asp:Label ID="lblPreparedBy" runat="server"></asp:Label>
            </td>

            <td class="label">Date Prepared</td>
            <td>
                <asp:Label ID="lblPreparedDate" runat="server"></asp:Label>
            </td>
        </tr>

        <tr>
            <td class="label">Approved By</td>
            <td>
                <asp:Label ID="lblApprovedBy" runat="server"></asp:Label>
            </td>

            <td class="label">Date Approved</td>
            <td>
                <asp:Label ID="lblDateApproved" runat="server"></asp:Label>
            </td>
        </tr>

        <tr>
            <td class="label">Received By</td>
            <td>
                <asp:Label ID="lblReceivedBy" runat="server"></asp:Label>
            </td>

            <td class="label">Date Received</td>
            <td>
                <asp:Label ID="lblDateReceived" runat="server"></asp:Label>
            </td>
        </tr>

    </table>

    <br />
    <br />

    <!-- Signatures -->

    <table style="width:100%; border-collapse:collapse; margin-top:20px;">

        <tr>

            <td style="width:33%; text-align:center;">

                __________________________

                <br />

                <strong>
                    <asp:Label ID="lblPreparedBySignature"
                        runat="server"></asp:Label>
                </strong>

                <br />

                Prepared By

            </td>

            <td style="width:33%; text-align:center;">

                __________________________

                <br />

                <strong>
                    <asp:Label ID="lblApprovedBySignature"
                        runat="server"></asp:Label>
                </strong>

                <br />

                Approved By

            </td>

            <td style="width:33%; text-align:center;">

                __________________________

                <br />

                <strong>
                    <asp:Label ID="lblReceivedBySignature"
                        runat="server"></asp:Label>
                </strong>

                <br />

                Received By

            </td>

        </tr>

    </table>

    <br />
    <br />

    <!-- Footer -->

    <table style="width:100%; border-collapse:collapse;">

        <tr>

            <td style="text-align:left; font-size:10px; color:#666;">

                Printed on:
                <asp:Label ID="lblPrintedDate"
                    runat="server"></asp:Label>

            </td>

            <td style="text-align:right; font-size:10px; color:#666;">

                Payment Voucher #
                <asp:Label ID="lblVoucherFooter"
                    runat="server"></asp:Label>

            </td>

        </tr>

    </table>

    <br />

    <!-- Declaration -->

    <div style="border:1px solid #ddd;
                padding:8px 10px;
                background:#fafafa;
                font-size:10px;
                line-height:1.5;">

        <strong>Declaration</strong>

        <br /><br />

        I acknowledge receiving the amount stated above as full or partial
        settlement for the purpose described in this voucher.

    </div>

    <br />

   <!-- Action Buttons -->
<div class="action-buttons">
    <button type="button"
            class="btn btn-print"
            onclick="window.print();">
        🖨 Print Voucher
    </button>

    <a href="Dashboard.aspx" class="btn btn-back">
        ← Back
    </a>
</div>

</div>

</form>

<script>

    window.onload = function () {

        var today = new Date();

        var options = {
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        };

        var lbl = document.getElementById('<%= lblPrintedDate.ClientID %>');

        if (lbl)
            lbl.innerHTML = today.toLocaleDateString('en-US', options);

    };

</script>

</body>

</html>