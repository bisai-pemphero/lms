<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard1.aspx.cs" Inherits="Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
   <title>Innobuild</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <link rel="stylesheet" href="/assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" />
    <link rel="stylesheet" href="/assets/css/ready.css" />
    <link rel="stylesheet" href="/assets/css/demo.css" />
    
    <!-- Chart.js for data visualization -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    
    <!-- Leaflet for maps -->
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.7.1/dist/leaflet.css" />
    <script src="https://unpkg.com/leaflet@1.7.1/dist/leaflet.js"></script>

    <style>
        .chart-container {
            position: relative;
            height: 300px;
            margin-bottom: 20px;
        }
        .dashboard-filter {
            background-color: #f8f9fa;
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 20px;
        }
        .filter-label {
            font-weight: 600;
            margin-bottom: 5px;
        }
        #map {
            height: 400px;
            width: 100%;
            border-radius: 5px;
            margin-bottom: 20px;
        }
        .plot-marker {
            border-radius: 50%;
            text-align: center;
            font-weight: bold;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .plot-available {
            background-color: green;
            color: white;
        }
        .plot-sold {
            background-color: red;
            color: white;
        }
        .plot-reserved {
            background-color: orange;
            color: white;
        }
        .info-window {
            padding: 10px;
            min-width: 200px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function DisableBackButton() {
            window.history.forward()
        }
        DisableBackButton();
        window.onload = function () {
            DisableBackButton();
            initCharts();
            initFilters();
        };
        window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
        window.onunload = function () { void (0) }

        // Initialize charts with real data
        function initCharts() {
            // Use AJAX to fetch data from server
            fetchChartData().then(data => {
                initPlotStatusChart(data.statusData);
                initSalesTrendChart(data.trendData);
                initPriceByZoneChart(data.zoneData);
            }).catch(error => {
                console.error('Error loading chart data:', error);
                // Initialize with sample data as fallback
                initPlotStatusChart();
                initSalesTrendChart();
                initPriceByZoneChart();
            });
        }

        // Fetch chart data from server
        async function fetchChartData() {
            try {
                const response = await fetch('Dashboard.aspx/GetChartData', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });

                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }

                const data = await response.json();
                return data.d; // The actual data is usually in the d property
            } catch (error) {
                console.error('Error fetching chart data:', error);
                throw error;
            }
        }

        // Update chart functions to accept data parameters
        function initPlotStatusChart(data) {
            // If no data provided, use sample data
            if (!data) {
                data = {
                    labels: ['Available', 'Sold', 'Under Offer', 'Reserved', 'Under Legal Review'],
                    values: [120, 85, 30, 45, 20]
                };
            }

            const ctx = document.getElementById('plotStatusChart').getContext('2d');
            const chart = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: data.labels,
                    datasets: [{
                        data: data.values,
                        backgroundColor: [
                            '#4caf50', '#f44336', '#ff9800', '#2196f3', '#9c27b0'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: { position: 'right' },
                        title: { display: true, text: 'Plot Status Distribution' }
                    }
                }
            });
        }

        function initSalesTrendChart(data) {
            // If no data provided, use sample data
            if (!data) {
                data = {
                    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                    plotsSold: [12, 19, 8, 15, 10, 13, 17, 11, 9, 14, 16, 20],
                    revenue: [240, 380, 160, 300, 200, 260, 340, 220, 180, 280, 320, 400]
                };
            }

            const ctx = document.getElementById('salesTrendChart').getContext('2d');
            const chart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: data.labels,
                    datasets: [
                        {
                            label: 'Plots Sold',
                            data: data.plotsSold,
                            backgroundColor: 'rgba(54, 162, 235, 0.5)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 1,
                            yAxisID: 'y'
                        },
                        {
                            label: 'Revenue (K)',
                            data: data.revenue,
                            type: 'line',
                            fill: false,
                            borderColor: 'rgba(255, 99, 132, 1)',
                            backgroundColor: 'rgba(255, 99, 132, 0.5)',
                            yAxisID: 'y1'
                        }
                    ]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            type: 'linear',
                            display: true,
                            position: 'left',
                            title: {
                                display: true,
                                text: 'Plots Sold'
                            }
                        },
                        y1: {
                            type: 'linear',
                            display: true,
                            position: 'right',
                            title: {
                                display: true,
                                text: 'Revenue (K)'
                            },
                            grid: {
                                drawOnChartArea: false
                            }
                        }
                    },
                    plugins: {
                        title: {
                            display: true,
                            text: 'Monthly Sales & Revenue Trends'
                        }
                    }
                }
            });
        }

        function initPriceByZoneChart(data) {
            // If no data provided, use sample data
            if (!data) {
                data = {
                    labels: ['North Zone', 'South Zone', 'East Zone', 'West Zone', 'Central Zone'],
                    values: [150, 120, 135, 180, 210]
                };
            }

            const ctx = document.getElementById('priceByZoneChart').getContext('2d');
            const chart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: data.labels,
                    datasets: [{
                        label: 'Average Price per Sq Ft ($)',
                        data: data.values,
                        backgroundColor: 'rgba(75, 192, 192, 0.6)',
                        borderColor: 'rgba(75, 192, 192, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: 'Price per Sq Ft ($)'
                            }
                        }
                    },
                    plugins: {
                        title: {
                            display: true,
                            text: 'Average Price by Zone'
                        }
                    }
                }
            });
        }

        // Initialize filters
        function initFilters() {
            // Date range filter initialization would go here
            // This is a simplified version

            document.getElementById('applyFilters').addEventListener('click', function () {
                const statusFilter = document.getElementById('statusFilter').value;
                const zoneFilter = document.getElementById('zoneFilter').value;
                const dateFilter = document.getElementById('dateFilter').value;

                // In a real implementation, this would trigger a data refresh
                alert(`Filters applied: Status=${statusFilter}, Zone=${zoneFilter}, Date Range=${dateFilter}`);
                // You would then reload the charts with filtered data
            });

            document.getElementById('resetFilters').addEventListener('click', function () {
                document.getElementById('statusFilter').value = 'all';
                document.getElementById('zoneFilter').value = 'all';
                document.getElementById('dateFilter').value = 'all';

                // In a real implementation, this would reset all charts to show all data
                alert('Filters reset');
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

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
                                        <asp:Label runat="server" ID="lblUser">Admin</asp:Label>
                                    </span></span> </a>
                                <ul class="dropdown-menu dropdown-user">
                                    <a class="dropdown-item" href="/UserLogin.aspx"><i class="la la-power-off"></i>Logout</a>
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
                                            <span class="link-collapse">Customer Payments</span>
                                        </a>
                                    </li>
                                    <li>
                                        <a href="SelectReceipt2.aspx">
                                            <span class="link-collapse">Change of Ownership Payments</span>
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
                    <!--System Users-->
                    <div class="user">
                        <div class="info">
                            <a class="" data-toggle="collapse" href="#systemUsers" aria-expanded="true">
                                <span>
                                    <span class="user-level">USERS</span>
                                    <span class="caret"></span>
                                </span>
                            </a>
                            <div class="clearfix"></div>
                            <div class="collapse in" id="systemUsers" aria-expanded="true" style="">
                                <ul class="nav">
                                    <li>
                                        <a href="SystemUsers.aspx">
                                            <span class="link-collapse">System Users</span>
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
                        <h4 class="page-title">Dashboard</h4>
                        
                        <!-- Dashboard Filters -->
                        <div class="row dashboard-filter">
                            <div class="col-md-12">
                                <h5>Filter Data</h5>
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="filter-label">Status</label>
                                            <select id="statusFilter" class="form-control">
                                                <option value="all">All Statuses</option>
                                                <option value="Pending">Pending</option>
                                                <option value="Allocated">Allocated</option>
                                                <option value="Completed">Completed</option>
                                              
                                            </select>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="filter-label">Zone</label>
                                            <select id="zoneFilter" class="form-control">
                                                <option value="all">All Zones</option>
                                                <option value="Lilongwe">Lilongwe</option>
                                                <option value="Blantyre">Blantyre</option>
                                                <option value="Mzuzu">Mzuzu</option>
                                                <option value="Zomba">Zomba</option>
                                                <option value="Kasungu">Kasungu</option>
                                                <option value="Salima">Salima</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label class="filter-label">Date Range</label>
                                            <select id="dateFilter" class="form-control">
                                                <option value="all">All Time</option>
                                                <option value="month">This Month</option>
                                                <option value="quarter">This Quarter</option>
                                                <option value="year">This Year</option>
                                                <option value="custom">Custom Range</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="col-md-3 d-flex align-items-end">
                                        <button id="applyFilters" class="btn btn-primary mr-2">Apply Filters</button>
                                        <button id="resetFilters" class="btn btn-secondary">Reset</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <!-- KPI Summary Cards -->
                        <div class="row">
                            <div class="col-md-3">
                                <div class="card card-stats card-warning">
                                    <div class="card-body ">
                                        <div class="row">
                                            <div class="col-5">
                                                <div class="icon-big text-center">
                                                    <i class="la la-pie-chart"></i>
                                                </div>
                                            </div>
                                            <div class="col-7 d-flex align-items-center">
                                                <div class="numbers">
                                                    <p class="card-category">Total Sites</p>
                                                    <h4 class="card-title">
                                                        <asp:Label runat="server" ID="lblTotalSites"></asp:Label>
                                                    </h4>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="card card-stats card-success">
                                    <div class="card-body ">
                                        <div class="row">
                                            <div class="col-5">
                                                <div class="icon-big text-center">
                                                    <i class="la la-bar-chart"></i>
                                                </div>
                                            </div>
                                            <div class="col-7 d-flex align-items-center">
                                                <div class="numbers">
                                                    <p class="card-category">Total Plots</p>
                                                    <h4 class="card-title">
                                                        <asp:Label runat="server" ID="lblTotalPlots"></asp:Label>
                                                    </h4>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="card card-stats card-primary">
                                    <div class="card-body ">
                                        <div class="row">
                                            <div class="col-5">
                                                <div class="icon-big text-center">
                                                    <i class="la la-check-circle"></i>
                                                </div>
                                            </div>
                                            <div class="col-7 d-flex align-items-center">
                                                <div class="numbers">
                                                    <p class="card-category">Available Plots</p>
                                                    <h4 class="card-title">
                                                        <asp:Label runat="server" ID="lblAvailablePlots"></asp:Label>
                                                    </h4>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="card card-stats card-danger">
                                    <div class="card-body ">
                                        <div class="row">
                                            <div class="col-5">
                                                <div class="icon-big text-center">
                                                    <i class="la la-money"></i>
                                                </div>
                                            </div>
                                            <div class="col-7 d-flex align-items-center">
                                                <div class="numbers">
                                                    <p class="card-category">Total Revenue</p>
                                                    <h4 class="card-title">$2.5M</h4>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
       
                        <!-- Data Visualizations -->
                        <div class="row">
                            <!-- Plot Status Distribution -->
                            <div class="col-md-6">
                                <div class="card">
                                    <div class="card-header">
                                        <h4 class="card-title">Plot Status Distribution</h4>
                                    </div>
                                    <div class="card-body">
                                        <div class="chart-container">
                                            <canvas id="plotStatusChart"></canvas>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <!-- Sales & Revenue Trend -->
                            <div class="col-md-6">
                                <div class="card">
                                    <div class="card-header">
                                        <h4 class="card-title">Sales & Revenue Trends</h4>
                                    </div>
                                    <div class="card-body">
                                        <div class="chart-container">
                                            <canvas id="salesTrendChart"></canvas>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row">
                            <!-- Average Price by Zone -->
                            <div class="col-md-12">
                                <div class="card">
                                    <div class="card-header">
                                        <h4 class="card-title">Average Price by Zone</h4>
                                    </div>
                                    <div class="card-body">
                                        <div class="chart-container">
                                            <canvas id="priceByZoneChart"></canvas>
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

<script src="/assets/js/core/jquery.3.2.1.min.js"></script>
<script src="/assets/js/plugin/jquery-ui-1.12.1.custom/jquery-ui.min.js"></script>
<script src="/assets/js/core/popper.min.js"></script>
<script src="/assets/js/core/bootstrap.min.js"></script>
<script src="/assets/js/plugin/chartist/chartist.min.js"></script>
<script src="/assets/js/plugin/chartist/plugin/chartist-plugin-tooltip.min.js"></script>

<script src="/assets/js/plugin/bootstrap-toggle/bootstrap-toggle.min.js"></script>
<script src="/assets/js/plugin/jquery-mapael/jquery.mapael.min.js"></script>
<script src="/assets/js/plugin/jquery-mapael/maps/world_countries.min.js"></script>
<script src="/assets/js/plugin/chart-circle/circles.min.js"></script>
<script src="/assets/js/plugin/jquery-scrollbar/jquery.scrollbar.min.js"></script>
<script src="/assets/js/ready.min.js"></script>
<script src="/assets/js/demo.js"></script>
</html>