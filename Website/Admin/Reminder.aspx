<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeFile="Reminder.aspx.cs" Inherits="Customers" %>

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
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.21/css/jquery.dataTables.min.css" />

    <style>
        /* Custom Styles for Better UX */
        .content-wrapper {
            padding: 20px;
            background: #f8f9fa;
            min-height: calc(100vh - 120px);
        }

        .stats-card {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border-radius: 15px;
            padding: 20px;
            margin-bottom: 20px;
            color: white;
            box-shadow: 0 10px 40px rgba(0,0,0,0.1);
            transition: transform 0.3s ease;
        }

            .stats-card:hover {
                transform: translateY(-5px);
            }

            .stats-card h3 {
                font-size: 32px;
                font-weight: bold;
                margin-bottom: 5px;
            }

            .stats-card p {
                margin: 0;
                opacity: 0.9;
            }

        .stats-icon {
            font-size: 48px;
            opacity: 0.3;
            position: absolute;
            right: 20px;
            top: 20px;
        }

        .action-card {
            background: white;
            border-radius: 15px;
            padding: 25px;
            margin-bottom: 30px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.05);
            border: 1px solid #e9ecef;
        }

        .btn-send-penalty {
            background: #dc3545;
            border: none;
            padding: 12px 30px;
            font-size: 16px;
            font-weight: 600;
            border-radius: 10px;
            transition: all 0.3s ease;
            color: white;
        }

            .btn-send-penalty:hover {
                transform: translateY(-2px);
                box-shadow: 0 5px 20px rgba(102, 126, 234, 0.4);
            }

        .btn-send-reminder {
            background: #0094ff;
            border: none;
            padding: 12px 30px;
            font-size: 16px;
            font-weight: 600;
            border-radius: 10px;
            transition: all 0.3s ease;
            color: white;
        }

            .btn-send-reminder:hover {
                transform: translateY(-2px);
                box-shadow: 0 5px 20px rgba(102, 126, 234, 0.4);
            }

            .btn-send-reminder:disabled {
                background: #6c757d;
                transform: none;
            }

        .alert-custom {
            border-radius: 10px;
            border-left: 4px solid;
            padding: 15px 20px;
            margin-bottom: 20px;
        }

            .alert-custom.alert-warning {
                background-color: #fff3cd;
                border-left-color: #ffc107;
            }

            .alert-custom.alert-success {
                background-color: #d4edda;
                border-left-color: #28a745;
            }

            .alert-custom.alert-danger {
                background-color: #f8d7da;
                border-left-color: #dc3545;
            }

        .info-badge {
            background: #e9ecef;
            border-radius: 8px;
            padding: 8px 15px;
            font-size: 14px;
            color: #495057;
            margin-right: 10px;
        }

            .info-badge i {
                margin-right: 5px;
                color: #667eea;
            }

        .table-container {
            background: white;
            border-radius: 15px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.05);
        }

        .dataTables_wrapper .dataTables_length,
        .dataTables_wrapper .dataTables_filter {
            margin-bottom: 20px;
        }

            .dataTables_wrapper .dataTables_filter input {
                border: 1px solid #dee2e6;
                border-radius: 8px;
                padding: 8px 15px;
                margin-left: 10px;
            }

        .dataTables_wrapper .dataTables_paginate .paginate_button {
            border-radius: 8px;
            padding: 6px 12px;
            margin: 0 3px;
        }

            .dataTables_wrapper .dataTables_paginate .paginate_button.current {
                background: #0094ff;
                border-color: #667eea;
                color: white !important;
            }

        .table thead th {
            background: #f8f9fa;
            border-bottom: 2px solid #dee2e6;
            color: #495057;
            font-weight: 600;
            font-size: 14px;
        }

        .table tbody tr:hover {
            background-color: #f8f9fa;
            transition: background-color 0.2s ease;
        }

        .status-badge {
            padding: 4px 12px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 600;
            display: inline-block;
        }

            .status-badge.sent {
                background: #d4edda;
                color: #155724;
            }

            .status-badge.pending {
                background: #fff3cd;
                color: #856404;
            }

        .modal-content {
            border-radius: 20px;
            border: none;
        }

        .modal-header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            border-radius: 20px 20px 0 0;
        }

            .modal-header i {
                color: white;
            }

        .modal-footer {
            border-top: none;
            padding: 20px;
        }

        .btn-modal-close {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border: none;
            padding: 10px 30px;
            border-radius: 10px;
            color: white;
            font-weight: 600;
            transition: all 0.3s ease;
        }

            .btn-modal-close:hover {
                transform: translateY(-2px);
                box-shadow: 0 5px 15px rgba(102, 126, 234, 0.3);
            }

        .loading-overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.7);
            z-index: 9999;
            justify-content: center;
            align-items: center;
        }

        .loading-content {
            background: white;
            padding: 30px;
            border-radius: 15px;
            text-align: center;
        }

        .spinner {
            border: 4px solid #f3f3f3;
            border-top: 4px solid #667eea;
            border-radius: 50%;
            width: 50px;
            height: 50px;
            animation: spin 1s linear infinite;
            margin: 0 auto 20px;
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        @media (max-width: 768px) {
            .stats-card h3 {
                font-size: 24px;
            }

            .action-card {
                padding: 15px;
            }

            .btn-send-reminder {
                width: 100%;
            }

            .btn-send-penalty {
                width: 100%;
            }
        }
    </style>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

    <script type="text/javascript">
        function DisableBackButton() {
            window.history.forward()
        }
        DisableBackButton();
        window.onload = DisableBackButton;
        window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
        window.onunload = function () { void (0) }

        function showLoading() {
            document.getElementById('loadingOverlay').style.display = 'flex';
        }

        function hideLoading() {
            document.getElementById('loadingOverlay').style.display = 'none';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Loading Overlay -->
        <div id="loadingOverlay" class="loading-overlay">
            <div class="loading-content">
                <div class="spinner"></div>
                <h4>Sending Reminders...</h4>
                <p>Please wait while we process your request</p>
            </div>
        </div>

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
                                    <img src="../assets/img/user.png" alt="user-img" width="36" class="img-circle">
                                    <span>
                                        <asp:Label runat="server" ID="lblUser"></asp:Label></span>
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

            <!-- Sidebar -->
            <div class="sidebar">
                <div class="scrollbar-inner sidebar-wrapper">
                    <ul class="nav">
                        <li class="nav-item">
                            <a href="Dashboard.aspx">
                                <i class="la la-dashboard"></i>
                                <p>Dashboard</p>
                            </a>
                        </li>

                    </ul>
                                      <!-- SITES -->
                   <div class="user">
                       <div class="info">
                           <a data-toggle="collapse" href="#sites" aria-expanded="true">
                               <span><span class="user-level">SITES</span><span class="caret"></span></span>
                           </a>
                           <div class="collapse in" id="sites">
                               <ul class="nav">
                                   <li><a href="ViewAllSites.aspx"><span class="link-collapse">Site Approval</span></a></li>
                                   <li><a href="ViewSites.aspx"><span class="link-collapse">View Sites</span></a></li>
                               </ul>
                           </div>
                       </div>
                   </div>
                   <!-- PLOTS -->
                   <div class="user">
                       <div class="info">
                           <a data-toggle="collapse" href="#plots" aria-expanded="true">
                               <span><span class="user-level">PLOTS</span><span class="caret"></span></span>
                           </a>
                           <div class="collapse in" id="plots">
                               <ul class="nav">
                                   <li><a href="ViewAllPlots.aspx"><span class="link-collapse">Plot Details</span></a></li>
                               </ul>
                           </div>
                       </div>
                   </div>
                   <!-- DOCUMENTS -->
                   <div class="user">
                       <div class="info">
                           <a data-toggle="collapse" href="#documents" aria-expanded="true">
                               <span><span class="user-level">DOCUMENTS</span><span class="caret"></span></span>
                           </a>
                           <div class="collapse in" id="documents">
                               <ul class="nav">
                                   <li><a href="OfferLetterDocuments.aspx"><span class="link-collapse">Offer Letters</span></a></li>
                                   <li><a href="SaleAgreementDocument.aspx"><span class="link-collapse">Sales Agreements</span></a></li>
                                   <li><a href="PlotHistory.aspx"><span class="link-collapse">Plot History</span></a></li>
                                   <li><a href="ChangeofOwnership.aspx"><span class="link-collapse">Change of Ownership</span></a></li>
                                    <li>
    <a href="Vouchers.aspx">
        <span class="link-collapse">Payment Voucher</span>
    </a>
                                         <a href="VoucherCreditor.aspx">
    <span class="link-collapse">Creditor Voucher</span>
</a>
</li>
                               </ul>
                           </div>
                       </div>
                   </div>
                   <!-- REPORTS -->
                   <div class="user">
                       <div class="info">
                           <a data-toggle="collapse" href="#reports" aria-expanded="true">
                               <span><span class="user-level">REPORTS</span><span class="caret"></span></span>
                           </a>
                           <div class="collapse in" id="reports">
                               <ul class="nav">
                                   <li><a href="SalesReport.aspx"><span class="link-collapse">Sales Report</span></a></li>
                                   <li><a href="SelectReceipt.aspx"><span class="link-collapse">Plot Payments</span></a></li>
                                   <li><a href="SelectReceipt2.aspx"><span class="link-collapse">Ownership Payments</span></a></li>
                                   <li><a href="ViewCustomers.aspx"><span class="link-collapse">View Customers</span></a></li>
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
            <!-- Main Content -->
            <div class="main-panel">
                <div class="content">
                    <div class="container-fluid">
                        <!-- Page Header -->


                        <!-- Table Section -->
                        <div class="row">
                            <div class="col-12">
                                <div class="table-container">
                                    <div class="d-flex justify-content-between align-items-center mb-4">
                                        <h5 style="color: #2c3e50; font-weight: 600; margin: 0;">
                                            <i class="la la-list"></i>Customers Due for Payment
                                        </h5>
                                        <div class="info-badge">
                                            <i class="la la-info-circle"></i>Showing customers with overdue payments
                                        </div>
                                    </div>

                                    <div class="table-responsive">
                                        <table id="sentRemindersTable" class="table table-hover">
                                            <thead>
                                                <tr>
                                                    <th>Plot #</th>
                                                    <th>Site Name</th>
                                                    <th>Client Name</th>
                                                    <th>Phone No</th>
                                                    <th>Installment</th>
                                                    <th>Agreed Price</th>
                                                    <th>Amount Paid</th>
                                                    <th>Balance</th>
                                                    <th>Penalty Charge</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <% LoadReminderLogs(); %>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:Label runat="server" ID="lblSession" Visible="false"></asp:Label>
                    </div>
                </div>

                <!-- Success Modal -->
                <div class="modal fade" id="successModal" tabindex="-1" role="dialog" aria-labelledby="successLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content">

                            <!-- Body -->
                            <div class="modal-body text-center" style="font-size: 16px; color: #555;">
                                Reminders Sent Successfully to all Customers!
                            </div>
                            <!-- Footer -->
                            <div class="modal-footer border-0 d-flex justify-content-center">
                                <asp:Button runat="server" ID="btnExit" class="btn btn-block custom-btn btn-outline-primary" Style="padding: 10px 20px; font-size: 16px;" Text="Done" OnClick="btnExit_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Update Modal -->
                <div class="modal fade" id="UpdateModal" tabindex="-1" role="dialog" aria-labelledby="UpdateLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content">
                            <div class="modal-header text-center border-0">
                                <i class="la la-check-circle" style="font-size: 60px;"></i>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body text-center">
                                <h4 class="mb-3" style="color: #2c3e50;">Updated!</h4>
                                <p style="color: #6c757d; font-size: 16px;">Customer details have been updated successfully.</p>
                            </div>
                            <div class="modal-footer d-flex justify-content-center">
                                <asp:Button runat="server" ID="Button1" class="btn btn-modal-close" Text="Done" OnClick="btnExit_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Delete Modal -->
                <div class="modal fade" id="ErrorModal" tabindex="-1" role="dialog" aria-labelledby="DeleteLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content">

                            <div class="modal-body text-center">
                                <h4 class="mb-3" style="color: #2c3e50;">Deleted!</h4>
                                <p style="color: #6c757d; font-size: 16px;">Sorry, Reminders have already been sent.</p>
                            </div>
                            <div class="modal-footer d-flex justify-content-center">
                                <asp:Button runat="server" ID="Button2" class="btn btn-modal-close" Text="Done" OnClick="btnExit_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <!-- Scripts -->
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
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.21/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.2/js/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.print.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#sentRemindersTable').DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "info": true,
                "autoWidth": false,
                "responsive": true,
                "language": {
                    "search": "Search:",
                    "lengthMenu": "Show _MENU_ entries",
                    "info": "Showing _START_ to _END_ of _TOTAL_ entries"
                }
            });

            // Hide loading overlay when page loads (in case of postback)
            hideLoading();
        });

        // Update current month display
        const months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
        const currentDate = new Date();
        document.getElementById('currentMonth').textContent = months[currentDate.getMonth()] + ' ' + currentDate.getFullYear();

        // You can populate stats via AJAX or code-behind
        function updateStats(totalCustomers, totalAmount) {
            document.getElementById('totalCustomers').textContent = totalCustomers;
            document.getElementById('totalAmount').textContent = 'K' + totalAmount.toLocaleString();
        }
    </script>
</body>
</html>