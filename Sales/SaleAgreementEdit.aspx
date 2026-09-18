<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleAgreementEdit.aspx.cs" Inherits="Plotwithdrawal" %>

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
    <style>
        /* Checkbox List */
        .styled-checkboxlist input[type="checkbox"] {
            margin-right: 8px;
            accent-color: var(--primary);
        }

        .styled-checkboxlist label {
            display: flex;
            align-items: center;
            padding: 6px 0;
            font-size: 14px;
            color: var(--text);
        }

        .plot {
            border: 1px solid #e5e7eb;
            border-radius: var(--radius);
            padding: 10px 15px;
            background: #fff;
            max-height: 200px;
            overflow-y: auto;
        }

        .full-width {
            grid-column: span 2;
        }

        /* ======= Modern Checkbox List Styling ======= */
        .plot-list {
            background: #fff;
            border: 1px solid #e5e7eb;
            border-radius: 10px;
            padding: 16px 20px;
            max-height: 250px;
            overflow-y: auto;
            box-shadow: 0 2px 8px rgba(0,0,0,0.05);
        }

        /* Remove default ASP.NET checkbox list layout */
        .styled-checkboxlist input[type="checkbox"] {
            appearance: none;
            width: 18px;
            height: 18px;
            border: 2px solid #ccc;
            border-radius: 6px;
            cursor: pointer;
            transition: all 0.2s ease;
            position: relative;
        }

            .styled-checkboxlist input[type="checkbox"]:checked {
                background-color: var(--primary, #2563eb);
                border-color: var(--primary, #2563eb);
            }

                .styled-checkboxlist input[type="checkbox"]:checked::after {
                    content: "✔";
                    position: absolute;
                    color: #fff;
                    font-size: 12px;
                    top: 0;
                    left: 3px;
                }

        /* Label alignment */
        .styled-checkboxlist label {
            display: flex;
            align-items: center;
            gap: 10px;
            font-size: 15px;
            color: #333;
            padding: 8px 0;
            cursor: pointer;
            transition: background 0.2s ease;
        }

            .styled-checkboxlist label:hover {
                background: #f9fafb;
                border-radius: 6px;
            }

        /* Scrollbar customization */
        .student-list::-webkit-scrollbar {
            width: 6px;
        }

        .student-list::-webkit-scrollbar-thumb {
            background: #cbd5e1;
            border-radius: 10px;
        }
    </style>
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
                        <!--Customers-->
                        <div class="user">

                            <div class="info">

                                <a class="" data-toggle="collapse" href="#customers" aria-expanded="true">

                                    <span>
                                        <span class="user-level">CUSTOMERS</span>
                                        <span class="caret"></span>
                                    </span>
                                </a>
                                <div class="clearfix"></div>

                                <div class="collapse in" id="customers" aria-expanded="true" style="">
                                    <ul class="nav">

                                        <li>
                                            <a href="ViewCustomers.aspx">
                                                <span class="link-collapse">Customers</span>
                                            </a>
                                        </li>

                                    </ul>
                                </div>
                            </div>
                        </div>
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
                                                <span class="link-collapse">View All Sites</span>
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
                                            <a href="PlotAllocation.aspx">
                                                <span class="link-collapse">Plot Allocation</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="SalesAgreement.aspx">
                                                <span class="link-collapse">Sale Agreement</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="ChangeofOwnership.aspx">
                                                <span class="link-collapse">Change Ownership</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="ViewPlots.aspx">
                                                <span class="link-collapse">View All Plots</span>
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

                <div class="main-panel">
                    <div class="content">
                        <div class="container-fluid">

                            <div class="row">
                                <div class="col-md-1"></div>
                                <div class="col-md-10">
                                    <div class="card">
                                        <div class="card-header">
                                            <div class="card-title">EDIT SALE AGREEMENT</div>
                                        </div>
                                        <div class="card-body">

                                            <div class="form-group">
                                                <asp:Label runat="server" ID="lblError" Style="font-size: large; color: #FF3300"></asp:Label>&nbsp;<asp:Label runat="server" ID="lblSuccess" Style="font-weight: 700; color: #009900"></asp:Label>



                                                <div class="form-group">
                                                    <label for="solidInput">Select Plot to Remove from Sales Agreement</label>
                                                    <div class="form-group full-width">

                                                        <asp:DropDownList ID="drpPlots" runat="server" CssClass="form-control input-solid">
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div class="card-action">

                                                        <asp:Button runat="server" ID="btnSave" class="btn btn-block btn-primary" Text="Withdraw Plot" Font-Bold="true" OnClick="btnSave_Click" />


                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-1"></div>
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
                                        Plot Removed from Offer Successfully!
                                    </div>
                                    <!-- Footer -->
                                    <div class="modal-footer border-0 d-flex justify-content-center">
                                        <asp:Button runat="server" ID="btnExit" class="btn btn-block custom-btn btn-outline-primary" Style="padding: 10px 20px; font-size: 16px;" Text="Done" OnClick="btnExit_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>


                        <!--Other usefull Labels-->
                        <asp:Label runat="server" ID="lblSession" Visible="false"></asp:Label>

                        <asp:Label runat="server" ID="lblDutyStation" Visible="false"></asp:Label>

                        <asp:Label runat="server" ID="lblAgreementID" Visible="false"></asp:Label>

                        <asp:Label runat="server" ID="lblSiteCode" Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lblDateOffered" Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lblClientCode" Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lblPrice2" Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="lblPaid2" Visible="false"></asp:Label>
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
