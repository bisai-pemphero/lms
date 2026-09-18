<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalesReport.aspx.cs" Inherits="ViewSite" %>

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

    <!--For the tables-->
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.21/css/jquery.dataTables.min.css" />

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

                    </div>
                </div>
                <!--Content-->

                <div class="main-panel">
                    <div class="content">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="card card-stats card-warning">
                                        <div class="card-body ">
                                            <div class="row">
                                                <div class="col-5">
                                                    <div class="icon-big text-center">
                                                        <i class="la la-money card-icon"></i>
                                                    </div>
                                                </div>
                                                <div class="col-7 d-flex align-items-center">
                                                    <div class="numbers">
                                                        <p class="card-category">Expected Income</p>
                                                        <h4 class="card-title">
                                                            <asp:Label runat="server" ID="lblExpected"></asp:Label>
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
                                                        <i class="la la-credit-card card-ico"></i>
                                                    </div>
                                                </div>
                                                <div class="col-7 d-flex align-items-center">
                                                    <div class="numbers">
                                                        <p class="card-category">Collected</p>
                                                        <h4 class="card-title">
                                                            <asp:Label runat="server" ID="lblCollected"></asp:Label>
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
                                                        <i class="la la-balance-scale card-icon"></i>
                                                    </div>
                                                </div>
                                                <div class="col-7 d-flex align-items-center">
                                                    <div class="numbers">
                                                        <p class="card-category">Balance</p>
                                                        <h4 class="card-title">
                                                            <asp:Label runat="server" ID="lblBalance"></asp:Label>
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
                                                        <i class="la la-exclamation-triangle card-icon"></i>
                                                    </div>
                                                </div>
                                                <div class="col-7 d-flex align-items-center">
                                                    <div class="numbers">
                                                        <p class="card-category">Overdue Amount</p>
                                                        <h4 class="card-title">
                                                            <asp:Label runat="server" ID="lblOverdueAmount"></asp:Label>
                                                        </h4>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Cards end here-->
                            <div class="row">

                                <div class="col-md-12">

                                    <div class="card">
                                        <div class="card-body">

                                            <div class="col-md-12 row">


                                                <div class="col-md-4">
                                                    Select District
          <div class="form-group">
              <asp:DropDownList ID="drpLocation" runat="server" CssClass="form-control input-solid">
              </asp:DropDownList>
          </div>
                                                </div>
                                                <div class="col-md-4">
                                                    Start Date
          <div class="form-group">
              <asp:TextBox runat="server" ID="txtStartDate" CssClass="form-control input-solid" TextMode="Date"></asp:TextBox>
          </div>
                                                </div>
                                                <div class="col-md-4">
                                                    End Date
          <div class="form-group">
              <asp:TextBox runat="server" ID="txtEndDate" CssClass="form-control input-solid" TextMode="Date" AutoPostBack="true" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
          </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                    <!--Sales Report Table-->
                                    <div class="card">
                                        <div class="card-header">
                                            <div class="card-title">SALES REPORT</div>
                                        </div>
                                        <div class="card-body">
                                            <table id="salesReportTable" class="table table-striped table-striped-bg-default mt-3">
                                                <thead>
                                                    <tr>
                                                        <th>CUSTOMER</th>
                                                        <th>SITE NAME</th>
                                                        <th>PLOT NO</th>
                                                        <th>PAID</th>
                                                        <th>BALANCE</th>
                                                        <th>MODE OF PAYMENT</th>
                                                        <th>DATE OF PAYMENT</th>
                                                    </tr>
                                                </thead>
                                                <tbody id="salesReportPlaceholder" runat="server">
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>


                                    <!--Overdue plots Table-->
                                    <div class="card">
                                        <div class="card-header">
                                            <div class="card-title">OVERDUE PLOTS</div>
                                        </div>
                                        <div class="card-body">
                                            <table id="OverDuePlotTable" class="table table-striped table-striped-bg-default mt-3">
                                                <thead>
                                                    <tr>
                                                        <th>SITE NAME</th>
                                                        <th>PLOT NO</th>
                                                        <th>CUSTOMER</th>
                                                        <th>DATE OFFERED</th>
                                                        <th>PERIOD (MONTHS)</th>
                                                        <th>PAID</th>
                                                        <th>OVERDUE AMOUNT</th>
                                                        <th>DUE DATE</th>
                                                        <th>MONTHS OVERDUE</th>
                                                    </tr>
                                                </thead>
                                                <tbody id="overduePlotsPlaceholder" runat="server">
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>




                                </div>
                                <asp:Label runat="server" ID="lblSession" Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lblLocation" Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lblUser" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--Delete Modal-->
        <div class="modal fade" id="ErrorModal" tabindex="-1" role="dialog" aria-labelledby="ErrorLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <!-- Header with Icon -->
                    <div class="modal-header border-0 d-flex flex-column align-items-center">

                        <h4 class="modal-title mt-2" style="font-size: 24px; color: steelblue;" id="ErrorLabel"></h4>
                        <br />
                        <hr />
                        <i class="la la-exclamation-circle" style="font-size: 60px; color: #2170b9;"></i>
                    </div>
                    <!-- Body -->
                    <div class="modal-body text-center" style="font-size: 16px; color: #555;">
                        Please select all required fields!
                    </div>
                    <!-- Footer -->
                    <div class="modal-footer border-0 d-flex justify-content-center">
                        <button type="button" class="btn btn-block btn-primary" data-dismiss="modal">Close</button>
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

<!--For the tables-->

<script src="https://cdn.datatables.net/1.10.21/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.6.2/js/dataTables.buttons.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.print.min.js"></script>
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://cdn.datatables.net/1.10.21/js/jquery.dataTables.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.6.2/js/dataTables.buttons.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.html5.min.js"></script>
<script src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.print.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.36/pdfmake.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.36/vfs_fonts.js"></script>

<script>
    $(document).ready(function () {
        var currentDate = new Date().toISOString().slice(0, 10);
        var table = $('#salesReportTable').DataTable({
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'csvHtml5',
                    text: 'Export to Excel',
                    className: 'btn btn-outline-primary',
                    title: 'Sales Report as of ' + currentDate,
                    exportOptions: {
                        modifier: {
                            search: 'applied',
                            order: 'applied',
                            page: 'all'
                        }
                    }
                },
                {
                    extend: 'pdfHtml5',
                    text: 'Export to PDF',
                    className: 'btn btn-outline-success',
                    title: 'Sales Report as of ' + currentDate,
                    exportOptions: {
                        modifier: {
                            search: 'applied',
                            order: 'applied',
                            page: 'all'
                        }
                    }
                },
                {
                    extend: 'print',
                    text: 'Print Table',
                    className: 'btn btn-outline-warning',
                    title: 'Sales Report as of ' + currentDate,
                    exportOptions: {
                        modifier: {
                            search: 'applied',
                            order: 'applied',
                            page: 'all'
                        }
                    }
                }
            ],
            "paging": true,
            "lengthChange": true,
            "searching": true,
            "ordering": true,
            "info": true,
            "autoWidth": false,
            "responsive": true
        });
    });
</script>


<script>
    $(document).ready(function () {
        var currentDate = new Date().toISOString().slice(0, 10);
        var table = $('#OverDuePlotTable').DataTable({
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'csvHtml5',
                    text: 'Export to Excel',
                    className: 'btn btn-outline-primary',
                    title: 'Overdue Plots as of ' + currentDate,
                    exportOptions: {
                        modifier: {
                            search: 'applied',
                            order: 'applied',
                            page: 'all'
                        }
                    }
                },
                {
                    extend: 'pdfHtml5',
                    text: 'Export to PDF',
                    className: 'btn btn-outline-success',
                    title: 'Overdue Plots as of ' + currentDate,
                    exportOptions: {
                        modifier: {
                            search: 'applied',
                            order: 'applied',
                            page: 'all'
                        }
                    }
                },
                {
                    extend: 'print',
                    text: 'Print Table',
                    className: 'btn btn-outline-warning',
                    title: 'Overdue Plots as of ' + currentDate,
                    exportOptions: {
                        modifier: {
                            search: 'applied',
                            order: 'applied',
                            page: 'all'
                        }
                    }
                }
            ],
            "paging": true,
            "lengthChange": true,
            "searching": true,
            "ordering": true,
            "info": true,
            "autoWidth": false,
            "responsive": true
        });
    });
</script>
</html>
