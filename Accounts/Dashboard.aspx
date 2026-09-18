<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link rel="icon" type="image/png" href="../assets/img/favicon.png" />
    <title>Innobuild</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <link rel="stylesheet" href="../assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" />
    <link rel="stylesheet" href="../assets/css/ready.css" />
    <link rel="stylesheet" href="../assets/css/demo.css" />

    <style>
        /* Quick Action Button Styles - Professional Desktop Design */
        .quick-action-card {
            transition: all 0.25s ease;
            cursor: pointer;
            border-radius: 10px;
            border: 1px solid rgba(0,0,0,0.04);
            height: 100%;
            min-height: 130px;
            display: flex;
            align-items: center;
            justify-content: center;
            text-align: center;
            padding: 18px 12px;
            background: #ffffff;
            box-shadow: 0 1px 3px rgba(0,0,0,0.06);
            position: relative;
            overflow: hidden;
        }

            .quick-action-card::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                height: 4px;
                transition: height 0.3s ease;
            }

            .quick-action-card:hover::before {
                height: 6px;
            }

            .quick-action-card:hover {
                transform: translateY(-4px);
                box-shadow: 0 8px 30px rgba(0,0,0,0.10);
                border-color: rgba(0,0,0,0.08);
            }

            .quick-action-card a {
                text-decoration: none;
                color: #2c3e50;
                display: block;
                width: 100%;
                height: 100%;
                padding: 5px;
            }

            .quick-action-card .action-icon {
                font-size: 2.2rem;
                display: block;
                margin-bottom: 8px;
                transition: transform 0.3s ease;
            }

            .quick-action-card:hover .action-icon {
                transform: scale(1.1);
            }

            .quick-action-card .action-title {
                font-size: 0.9rem;
                font-weight: 700;
                margin: 0;
                letter-spacing: 0.2px;
                color: #1a2332;
            }

            .quick-action-card .action-sub {
                font-size: 0.7rem;
                opacity: 0.65;
                margin-top: 2px;
                font-weight: 400;
                color: #6c7a8d;
            }

        /* Color variations with top border accent */
        .action-primary::before {
            background: #1572e8;
        }

        .action-primary .action-icon {
            color: #1572e8;
        }

        .action-primary:hover {
            border-color: #1572e8;
        }

        .action-success::before {
            background: #10b759;
        }

        .action-success .action-icon {
            color: #10b759;
        }

        .action-success:hover {
            border-color: #10b759;
        }

        .action-warning::before {
            background: #ffc107;
        }

        .action-warning .action-icon {
            color: #ffc107;
        }

        .action-warning:hover {
            border-color: #ffc107;
        }

        .action-info::before {
            background: #17a2b8;
        }

        .action-info .action-icon {
            color: #17a2b8;
        }

        .action-info:hover {
            border-color: #17a2b8;
        }

        .action-secondary::before {
            background: #6c757d;
        }

        .action-secondary .action-icon {
            color: #6c757d;
        }

        .action-secondary:hover {
            border-color: #6c757d;
        }

        /* Quick Actions Section */
        .quick-actions-section {
            margin-top: 25px;
            padding: 25px 20px 15px 20px;
            background: #fafbfc;
            border-radius: 14px;
            border: 1px solid #eef0f2;
        }

        .quick-actions-title {
            font-size: 1.05rem;
            font-weight: 700;
            color: #1a2332;
            margin-bottom: 20px;
            display: flex;
            align-items: center;
        }

            .quick-actions-title i {
                margin-right: 10px;
                color: #1572e8;
                font-size: 1.2rem;
            }

        /* Stats Cards Enhancement */
        .card-stats {
            border-radius: 10px;
            border: none;
            box-shadow: 0 1px 4px rgba(0,0,0,0.06);
            transition: all 0.25s ease;
            background: #ffffff;
        }

            .card-stats:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 20px rgba(0,0,0,0.08);
            }

            .card-stats .card-body {
                padding: 18px 15px;
            }

            .card-stats .icon-big {
                font-size: 2.2rem;
            }

            .card-stats .card-category {
                font-size: 0.75rem;
                font-weight: 600;
                text-transform: uppercase;
                letter-spacing: 0.4px;
                opacity: 0.7;
                color: #6c7a8d;
            }

            .card-stats .card-title {
                font-size: 1.6rem;
                font-weight: 800;
                margin: 3px 0 0 0;
                color: #1a2332;
            }

        .card-primary .icon-big {
            color: #1572e8;
        }

        .card-warning .icon-big {
            color: #ffc107;
        }

        .card-success .icon-big {
            color: #10b759;
        }

        /* Mobile Responsive */
        @media (max-width: 768px) {
            .quick-action-card {
                min-height: 110px;
                padding: 15px 10px;
            }

                .quick-action-card .action-icon {
                    font-size: 1.8rem;
                }

                .quick-action-card .action-title {
                    font-size: 0.8rem;
                }

                .quick-action-card .action-sub {
                    font-size: 0.65rem;
                }

            .quick-actions-section {
                padding: 18px 12px 10px 12px;
            }

            .card-stats .card-title {
                font-size: 1.3rem;
            }
        }

        @media (max-width: 576px) {
            .quick-action-card {
                min-height: 95px;
                padding: 12px 8px;
            }

                .quick-action-card .action-icon {
                    font-size: 1.5rem;
                    margin-bottom: 4px;
                }

                .quick-action-card .action-title {
                    font-size: 0.7rem;
                }

                .quick-action-card .action-sub {
                    display: none;
                }
        }
    </style>

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
                                            <asp:Label runat="server" ID="lblUser">Admin</asp:Label>
                                        </span></span>
                                    </a>
                                    <ul class="dropdown-menu dropdown-user">
                                        <a class="dropdown-item" href="../UserLogin.aspx"><i class="la la-power-off"></i>Logout</a>
                                        <a class="dropdown-item" href="ChangePassword.aspx"><i class="la la-gears"></i>Change Password</a>
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
                                        <li><a href="PlotPayment.aspx"><span class="link-collapse">Plot Payment</span></a></li>
                                        <li><a href="ChangeofOwnership.aspx"><span class="link-collapse">Change of Ownership</span></a></li>
                                        <li><a href="Vouchers.aspx"><span class="link-collapse">Payment Voucher</span></a></li>
                                        <li><a href="CreditorVouchers.aspx"><span class="link-collapse">Creditor Voucher</span></a></li>
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
                                        <li><a href="SalesReport.aspx"><span class="link-collapse">Sales Report</span></a></li>
                                        <li><a href="ViewPlots.aspx"><span class="link-collapse">Plots</span></a></li>
                                        <li><a href="ViewAllSites.aspx"><span class="link-collapse">Sites</span></a></li>
                                        <li><a href="ViewCustomers.aspx"><span class="link-collapse">Customers</span></a></li>
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
                                        <li><a href="SelectReceipt.aspx"><span class="link-collapse">Reprint Receipt</span></a></li>
                                        <li><a href="SelectReceipt2.aspx"><span class="link-collapse">Change of Ownership Receipt</span></a></li>
                                    </ul>
                                </div>
                            </div>
                        </div>

                        <!--Reminders-->
                        <div class="user">
                            <div class="info">
                                <a class="" data-toggle="collapse" href="#reminders" aria-expanded="true">
                                    <span>
                                        <span class="user-level">REMINDERS</span>
                                        <span class="caret"></span>
                                    </span>
                                </a>
                                <div class="clearfix"></div>
                                <div class="collapse in" id="reminders" aria-expanded="true" style="">
                                    <ul class="nav">
                                        <li><a href="Reminder.aspx"><span class="link-collapse">Send Reminder</span></a></li>
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
                                            <a href="ChangePassword.aspx"><span class="link-collapse">Change Password</span></a>
                                            <a href="../UserLogin.aspx"><span class="link-collapse">Logout</span></a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!--Dashboard-->
                <div class="main-panel">
                    <div class="content">
                        <div class="container-fluid">
                            <h4 class="page-title">Accounts Dashboard</h4>

                            <!-- Stats Cards -->
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="card card-stats card-primary">
                                        <div class="card-body">
                                            <div class="row">
                                               
                                                <div class="col-12 d-flex align-items-center">
                                                    <div class="numbers">
                                                        <p class="card-category">Today</p>
                                                        <h4 class="card-title">
                                                            <asp:Label runat="server" ID="lblToday">0</asp:Label>
                                                        </h4>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="card card-stats card-warning">
                                        <div class="card-body">
                                            <div class="row">
                                               
                                                <div class="col-12 d-flex align-items-center">
                                                    <div class="numbers">
                                                        <p class="card-category">This Week</p>
                                                        <h4 class="card-title">
                                                            <asp:Label runat="server" ID="lblThisWeek">0</asp:Label>
                                                        </h4>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="card card-stats card-success">
                                        <div class="card-body">
                                            <div class="row">
                                               
                                                <div class="col-12 d-flex align-items-center">
                                                    <div class="numbers">
                                                        <p class="card-category">This Month</p>
                                                        <h4 class="card-title">
                                                            <asp:Label runat="server" ID="lblThismonth"> 0</asp:Label>
                                                        </h4>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="quick-actions-section">
                                <div class="quick-actions-title">
                                    <i class="la la-bolt"></i>Quick Actions
                                </div>
                                <div class="row">

                                    <div class="col-xl-2 col-lg-2 col-md-4 col-sm-4 col-6 mb-3">
                                        <div class="quick-action-card action-primary">
                                            <a href="PlotPayment.aspx">
                                                <span class="action-icon la la-credit-card"></span>
                                                <p class="action-title">Plot Payment</p>

                                            </a>
                                        </div>
                                    </div>




                                    <div class="col-xl-2 col-lg-2 col-md-4 col-sm-4 col-6 mb-3">
                                        <div class="quick-action-card action-info">
                                            <a href="ChangeofOwnership.aspx">
                                                <span class="action-icon la la-exchange"></span>
                                                <p class="action-title">Ownership</p>

                                            </a>
                                        </div>
                                    </div>


                                    <div class="col-xl-2 col-lg-2 col-md-4 col-sm-4 col-6 mb-3">
                                        <div class="quick-action-card action-secondary">
                                            <a href="CreditorVouchers.aspx">
                                                <span class="action-icon la la-folder"></span>
                                                <p class="action-title">Creditor Voucher</p>

                                            </a>
                                        </div>
                                    </div>

                                    <div class="col-xl-2 col-lg-2 col-md-4 col-sm-4 col-6 mb-3">
                                        <div class="quick-action-card action-success">
                                            <a href="SalesReport.aspx">
                                                <span class="action-icon la la-file-text"></span>
                                                <p class="action-title">Sales Report</p>

                                            </a>
                                        </div>
                                    </div>

                                    <div class="col-xl-2 col-lg-2 col-md-4 col-sm-4 col-6 mb-3">
                                        <div class="quick-action-card action-warning">
                                            <a href="SelectReceipt.aspx">
                                                <span class="action-icon la la-clipboard"></span>
                                                <p class="action-title">Reprint Receipt</p>

                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:Label runat="server" ID="lblSession" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="lblLocation" Visible="false"></asp:Label>
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
