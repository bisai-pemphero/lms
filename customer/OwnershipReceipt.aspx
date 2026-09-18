<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnershipReceipt.aspx.cs" Inherits="Test" %>


<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
<title>Innobuild</title>

    <!--Modal jQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <!-- Bootstrap JavaScript -->
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>


    <script type="text/javascript">
        function DisableBackButton() {
            window.history.forward();
        }
        DisableBackButton();
        window.onload = DisableBackButton;
        window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton(); };
        window.onunload = function () { void (0); };

        function printReceipt() {
            window.print();
        }
    </script>

    <link rel="stylesheet" href="../assets/vendors/ti-icons/css/themify-icons.css" />

    <style>
        body {
            font-family: "Arial", sans-serif;
            background-color: #f8f9fa;
            margin: 0;
            padding: 20px;
            text-align: center;
        }

        .receipt-container {
            max-width: 320px;
            background: #fff;
            padding: 15px;
            border-radius: 5px;
            box-shadow: 0px 2px 10px rgba(0, 0, 0, 0.1);
            margin: auto;
        }

        .company-logo {
            max-width: 60px;
            display: block;
            margin: auto;
        }

        .company-info {
            font-size: 14px;
            text-align: center;
            font-weight: bold;
            margin-bottom: 10px;
        }

        .pos-address, .pos-phone {
            font-size: 12px;
            text-align: center;
        }

        .receipt-header {
            font-size: 12px;
            text-align: center;
            font-weight: bold;
            padding: 8px 0;
            border-bottom: 1px dashed #000;
        }

        .receipt-info {
            font-size: 12px;
            text-align: left;
            margin: 10px 0;
        }

            .receipt-info label {
                font-weight: bold;
            }

        .table-container {
            width: 100%;
            font-size: 12px;
            text-align: left;
            margin-top: 10px;
            border-top: 1px dashed #000;
            border-bottom: 1px dashed #000;
            padding: 5px 0;
        }

            .table-container table {
                width: 100%;
                border-collapse: collapse;
            }

            .table-container th, .table-container td {
                padding: 5px;
                text-align: left;
            }

            .table-container th {
                font-weight: bold;
                border-bottom: 1px solid #000;
            }

        .totals {
            font-size: 12px;
            text-align: right;
            margin-top: 5px;
            font-weight: bold;
        }

        .footer {
            font-size: 12px;
            text-align: center;
            font-weight: bold;
            margin-top: 10px;
        }

        .footer-end {
            font-size: 12px;
            text-align: center;
            margin-top: 10px;
        }

        .button-container {
            margin-top: 15px;
        }

        .button-container {
            display: flex;
            gap: 10px;
        }

            .button-container input,
            .button-container button {
                padding: 10px 18px;
                font-size: 14px;
                border: none;
                cursor: pointer;
                border-radius: 5px;
                display: flex;
                align-items: center;
                gap: 8px;
                font-weight: bold;
                transition: all 0.3s ease;
                box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.2);
            }

        .btn-print {
            background: #007bff;
            color: white;
        }

        /*for link buttons*/
        .btn-email,
        .btn-whatsapp,
        .btn-back{
            display: inline-flex;
            align-items: center;
            padding: 10px 18px;
            font-size: 14px;
            font-weight: bold;
            text-decoration: none;
            border-radius: 5px;
            border: none;
            cursor: pointer;
            transition: all 0.3s ease;
            gap: 8px;
            box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.2);
            color: white;
        }

        .btn-email {
            background: #007bff;
        }

        .btn-whatsapp {
            background: #25d366;
        }

        /* Hover Effects */
        .btn-email:hover {
            background: #0056b3;
        }

        .btn-whatsapp:hover {
            background: #1da851;
        }

        .btn-icon-prepend {
            font-size: 16px;
        }

        /**End here*/
        .btn-back {
            background: #28a745;
            color: white;
        }

        /* Hover Effects */
        .btn-print:hover {
            background: #0056b3;
        }

        .btn-back:hover {
            background: #1e7e34;
        }

        @media print {
            body {
                background: none;
                padding: 0;
                margin: 0;
            }

            .receipt-container {
                box-shadow: none;
                border: none;
                width: 58mm;
            }

            .button-container {
                display: none;
            }
        }

        @font-face {
            font-family: 'IDAutomationHC39MFree';
            src: url('fonts/IDAutomationHC39M.ttf') format('truetype');
            /* Adjust the URL based on where your font file is located */
        }

        .barcode {
            font-family: 'IDAutomationHC39MFree', 'IDAutomationHC39M Free Version', sans-serif;
            font-size: 15px;
            margin-top: 10px;
        }
    </style>
</head>
<body>
    <form runat="server">
        <div class="receipt-container">
                      <center>
               <img src="../assets/img/Innobuild logo small-black.png" alt="Innobuild" class="company-logo" />
           </center>

           <div class="company-info">
               INNOBUILD PRIVATE LIMITED
           </div>

           <div class="pos-address">
               P.O. Box 1967,<br />
               Lilongwe, Malawi.<br /><br />
               innobuildmw@gmail.com
           </div>

           <div class="pos-phone">
              
           <br />
           +265 991 148 500 /  +265 888 002 899
<br />
           
            </div>
            <hr />
           <p> CHANGE OF OWNERSHIP PAYMENT RECEIPT</p>
            <hr />
            <div class="receipt-info">
                <label>Receipt No:</label>
                <asp:Label ID="lblReceipNo" runat="server"></asp:Label><br />
                <label>Branch: </label>
                <asp:Label ID="lblBranch" runat="server"></asp:Label>
                <br />
                <label>Operator:</label>
                <asp:Label ID="lblUser" runat="server"></asp:Label>
                <br />
                <label>Date:</label>
                <asp:Label ID="lblDate" runat="server"></asp:Label>

            </div>
            <div class="table-container">
                <div class="pos-received-from">
                    Customer : <strong>
                        <asp:Label ID="lblCustomer" runat="server"></asp:Label>
                    </strong>
                </div>
                <div class="pos-plot-no">
                    Site Name: <strong>
                        <asp:Label ID="lblSite" runat="server"></asp:Label>
                    </strong>
                </div>
                <div class="pos-plot-no">
                    For Plot No: <strong>
                        <asp:Label ID="lblPlotNo" runat="server"></asp:Label>
                    </strong>
                </div>

               
                <div class="pos-total">
                    Amount Paid : <strong>
                        <asp:Label ID="lblPaid" runat="server"> </asp:Label>
                    </strong>
                </div>
               
                <div class="pos-price">
                    Payment Mode : <strong>
                        <asp:Label ID="lblMode" runat="server"></asp:Label>
                    </strong>
                </div>
            </div>

            <div class="pos-address">

                 </div>
            <div class="footer-end">
                *** Making Malawi for Malawians First ***
            </div>

            <div class="button-container">

                <asp:LinkButton runat="server" class="btn-email" ID="btnEmail" OnClientClick="printReceipt()">
     <i class="ti-printer  btn-icon-prepend"></i> Print
                </asp:LinkButton>

                <asp:LinkButton runat="server" class="btn-back" ID="btnBack" OnClick="btnBack_Click">
                         Back
                </asp:LinkButton>

                <%--   <asp:LinkButton runat="server" CssClass="btn-whatsapp" ID="btnWhatsapp" OnClick="btnWhatsapp_Click">
    <i class="ti-comment btn-icon-prepend"></i> WhatsApp
</asp:LinkButton>--%>
            </div>

            <div class="footer">
                <asp:Label ID="lblError" runat="server" Style="color: #FF3300"></asp:Label>
                <asp:Label ID="lblPhoneNumber" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="lblEmail" runat="server" Visible="false"></asp:Label>
            </div>
        </div>

        <%--         <!--Success Modal-->
 <div class="modal fade" id="successModal" tabindex="-1" role="dialog" aria-labelledby="successLabel" aria-hidden="true">
     <div class="modal-dialog modal-dialog-centered" role="document">
         <div class="modal-content">
             <!-- Header with Close Button -->
             <div class="modal-header border-0 d-flex flex-column align-items-center">
                 <!-- Close Button -->
                 <button type="button" class="close position-absolute" style="right: 15px; top: 15px;" data-dismiss="modal" aria-label="Close">
                     <span aria-hidden="true">&times;</span>
                 </button>

                 <h4 class="modal-title mt-2" style="font-size: 24px; color: steelblue;" id="successLabel">Success</h4>
                 <br />
                 <hr />
                 <i class="ti-email" style="font-size: 60px; color: #2170b9;"></i>
             </div>
             <!-- Body -->
             <div class="modal-body text-center" style="font-size: 16px; color: #555;">
                Email Sent Successfully!
             </div>
             <!-- Footer with Close Button -->
             <div class="modal-footer border-0 justify-content-center">
                 <button type="button" class="btn btn-outline-primary btn-lg btn-block" data-dismiss="modal">Close</button>
             </div>
         </div>
     </div>
 </div>--%>
    </form>
</body>
</html>

