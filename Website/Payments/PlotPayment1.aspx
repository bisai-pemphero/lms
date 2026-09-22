<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeFile="PlotPayment1.aspx.cs" Inherits="Customers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Innobuild</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <link rel="stylesheet" href="../assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" />
    <link rel="stylesheet" href="../assets/css/ready.css" />
    <link rel="stylesheet" href="../assets/css/demo.css" />

    <!--Modal jQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <!-- Bootstrap JavaScript -->
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

    <script type="text/javascript" language="javascript">

        function DisableBackButton() {
            window.history.forward()
        }
        DisableBackButton();
        window.onload = DisableBackButton;
        window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
        window.onunload = function () { void (0) }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="wrapper">
                <div class="main-header">
                    <div class="logo-header">
                        LAND MANAGEMENT
		
                <!-- Top bar -->
                        <button class="navbar-toggler sidenav-toggler ml-auto" type="button" data-toggle="collapse" data-target="collapse" aria-controls="sidebar" aria-expanded="false" aria-label="Toggle navigation">
                            <span class="navbar-toggler-icon"></span>
                        </button>
                        <button class="topbar-toggler more"><i class="la la-ellipsis-v"></i></button>
                    </div>
                    <nav class="navbar navbar-header navbar-expand-lg">
                        <div class="container-fluid">


                            <ul class="navbar-nav topbar-nav ml-md-auto align-items-center">

                                <li class="nav-item dropdown">
                                    <a class="dropdown-toggle profile-pic" data-toggle="dropdown" href="#" aria-expanded="false">
                                        <img src="../assets/img/user.png" alt="user-img" width="36" class="img-circle"><span>
                                            <asp:Label runat="server" ID="lblUser"></asp:Label>
                                        </span></span> </a>
                                    <ul class="dropdown-menu dropdown-user">
                                        <a class="dropdown-item" href="../UserLogin.aspx"><i class="la la-power-off"></i>Logout</a>
                                        <a class="dropdown-item" href="ChangePassword.aspx"><i class="la la-gears"></i>Change Password</a>
                                    </ul>
                                    <ul class="dropdown-menu dropdown-user">
                                    </ul>

                                </li>
                            </ul>
                        </div>
                    </nav>
                </div>
                <!-- Side bar links -->
                <div class="sidebar">
                    <div class="scrollbar-inner sidebar-wrapper">
                        <ul class="nav">
                            <li class="nav-item active">
                                <a href="Dashboard.aspx">
                                    <i class="la la-dashboard"></i>
                                    <p>Dashboard</p>
                                </a>
                            </li>

                        </ul>
                        <!--Payments-->
                        <div class="user">
                            <div class="info">
                                <a class="" data-toggle="collapse" href="#documents" aria-expanded="true">
                                    <span>
                                        <span class="user-level">PAYMENTS</span>
                                        <span class="caret"></span>
                                    </span>
                                </a>
                                <div class="clearfix"></div>
                                <div class="collapse in" id="documents" aria-expanded="true" style="">
                                    <ul class="nav">
                                        <li>
                                            <a href="PlotPayment.aspx">
                                                <span class="link-collapse">Plot Payment</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="ChangeofOwnership.aspx">
                                                <span class="link-collapse">Change of Ownership</span>
                                            </a>
                                        </li>

                                    </ul>
                                </div>
                            </div>
                        </div>

                        <!--Reports-->
                        <div class="user">
                            <div class="info">
                               
                                <div class="clearfix"></div>

                                <div class="collapse in" id="reports" aria-expanded="true" style="">
                                    <ul class="nav">
                                        <li>
                                            <a href="SalesReport.aspx">
                                                <span class="link-collapse">Sales Report</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="ViewPlots.aspx">
                                                <span class="link-collapse">Plots</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="ViewAllSites.aspx">
                                                <span class="link-collapse">Sites</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="ViewCustomers.aspx">
                                                <span class="link-collapse">Customers</span>
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>

                        <!--Receipts-->
                        <div class="user">
                            <div class="info">
                                <a class="" data-toggle="collapse" href="#receipts" aria-expanded="true">
                                    <span>
                                        <span class="user-level">RECEIPTS</span>
                                        <span class="caret"></span>
                                    </span>
                                </a>
                                <div class="clearfix"></div>

                                <div class="collapse in" id="receipts" aria-expanded="true" style="">
                                    <ul class="nav">
                                        <li>
                                            <a href="SelectReceipt.aspx">
                                                <span class="link-collapse">Reprint Receipt</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="SelectReceipt2.aspx">
                                                <span class="link-collapse">Change of Ownership Receipt</span>
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>

                         <!--Settings-->
 <div class="user">

     <div class="info">

         <a class="" data-toggle="collapse" href="#settings" aria-expanded="true">

             <span>
                 <span class="user-level">SETTINGS</span>
                 <span class="caret"></span>
             </span>
         </a>
         <div class="clearfix"></div>

         <div class="collapse in" id="settings" aria-expanded="true" style="">
             <ul class="nav">

                 <li>
                     <a href="ChangePassword.aspx">
                         <span class="link-collapse">Change Password</span>
                     </a>

                     <a href="../UserLogin.aspx">
                         <span class="link-collapse">Logout</span>
                     </a>
                 </li>

             </ul>
         </div>
     </div>
 </div>
                    </div>
                </div>
                <!--Content-->
                <div class="main-panel">
                    <div class="content">
                        <div class="container-fluid">

                            <div class="row">
                                <div class="col-md-2"></div>
                                <div class="col-md-8">
                                    <div class="card">
                                        <div class="card-header">
                                            <div class="card-title">
                                                PLOT PAYMENT FOR 
                                                <asp:Label runat="server" ID="lblFor"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="card-body">

                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lblError" Style="font-size: large; color: #FF3300"></asp:Label>&nbsp;<asp:Label runat="server" ID="lblSuccess" Style="font-weight: 700; color: #009900"></asp:Label>
                                                <br />
                                                <br />
                                                <div class="form-group">
                                                    <label for="solidInput">Site </label>
                                                    <asp:TextBox CssClass="form-control input-solid" ID="txtSite" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                                <div class="form-group">
                                                    <label for="solidInput">Plot No </label>
                                                    <asp:TextBox CssClass="form-control input-solid" ID="txtPlotNo" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                                <label for="solidInput">Amount Paying <strong> (Balance:  <asp:Label runat="server" ID="lblBalance"></asp:Label>) </strong></label>
                                                <asp:TextBox class="form-control input-solid" ID="txtAmountPaying" runat="server" TextMode="Number"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="solidInput">Discount</label>
                                                <asp:TextBox class="form-control input-solid" ID="txtdiscount" runat="server" TextMode="Number"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="solidInput">Payment Mode</label>
                                                <asp:DropDownList ID="drpPayMode" runat="server" class="form-control input-solid">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem>Cash</asp:ListItem>
                                                    <asp:ListItem>Cheque</asp:ListItem>
                                                    <asp:ListItem>Bank Transfer</asp:ListItem>
                                                    <asp:ListItem>AirtelMoney</asp:ListItem>
                                                    <asp:ListItem>Mpamba</asp:ListItem>
                                                    <asp:ListItem>Other</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="form-group">
                                                <label for="solidInput">Payment Reference</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Reference" ID="txtReference" runat="server"></asp:TextBox>
                                            </div>
                                             <div class="form-group">
     <label for="solidInput">Payment Date</label>
     <asp:TextBox class="form-control input-solid" ID="txtDate" runat="server" TextMode="DateTimeLocal"></asp:TextBox>
 </div>


                                        </div>
                                        <div class="card-action">
                                            <asp:Button runat="server" ID="btnSave" class="btn btn-block btn-primary" Text="Save" Font-Bold="true" OnClick="btnSave_Click" />
                                          
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <!--Important labels-->
                                <asp:Label runat="server" ID="lblSession" Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lblReceiptNumber" Visible="false"></asp:Label>

                                <asp:Label runat="server" ID="lblPhoneNumber" Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lblEmail" Visible="false"></asp:Label>

                                <asp:Label runat="server" ID="lblDutyStation" Visible="false"></asp:Label>

                                <asp:Label runat="server" ID="lblTotalPaid" Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lblPreviousBalance" Visible="false"></asp:Label>

                                <asp:Label runat="server" ID="lblClient" Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lblPlotNo" Visible="false"></asp:Label>

                                <asp:Label runat="server" ID="lblAddress" Visible="false"></asp:Label>

                            </div>
                        </div>
                    </div>
                </div>

                <!--Success Modal-->
                <div class="modal fade" id="successModal" tabindex="-1" role="dialog" aria-labelledby="successLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content">
                            <!-- Header with Icon -->
                            <div class="modal-header border-0 d-flex flex-column align-items-center">

                                <h4 class="modal-title mt-2" style="font-size: 24px; color: steelblue;" id="successLabel"></h4>
                                <br />
                                <hr />
                                <i class="la la-check-circle" style="font-size: 60px; color: #2170b9;"></i>
                            </div>
                            <!-- Body -->
                            <div class="modal-body text-center" style="font-size: 16px; color: #555;">
                                Email Sent Successfully!
                            </div>
                            <!-- Footer -->
                            <div class="modal-footer border-0 d-flex justify-content-center">
                                <asp:Button runat="server" ID="btnExit" class="btn btn-block custom-btn btn-outline-primary" Style="padding: 10px 20px; font-size: 16px;" Text="Done" OnClick="btnExit_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <!--Update Modal-->
                <div class="modal fade" id="UpdateModal" tabindex="-1" role="dialog" aria-labelledby="UpdateLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content">
                            <!-- Header with Icon -->
                            <div class="modal-header border-0 d-flex flex-column align-items-center">

                                <h4 class="modal-title mt-2" style="font-size: 24px; color: steelblue;" id="UpdateLabel"></h4>
                                <br />
                                <hr />
                                <i class="la la-check-circle" style="font-size: 60px; color: #2170b9;"></i>
                            </div>
                            <!-- Body -->
                            <div class="modal-body text-center" style="font-size: 16px; color: #555;">
                                Customer Details Updated Successfully!
                            </div>
                            <!-- Footer -->
                            <div class="modal-footer border-0 d-flex justify-content-center">
                                <asp:Button runat="server" ID="Button1" class="btn btn-block custom-btn btn-outline-primary" Style="padding: 10px 20px; font-size: 16px;" Text="Done" OnClick="btnExit_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <!--Delete Modal-->
                <div class="modal fade" id="DeleteModal" tabindex="-1" role="dialog" aria-labelledby="DeleteLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content">
                            <!-- Header with Icon -->
                            <div class="modal-header border-0 d-flex flex-column align-items-center">

                                <h4 class="modal-title mt-2" style="font-size: 24px; color: steelblue;" id="DeleteLabel"></h4>
                                <br />
                                <hr />
                                <i class="la la-check-circle" style="font-size: 60px; color: #2170b9;"></i>
                            </div>
                            <!-- Body -->
                            <div class="modal-body text-center" style="font-size: 16px; color: #555;">
                                Customer Details Deleted Successfully!
                            </div>
                            <!-- Footer -->
                            <div class="modal-footer border-0 d-flex justify-content-center">
                                <asp:Button runat="server" ID="Button2" class="btn btn-block custom-btn btn-outline-primary" Style="padding: 10px 20px; font-size: 16px;" Text="Done" OnClick="btnExit_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </form>
</body>

<script src="../assets/js/core/jquery.3.2.1.min.js"></script>
<script src="../assets/js/plugin/jquery-ui-1.12.1.custom/jquery-ui.min.js"></script>
<script src="../assets/js/core/popper.min.js"></script>
<script src="../assets/js/core/bootstrap.min.js"></script>
<script src="../assets/js/plugin/chartist/chartist.min.js"></script>
<script src="../assets/js/plugin/chartist/plugin/chartist-plugin-tooltip.min.js"></script>

<script src="../assets/js/plugin/bootstrap-toggle/bootstrap-toggle.min.js"></script>
<script src="../assets/js/plugin/jquery-mapael/jquery.mapael.min.js"></script>
<script src="../assets/js/plugin/jquery-mapael/maps/world_countries.min.js"></script>
<script src="../assets/js/plugin/chart-circle/circles.min.js"></script>
<script src="../assets/js/plugin/jquery-scrollbar/jquery.scrollbar.min.js"></script>
<script src="../assets/js/ready.min.js"></script>
<script src="../assets/js/demo.js"></script>

</html>
