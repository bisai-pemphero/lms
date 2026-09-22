<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Dashboard</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <link rel="stylesheet" href="../assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" />
    <link rel="stylesheet" href="../assets/css/ready.css" />
    <link rel="stylesheet" href="../assets/css/demo.css" />
    <!-- DataTables CSS -->
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.21/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/buttons/1.6.2/css/buttons.dataTables.min.css" />

    <style>
        /* ----- global overrides & original colors preserved ----- */
        body {
            background: #f7f9fc;
            font-family: 'Nunito', sans-serif;
        }
        .wrapper {
            background: #f7f9fc;
        }
        .card {
            border-radius: 14px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.03);
            border: 1px solid #e9edf4;
        }
        .card-stats {
            transition: transform 0.2s, box-shadow 0.2s;
            border-radius: 14px;
            background: white;
            border: none;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.03);
        }
        .card-stats:hover {
            transform: translateY(-3px);
            box-shadow: 0 8px 24px rgba(0,0,0,0.08);
        }
        .stat-value {
            font-size: 28px;
            font-weight: 800;
            line-height: 1.2;
            color: #1e3c72;
        }
        .stat-label {
            font-size: 13px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            color: #6c757d;
        }
        .kpi-card {
            border-left: 4px solid;
            border-radius: 14px;
        }
        .kpi-card.primary { border-left-color: #1572e8; }
        .kpi-card.success { border-left-color: #10b759; }
        .kpi-card.warning { border-left-color: #ffc107; }
        .kpi-card.info { border-left-color: #17a2b8; }
        .page-title {
            font-weight: 700;
            color: #1e3c72;
            border-left: 4px solid #1572e8;
            padding-left: 15px;
            margin-bottom: 25px;
        }
        .welcome-banner {
            background: #f0f4fc;
            border-radius: 16px;
            padding: 18px 24px;
            margin-bottom: 28px;
            border-left: 5px solid #1572e8;
        }
        .badge-status {
            font-size: 1rem;
            font-weight: 600;
            padding: 0.5rem 1rem;
            border-radius: 30px;
            min-width: 60px;
            display: inline-block;
            text-align: center;
        }
        .clickable-status {
            cursor: pointer;
            transition: all 0.15s ease;
        }
        .clickable-status:hover {
            opacity: 0.85;
            transform: scale(1.04);
            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
        }
        .financial-item {
            display: flex;
            justify-content: space-between;
            padding: 0.5rem 0;
            border-bottom: 1px solid #f0f2f5;
        }
        .financial-item:last-child { border-bottom: none; }
        .financial-item span:first-child { color: #5a6a7e; font-weight: 500; }
        .financial-item strong { color: #1e3c72; }

        /* ----- data table refinement (hierarchy + clean design) ----- */
        .table-wrapper {
            background: white;
            border-radius: 16px;
            padding: 0.5rem 0.25rem 0.25rem 0.25rem;
            box-shadow: 0 6px 18px rgba(0,0,0,0.03);
            border: 1px solid #eef2f7;
        }
        #siteTables {
            width: 100% !important;
            font-size: 0.9rem;
            border-collapse: separate;
            border-spacing: 0 6px;
        }
        #siteTables thead th {
            background: #f2f6fc;
            color: #1e3c72;
            font-weight: 700;
            font-size: 0.8rem;
            text-transform: uppercase;
            letter-spacing: 0.3px;
            padding: 14px 12px;
            border-bottom: 2px solid #dce3ed;
            border-top: none;
        }
        #siteTables tbody td {
            background: white;
            padding: 14px 12px;
            vertical-align: middle;
            border: none;
            border-bottom: 1px solid #eef2f7;
            color: #1f2a41;
        }
        #siteTables tbody tr:hover td {
            background: #fafcff;
        }
        #siteTables tbody tr:last-child td {
            border-bottom: none;
        }
        #siteTables .dataTables_wrapper .dataTables_filter input {
            border: 1px solid #dce3ed;
            border-radius: 40px;
            padding: 0.4rem 1rem 0.4rem 2rem;
            background: #f8faff;
            font-size: 0.85rem;
            outline: none;
        }
        #siteTables .dataTables_wrapper .dataTables_filter input:focus {
            border-color: #1572e8;
            box-shadow: 0 0 0 3px rgba(21,114,232,0.1);
        }
        #siteTables .dataTables_wrapper .dataTables_length select {
            border: 1px solid #dce3ed;
            border-radius: 30px;
            padding: 0.25rem 1.5rem 0.25rem 0.8rem;
            background: #f8faff;
        }
        #siteTables .dataTables_wrapper .dataTables_paginate .paginate_button {
            border-radius: 30px !important;
            border: none !important;
            background: transparent !important;
            color: #1e3c72 !important;
            font-weight: 600;
            margin: 0 2px;
        }
        #siteTables .dataTables_wrapper .dataTables_paginate .paginate_button.current {
            background: #1572e8 !important;
            color: white !important;
            border-radius: 30px !important;
            box-shadow: 0 2px 8px rgba(21,114,232,0.25);
        }
        #siteTables .dataTables_wrapper .dataTables_paginate .paginate_button:hover {
            background: #e9f0fa !important;
            color: #1572e8 !important;
            border-radius: 30px !important;
        }
        .dt-buttons .dt-button {
            background: #f2f6fc !important;
            border: 1px solid #dce3ed !important;
            border-radius: 30px !important;
            color: #1e3c72 !important;
            font-weight: 600;
            padding: 0.3rem 1.2rem !important;
            font-size: 0.8rem;
        }
        .dt-buttons .dt-button:hover {
            background: #e2ebf7 !important;
            border-color: #b8cade !important;
        }

        /* mobile */
        @media (max-width: 768px) {
            .stat-value { font-size: 22px; }
            .card-stats { margin-bottom: 15px; }
            .welcome-banner { padding: 14px 18px; }
            #siteTables thead th, #siteTables tbody td { padding: 10px 8px; font-size: 0.8rem; }
        }
        /* sidebar tweaks */
        .sidebar .nav-item a p { font-size: 14px; }
        .btn-mobile-menu { display: none; }
        @media (max-width: 768px) { .btn-mobile-menu { display: inline-block; } }
        .logo-header span { color: #1e3c72; }
    </style>

    <script type="text/javascript" language="javascript">
        function DisableBackButton() { window.history.forward() }
        DisableBackButton();
        window.onload = DisableBackButton;
        window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
        window.onunload = function () { void (0) }
        function toggleMobileSidebar() {
            if (window.innerWidth <= 768) {
                document.querySelector('.sidebar').classList.toggle('active');
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">
            <!-- header -->
            <div class="main-header">
                <div class="logo-header">
                    <span style="font-size: 22px; font-weight: 800; color: #1e3c72;">INNOBUILD</span>
                    <button class="navbar-toggler sidenav-toggler ml-auto" type="button" data-toggle="collapse" data-target="#collapse" aria-controls="sidebar" aria-expanded="false" aria-label="Toggle navigation">
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
                                    <span><asp:Label runat="server" ID="lblUser">CEO</asp:Label></span>
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

            <!-- sidebar (exactly as original) -->
            <div class="sidebar">
                <div class="scrollbar-inner sidebar-wrapper">
                    <ul class="nav">
                        <li class="nav-item active"><a href="Dashboard.aspx"><i class="la la-dashboard"></i><p>Dashboard</p></a></li>
                    </ul>
                    <div class="user"><div class="info"><a data-toggle="collapse" href="#sites" aria-expanded="true"><span><span class="user-level">SITES</span><span class="caret"></span></span></a><div class="collapse in" id="sites"><ul class="nav"><li><a href="ViewAllSites.aspx"><span class="link-collapse">Site Approval</span></a></li><li><a href="ViewSites.aspx"><span class="link-collapse">View Sites</span></a></li></ul></div></div></div>
                    <div class="user"><div class="info"><a data-toggle="collapse" href="#plots" aria-expanded="true"><span><span class="user-level">PLOTS</span><span class="caret"></span></span></a><div class="collapse in" id="plots"><ul class="nav"><li><a href="ViewAllPlots.aspx"><span class="link-collapse">Plot Details</span></a></li></ul></div></div></div>
                    <div class="user"><div class="info"><a data-toggle="collapse" href="#documents" aria-expanded="true"><span><span class="user-level">DOCUMENTS</span><span class="caret"></span></span></a><div class="collapse in" id="documents"><ul class="nav"><li><a href="OfferLetterDocuments.aspx"><span class="link-collapse">Offer Letters</span></a></li><li><a href="SaleAgreementDocument.aspx"><span class="link-collapse">Sales Agreements</span></a></li><li><a href="PlotHistory.aspx"><span class="link-collapse">Plot History</span></a></li><li><a href="ChangeofOwnership.aspx"><span class="link-collapse">Change of Ownership</span></a></li><li><a href="Vouchers.aspx"><span class="link-collapse">Payment Voucher</span></a><a href="VoucherCreditor.aspx"><span class="link-collapse">Creditor Voucher</span></a></li></ul></div></div></div>
                    <div class="user"><div class="info"><a data-toggle="collapse" href="#reports" aria-expanded="true"><span><span class="user-level">REPORTS</span><span class="caret"></span></span></a><div class="collapse in" id="reports"><ul class="nav"><li><a href="SalesReport.aspx"><span class="link-collapse">Sales Report</span></a></li><li><a href="SelectReceipt.aspx"><span class="link-collapse">Plot Payments</span></a></li><li><a href="SelectReceipt2.aspx"><span class="link-collapse">Ownership Payments</span></a></li><li><a href="ViewCustomers.aspx"><span class="link-collapse">View Customers</span></a></li></ul></div></div></div>
                    <div class="user"><div class="info"><a class="" data-toggle="collapse" href="#settings" aria-expanded="true"><span><span class="user-level">SETTINGS</span><span class="caret"></span></span></a><div class="clearfix"></div><div class="collapse in" id="settings"><ul class="nav"><li><a href="ChangePassword.aspx"><span class="link-collapse">Change Password</span></a><a href="../UserLogin.aspx"><span class="link-collapse">Logout</span></a></li></ul></div></div></div>
                </div>
            </div>

            <!-- main panel -->
            <div class="main-panel">
                <div class="content">
                    <div class="container-fluid">

                        <!-- welcome banner (hierarchy: first) -->
                        <div class="welcome-banner">
                            <div class="row align-items-center">
                                <div class="col-md-8">
                                    <h4 class="mb-1">Welcome back, <asp:Label runat="server" ID="lblCEOName" Text="CEO" Font-Bold="true" /></h4>
                                    <p class="text-muted mb-0" style="font-size:0.9rem;">Executive overview · all key metrics at a glance</p>
                                </div>
                            </div>
                        </div>

                        <!-- KPI ROW (hierarchy: primary metrics) -->
                        <div class="row">
                            <div class="col-md-3 col-sm-6"><div class="card card-stats kpi-card primary"><div class="card-body"><div class="row"><div class="col-5"><div class="icon-big text-center"><i class="la la-pie-chart" style="font-size:2.5rem;"></i></div></div><div class="col-7"><div class="numbers"><p class="stat-label">Sites</p><h4 class="stat-value"><asp:Label runat="server" ID="lblTotalSites">0</asp:Label></h4></div></div></div></div></div></div>
                            <div class="col-md-3 col-sm-6"><div class="card card-stats kpi-card success"><div class="card-body"><div class="row"><div class="col-5"><div class="icon-big text-center"><i class="la la-bar-chart" style="font-size:2.5rem;"></i></div></div><div class="col-7"><div class="numbers"><p class="stat-label">Plots</p><h4 class="stat-value"><asp:Label runat="server" ID="lblTotalPlots">0</asp:Label></h4></div></div></div></div></div></div>
                            <div class="col-md-3 col-sm-6"><div class="card card-stats kpi-card warning"><div class="card-body"><div class="row"><div class="col-5"><div class="icon-big text-center"><i class="la la-check-circle" style="font-size:2.5rem;"></i></div></div><div class="col-7"><div class="numbers"><p class="stat-label">Unallocated</p><h4 class="stat-value"><asp:Label runat="server" ID="lblAvailablePlots">0</asp:Label></h4></div></div></div></div></div></div>
                            <div class="col-md-3 col-sm-6"><div class="card card-stats kpi-card info"><div class="card-body"><div class="row"><div class="col-5"><div class="icon-big text-center"><i class="la la-users" style="font-size:2.5rem;"></i></div></div><div class="col-7"><div class="numbers"><p class="stat-label">Customers</p><h4 class="stat-value"><asp:Label runat="server" ID="lblTotalCustomers">0</asp:Label></h4></div></div></div></div></div></div>
                        </div>

                        <!-- SECONDARY ROW: Revenue + Plot status + Pending (hierarchy: secondary metrics) -->
                        <div class="row mt-3">
                            <!-- revenue card -->
                            <div class="col-md-4">
                                <div class="card">
                                    <div class="card-header"><h6 class="card-title mb-0 fw-bold">Revenue Overview</h6></div>
                                    <div class="card-body">
                                        <div class="financial-item"><span>Total Today</span><strong><asp:Label runat="server" ID="lblToday" Text="K0" /></strong></div>
                                        <div class="financial-item"><span>Total This Week</span><strong><asp:Label runat="server" ID="lblThisWeek" Text="K0" /></strong></div>
                                        <div class="financial-item"><span>Total This Month</span><strong><asp:Label runat="server" ID="lblTotalSales" Text="K0" /></strong></div>
                                        <div class="financial-item"><span>Due to be Collected</span><strong class="text-warning"><a href="Reminder.aspx" style="color:#b8860b;"><asp:Label runat="server" ID="lblPendingPayments" Text="K0" /></a></strong></div>
                                        <div class="financial-item"><span>Plot Stock Value</span><strong><asp:Label runat="server" ID="lblPlotStockValue" Text="0" /></strong></div>
                                    </div>
                                </div>
                            </div>

                            <!-- plot status card -->
                            <div class="col-md-4">
                                <div class="card shadow-sm border-0">
                                    <div class="card-header bg-white border-0"><h6 class="card-title mb-0 fw-bold">Plot Status</h6></div>
                                    <div class="card-body">
                                        <div class="d-flex justify-content-between align-items-center mb-3"><span class="text-muted">Vacant Inventory</span><span class="badge bg-primary fs-5 px-3 py-2 clickable-status" data-status="vacant" style="cursor:pointer; border-radius:40px;"><a href="Plots.aspx?status=available" style="color:white; text-decoration:none;"><asp:Label runat="server" ID="lblAvailable" Text="0" ForeColor="White" Font-Bold="true" /></a></span></div>
                                        <div class="d-flex justify-content-between align-items-center mb-3"><span class="text-muted">Reserved</span><span class="badge bg-success fs-5 px-3 py-2 clickable-status" data-status="reserved" style="cursor:pointer; border-radius:40px;"><a href="Plots.aspx?status=pending" style="color:white; text-decoration:none;"><asp:Label runat="server" ID="lblPending" Text="0" ForeColor="White" Font-Bold="true" /></a></span></div>
                                        <div class="d-flex justify-content-between align-items-center mb-3"><span class="text-muted">Allocated</span><span class="badge bg-warning fs-5 px-3 py-2 clickable-status" data-status="allocated" style="cursor:pointer; border-radius:40px;"><a href="Plots.aspx?status=allocated" style="color:white; text-decoration:none;"><asp:Label runat="server" ID="lblAllocated" Text="0" ForeColor="White" Font-Bold="true" /></a></span></div>
                                        <div class="d-flex justify-content-between align-items-center"><span class="text-muted">Completed</span><span class="badge bg-info fs-5 px-3 py-2 clickable-status" data-status="completed" style="cursor:pointer; border-radius:40px;"><a href="Plots.aspx?status=completed" style="color:white; text-decoration:none;"><asp:Label runat="server" ID="lblCompleted" Text="0" ForeColor="White" Font-Bold="true" /></a></span></div>
                                    </div>
                                </div>
                            </div>

                            <!-- pending approvals card -->
                            <div class="col-md-4">
                                <div class="card shadow-sm border-0">
                                    <div class="card-header bg-white border-0"><h6 class="card-title mb-0 fw-bold">Pending Approvals</h6></div>
                                    <div class="card-body">
                                        <div class="d-flex justify-content-between align-items-center mb-3"><span class="text-muted">Plot Withdrawals</span><span class="badge bg-info fs-5 px-3 py-2 clickable-status" data-status="withdrawal" style="cursor:pointer; border-radius:40px;"><a href="ApproveWithdraw.aspx" style="color:white; text-decoration:none;"><asp:Label runat="server" ID="lblWithdraws" Text="0" ForeColor="White" Font-Bold="true" /></a></span></div>
                                        <div class="d-flex justify-content-between align-items-center mb-3"><span class="text-muted">Voucher Approvals</span><span class="badge bg-success fs-5 px-3 py-2 clickable-status" data-status="pendingVoucher" style="cursor:pointer; border-radius:40px;"><a href="Vouchers.aspx" style="color:white; text-decoration:none;"><asp:Label runat="server" ID="lblPendingVouchers" Text="0" ForeColor="White" Font-Bold="true" /></a></span></div>
                                        <div class="d-flex justify-content-between align-items-center"><span class="text-muted">Creditor Approvals</span><span class="badge bg-primary fs-5 px-3 py-2 clickable-status" data-status="pendingCreditor" style="cursor:pointer; border-radius:40px;"><a href="VoucherCreditor.aspx" style="color:white; text-decoration:none;"><asp:Label runat="server" ID="lblPendingCreditor" Text="0" ForeColor="White" Font-Bold="true" /></a></span></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- DATA TABLE (hierarchy: detailed site table, refined design) -->
                        <div class="row mt-4">
                            <div class="col-12">
                                <div class="card">
                                    <div class="card-header bg-white d-flex align-items-center justify-content-between">
                                        <h6 class="card-title mb-0 fw-bold">Site Performance · Detailed View</h6>
                                        <span class="badge badge-light text-muted">updated in real-time</span>
                                    </div>
                                    <div class="card-body table-wrapper">
                                        <table id="siteTables" class="table table-striped table-striped-bg-default">
                                            <thead>
                                                <tr>
                                                    <th>Name</th>
                                                     <th>All Plots</th>
                                                    <th>Allocated</th>
                                                     
                                                    <th>Available</th>
                                                    <th>Site Size (sqm)</th>
                                                    <th>Plot Size (sqm)</th>
                                                    <th>Road Size (sqm)</th>
                                                    <th>Allocated (sqm)</th>
                                                    <th>Remaining (sqm)</th>
                                                </tr>
                                            </thead>
                                            <tbody id="SiteDetails" runat="server">
                                                <!-- dynamic rows from code-behind -->
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div> <!-- container-fluid -->
                </div> <!-- content -->
            </div> <!-- main-panel -->
        </div> <!-- wrapper -->

        <asp:Label runat="server" ID="lblSession" Visible="false"></asp:Label>
        <asp:Label runat="server" ID="lblLocation" Visible="false"></asp:Label>
    </form>

    <!-- scripts -->
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

    <!-- DataTables JS -->
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.21/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.2/js/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.print.min.js"></script>

    <script>
        // clickable status navigation (original behavior preserved)
        document.querySelectorAll('.clickable-status').forEach(function (el) {
            el.addEventListener('click', function (e) {
                e.preventDefault();
                var status = this.getAttribute('data-status');
                var map = {
                    'vacant': 'Plots.aspx?status=available',
                    'reserved': 'Plots.aspx?status=pending',
                    'allocated': 'Plots.aspx?status=allocated',
                    'completed': 'Plots.aspx?status=completed',
                    'withdrawal': 'ApproveWithdraw.aspx',
                    'pendingVoucher': 'Vouchers.aspx',
                    'pendingCreditor': 'VoucherCreditor.aspx'
                };
                if (map[status]) window.location.href = map[status];
            });
            el.addEventListener('mouseenter', function () { this.style.opacity = '0.85'; this.style.transform = 'scale(1.03)'; this.style.transition = 'all 0.15s'; });
            el.addEventListener('mouseleave', function () { this.style.opacity = '1'; this.style.transform = 'scale(1)'; });
        });

        // DataTable initialization (refined design)
        $(document).ready(function () {
            $('#siteTables').DataTable({
                dom: '<"d-flex justify-content-between align-items-center flex-wrap"Bf>t<"d-flex justify-content-between align-items-center flex-wrap"ip>',
                buttons: [
                    {
                        extend: 'print',
                        text: '<i class="la la-print"></i> Print',
                        className: 'btn btn-outline-secondary btn-sm rounded-pill px-3'
                    }
                ],
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                autoWidth: false,
                responsive: true,
                language: {
                    search: "Filter:",
                    lengthMenu: "Show _MENU_ entries",
                    info: "Showing _START_ to _END_ of _TOTAL_ sites",
                },
                columnDefs: [
                    { className: "align-middle", targets: "_all" }
                ]
            });
        });
    </script>
</body>
</html>