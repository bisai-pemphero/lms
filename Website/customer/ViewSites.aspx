<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewSites.aspx.cs" Inherits="ViewSites" %>

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
                                            <asp:Label runat="server" ID="lblUser">Admin</asp:Label>
                                        </span></span> </a>
                                    <ul class="dropdown-menu dropdown-user">
                                        <a class="dropdown-item" href="../UserLogin.aspx"><i class="la la-power-off"></i>Logout</a>
                                        <a class="dropdown-item" href="ChangePassword.aspx"><i class="la la-gears"></i>Change Password</a>
                                    </ul>
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
         </div>
                    </div>
             
                <!--Content-->

                <div class="main-panel">
                    <div class="content">
                        <div class="container-fluid">

                            <div class="row">

                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-header">
                                            <div class="card-title">SITES</div>
                                        </div>
                                        <div class="card-body">

                                            <table id="siteTable" class="table table-striped table-striped-bg-default mt-3">
                                                <thead>
                                                    <tr>
                                                        <th>DISTRICT</th>
                                                        <th>SITE NAME</th>
                                                        <th>SIZE</th>
                                                        <th>PREVIOUS OWNER</th>
                                                        <th>INITIAL VALUE</th>
                                                        <th>PAID</th>
                                                        <th>BALANCE</th>
                                                        <th>TOTAL PLOTS</th>
                                                        <th>ESTIMATE RETURN</th>
                                                    </tr>
                                                </thead>
                                                <% LoadSiteDetails(); %>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <asp:Label runat="server" ID="lblSession" Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lblLocation" Visible="false"></asp:Label>

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
        var table = $('#siteTable').DataTable({
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'csvHtml5',
                    text: 'Export to Excel',
                    className: 'btn btn-outline-primary',
                    title: 'All Sites as of ' + currentDate,
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
                    title: 'All Sites as of ' + currentDate,
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
                    title: 'All Sites as of ' + currentDate,
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
