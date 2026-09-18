<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SiteRegister.aspx.cs" Inherits="Site" %>

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
                        <!--Sites-->
                        <div class="user">

                            <div class="info">

                                <a class="" data-toggle="collapse" href="#sites" aria-expanded="true">

                                    <span>
                                        <span class="user-level">SITES</span>
                                        <span class="caret"></span>
                                    </span>
                                </a>
                                <div class="clearfix"></div>

                                <div class="collapse in" id="sites" aria-expanded="true" style="">
                                    <ul class="nav">

                                        <li>
                                            <a href="ViewAllSites.aspx">
                                                <span class="link-collapse">Site Approval</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="ViewSites.aspx">
                                                <span class="link-collapse">View Sites</span>
                                            </a>
                                        </li>

                                    </ul>
                                </div>
                            </div>
                        </div>
                        <!--Plots-->
                        <div class="user">

                            <div class="info">

                                <a class="" data-toggle="collapse" href="#plots" aria-expanded="true">

                                    <span>
                                        <span class="user-level">PLOTS</span>
                                        <span class="caret"></span>
                                    </span>
                                </a>
                                <div class="clearfix"></div>

                                <div class="collapse in" id="plots" aria-expanded="true" style="">
                                    <ul class="nav">
                                        <li>
                                            <a href="ViewAllPlots.aspx">
                                                <span class="link-collapse">Plot Details</span>
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <!--Documents-->
                        <div class="user">
                            <div class="info">
                                <a class="" data-toggle="collapse" href="#documents" aria-expanded="true">
                                    <span>
                                        <span class="user-level">DOCUMENTS</span>
                                        <span class="caret"></span>
                                    </span>
                                </a>
                                <div class="clearfix"></div>
                                <div class="collapse in" id="documents" aria-expanded="true" style="">
                                    <ul class="nav">
                                        <li>
                                            <a href="OfferLetterDocuments.aspx">
                                                <span class="link-collapse">Offer Letters</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="SaleAgreementDocument.aspx">
                                                <span class="link-collapse">Sales Agreements</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="PlotHistory.aspx">
                                                <span class="link-collapse">Plot History</span>
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
                                <a class="" data-toggle="collapse" href="#reports" aria-expanded="true">
                                    <span>
                                        <span class="user-level">REPORTS</span>
                                        <span class="caret"></span>
                                    </span>
                                </a>
                                <div class="clearfix"></div>

                                <div class="collapse in" id="reports" aria-expanded="true" style="">
                                    <ul class="nav">
                                        <li>
                                            <a href="SalesReport.aspx">
                                                <span class="link-collapse">Sales Report</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="SelectReceipt.aspx">
                                                <span class="link-collapse">Plot Payments</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="SelectReceipt2.aspx">
                                                <span class="link-collapse">Change of Ownership Payments</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="ViewCustomers.aspx">
                                                <span class="link-collapse">View Customers</span>
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
                        <!--Dashboard-->
                    </div>
                </div>

                <div class="main-panel">
                    <div class="content">
                        <div class="container-fluid">

                            <div class="row">
                                <div class="col-md-2"></div>
                                <div class="col-md-8">
                                    <div class="card">

                                        <div class="card-body">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lblError" Style="font-size: large; color: #FF3300"></asp:Label>&nbsp;<asp:Label runat="server" ID="lblSuccess" Style="font-weight: 700; color: #009900"></asp:Label>

                                            </div>

                                            <div class="form-group">
                                                <label for="solidInput">Site Name</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Site Name" ID="txtLocation" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>

                                            <div class="form-group">
                                                <label for="solidInput">Site District</label>
                                                <asp:DropDownList ID="drpDistrict" runat="server" class="form-control input-solid">
                                                </asp:DropDownList>

                                            </div>
                                            <div class="form-group">
                                                <label for="solidInput">Region</label>
                                                <asp:DropDownList ID="drpRegion" runat="server" class="form-control input-solid">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Central Region" Value="Central Region"></asp:ListItem>
                                                    <asp:ListItem Text="Southern Region" Value="Southern Region"></asp:ListItem>
                                                    <asp:ListItem Text="Northern Region" Value="Northern Region"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="form-group">
                                                <label for="solidInput">Traditional Authority</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Traditional Authority" ID="txtTA" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="solidInput">Village/Town</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Village or Town name" ID="txtVillage" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="solidInput">Site Description</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="What better describes the site" ID="txtDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="solidInput">Land Size</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Land Size" ID="txtSize" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </div>


                                            <div class="form-group">
                                                <label for="solidInput">Previous Owner</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Previous Owner" ID="txtPreviousOwner" runat="server" CssClass="form-control"></asp:TextBox>

                                            </div>

                                            <div class="form-group">
                                                <label for="solidInput">Initial Value</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Initial Value" ID="txtInitialValue" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>

                                            </div>

                                            <div class="form-group">
                                                <label for="solidInput">Amount Paid</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Amount Paid" ID="txtAmountPaid" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="solidInput">Price Per Square Meter</label>
                                                <asp:TextBox class="form-control input-solid" ID="txtPricePerSquareMeters" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                            </div>

               
                                            <div class="form-group">
                                                <label for="solidInput">Developments Done</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Developments" ID="txtDevelopment" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>

                                            <div class="form-group">
                                                <label for="solidInput">Development Costs</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Development Cost" ID="txtDevelopmentCost" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                            </div>

                                            <!--Start here -->


                                            <div class="form-group">
                                                <label for="solidInput">Bank Name</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Bank Name" ID="txtBank" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="solidInput">Account Name</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Account Name" ID="txtAccountName" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="solidInput">Account Number</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Creditors Account Number" ID="txtAccountNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="solidInput">Branch</label>
                                                <asp:TextBox class="form-control input-solid" placeholder="Branch" ID="txtBranch" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>

                                            <div class="form-group">
                                                <label for="solidInput">Is this our Site?</label>
                                                <asp:DropDownList runat="server" class="form-control input-solid" ID="drpOwner">
                                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
                                                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>

                                        </div>
                                        <div class="card-action">


                                            <asp:Button runat="server" ID="btnUpdate" class="btn btn-success" Text="Approve Site Register" Font-Bold="true" OnClick="btnUpdate_Click" />
                                            <asp:Button runat="server" ID="btnDelete" class="btn btn-danger" Text="Delete Site" Font-Bold="true" OnClick="btnDelete_Click" />

                                            <asp:Label runat="server" ID="lblSiteCode" Visible="false"></asp:Label>
                                            <asp:Label runat="server" ID="lblSession" Visible="false"></asp:Label>
                                            <asp:Label runat="server" ID="lblUser" Visible="false"></asp:Label>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2"></div>
                        </div>
                    </div>
                </div>

                <!--Notifications Modals-->
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
                                Site Approved Successfully!
                            </div>
                            <!-- Footer -->
                            <div class="modal-footer border-0 d-flex justify-content-center">
                                <asp:Button runat="server" ID="btnExit" class="btn btn-block custom-btn btn-outline-primary" Style="padding: 10px 20px; font-size: 16px;" Text="Done" OnClick="btnExit_Click" />
                            </div>
                        </div>
                    </div>
                </div>


                <!--Notifications Modals-->
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
                                Site Approved Successfully!
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
                                Site Deleted Successfully!
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
