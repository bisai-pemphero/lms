<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Customer Portal | Innobuild</title>

    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <link rel="stylesheet" href="../assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../assets/css/ready.css" />
    <link rel="stylesheet" href="../assets/css/demo.css" />
    <link href="https://fonts.googleapis.com/css?family=Nunito:300,400,600,700,800" rel="stylesheet" />

    <style>

        body {
            background: #f4f7fc;
            font-family: 'Nunito', sans-serif;
        }

        .welcome-banner {
            background: linear-gradient(135deg,#2563eb,#1d4ed8);
            color: white;
            border-radius: 20px;
            padding: 30px;
            margin-bottom: 25px;
        }

        .customer-card {
            border: none;
            border-radius: 18px;
            background: #fff;
            box-shadow: 0 8px 24px rgba(0,0,0,.05);
            transition: .3s;
        }

        .customer-card:hover {
            transform: translateY(-4px);
        }

        .metric-value {
            font-size: 30px;
            font-weight: 800;
        }

        .metric-title {
            font-size: 12px;
            text-transform: uppercase;
            color: #6b7280;
            font-weight: 700;
        }

        .metric-icon {
            width: 55px;
            height: 55px;
            border-radius: 14px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 24px;
        }

        .icon-blue { background:#e0ecff; color:#2563eb; }
        .icon-green { background:#dcfce7; color:#16a34a; }
        .icon-orange { background:#fff7ed; color:#f59e0b; }
        .icon-purple { background:#f3e8ff; color:#8b5cf6; }

        .quick-action {
            padding:15px;
            border-radius:14px;
            text-align:center;
            color:#fff;
            cursor:pointer;
            transition:.3s;
        }

        .quick-action:hover {
            transform: translateY(-3px);
        }

        .action1 { background:linear-gradient(135deg,#16a34a,#15803d); }
        .action2 { background:linear-gradient(135deg,#2563eb,#1d4ed8); }
        .action3 { background:linear-gradient(135deg,#f59e0b,#d97706); }
        .action4 { background:linear-gradient(135deg,#8b5cf6,#7c3aed); }

        .bottom-nav {
            display:none;
            position:fixed;
            bottom:0;
            left:0;
            right:0;
            background:#fff;
            box-shadow:0 -2px 10px rgba(0,0,0,.1);
            justify-content:space-around;
            padding:10px 0;
        }

        .bottom-nav a {
            text-align:center;
            font-size:12px;
            color:#555;
        }

        @media(max-width:768px){
            .bottom-nav{display:flex;}
        }

    </style>

</head>

<body>

<form id="form1" runat="server">

<div class="wrapper">

    <!-- HEADER -->
    <div class="main-header">
        <div class="logo-header">
            <span style="font-weight:800;font-size:20px;">INNOBUILD</span>
        </div>

        <nav class="navbar navbar-header navbar-expand-lg">
            <div class="container-fluid">
                <ul class="navbar-nav ml-auto">
                    <li class="nav-item dropdown">
                        <a class="dropdown-toggle profile-pic" data-toggle="dropdown">
                            <img src="../assets/img/user.png" width="40" />
                            <asp:Label ID="lblCustomerName" runat="server" Text="Customer" />
                        </a>
                    </li>
                </ul>
            </div>
        </nav>
    </div>

    <!-- SIDEBAR -->
    <div class="sidebar">
        <div class="scrollbar-inner sidebar-wrapper">
            <ul class="nav">
                <li class="nav-item active"><a href="#"><i class="la la-home"></i><p>Dashboard</p></a></li>
                <li class="nav-item"><a href="MyPlots.aspx"><i class="la la-map"></i><p>My Plot</p></a></li>
                <li class="nav-item"><a href="MyPayments.aspx"><i class="la la-money"></i><p>Payments</p></a></li>
                <li class="nav-item"><a href="Documents.aspx"><i class="la la-file"></i><p>Documents</p></a></li>
                <li class="nav-item"><a href="Support.aspx"><i class="la la-phone"></i><p>Support</p></a></li>
            </ul>
        </div>
    </div>

    <!-- MAIN -->
    <div class="main-panel">
        <div class="content">
        <div class="container-fluid">

            <!-- WELCOME -->
            <div class="welcome-banner">
                <h3>Welcome Back 👋 <asp:Label ID="lblCustomer" runat="server" /></h3>
                <p>Manage your plot, payments, and documents easily.</p>
            </div>

            <!-- CARDS -->
            <div class="row">

                <div class="col-md-3">
                    <div class="card customer-card p-3">
                        <div class="d-flex justify-content-between">
                            <div>
                                <div class="metric-title">My Plot</div>
                                <div class="metric-value">
                                    <asp:Label ID="lblPlot" runat="server" Text="1" />
                                </div>
                            </div>
                            <div class="metric-icon icon-blue">📍</div>
                        </div>
                    </div>
                </div>

                <div class="col-md-3">
                    <div class="card customer-card p-3">
                        <div class="d-flex justify-content-between">
                            <div>
                                <div class="metric-title">Paid</div>
                                <div class="metric-value">
                                    <asp:Label ID="lblPaid" runat="server" />
                                </div>
                            </div>
                            <div class="metric-icon icon-green">💰</div>
                        </div>
                    </div>
                </div>

                <div class="col-md-3">
                    <div class="card customer-card p-3">
                        <div class="d-flex justify-content-between">
                            <div>
                                <div class="metric-title">Balance</div>
                                <div class="metric-value">
                                    <asp:Label ID="lblBalance" runat="server" />
                                </div>
                            </div>
                            <div class="metric-icon icon-orange">⚠</div>
                        </div>
                    </div>
                </div>

                <div class="col-md-3">
                    <div class="card customer-card p-3">
                        <div class="d-flex justify-content-between">
                            <div>
                                <div class="metric-title">Status</div>
                                <div class="metric-value">
                                    <asp:Label ID="lblStatus" runat="server" />
                                </div>
                            </div>
                            <div class="metric-icon icon-purple">✔</div>
                        </div>
                    </div>
                </div>

            </div>

            <!-- PAYMENT PROGRESS -->
            <div class="row mt-4">
                <div class="col-md-8">
                    <div class="card customer-card p-3">
                        <h5>Payment Progress</h5>

                        <div class="progress" style="height:20px;">
                            <div class="progress-bar bg-success" style="width:75%;">75%</div>
                        </div>

                        <p class="mt-2">You have completed 75% of your payments.</p>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="card customer-card p-3">
                        <h5>Next Payment</h5>
                        <h3 class="text-success">
                            <asp:Label ID="lblNext" runat="server" />
                        </h3>
                        <asp:Button ID="btnPay" runat="server" Text="Pay Now" CssClass="btn btn-success btn-block" />
                    </div>
                </div>
            </div>

            <!-- PLOT INFO -->
            <div class="row mt-4">
                <div class="col-md-6">
                    <div class="card customer-card p-3">
                        <h5>My Plot</h5>

                        <p>Site: <asp:Label ID="lblSite" runat="server" /></p>
                        <p>Plot No: <asp:Label ID="lblPlotNo" runat="server" /></p>
                        <p>Size: <asp:Label ID="lblSize" runat="server" /></p>
                        <p>Price: <asp:Label ID="lblPrice" runat="server" /></p>

                    </div>
                </div>

                <div class="col-md-6">
                    <div class="card customer-card p-3">
                        <h5>Documents</h5>

                        <a class="btn btn-light btn-block">Offer Letter</a>
                        <a class="btn btn-light btn-block">Agreement</a>
                        <a class="btn btn-light btn-block">Receipts</a>

                    </div>
                </div>
            </div>

            <!-- FOOTER SPACE -->
            <div style="height:80px;"></div>

        </div>
        </div>
    </div>

</div>

<!-- MOBILE NAV -->
<div class="bottom-nav">
    <a href="#">🏠<br/>Home</a>
    <a href="MyPlots.aspx">📍<br/>Plot</a>
    <a href="MyPayments.aspx">💰<br/>Pay</a>
    <a href="Documents.aspx">📄<br/>Docs</a>
    <a href="Support.aspx">☎<br/>Help</a>
</div>

</form>

</body>
</html>