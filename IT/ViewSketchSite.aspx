<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewSketchSite.aspx.cs" Inherits="ViewSite" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Innobuild - View Document</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <link rel="stylesheet" href="../assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" />
    <link rel="stylesheet" href="../assets/css/ready.css" />
    <link rel="stylesheet" href="../assets/css/demo.css" />

    <style>
        /* Document viewer styles */
        .document-container {
            background: #f8f9fc;
            border-radius: 12px;
            padding: 20px;
            min-height: 500px;
            display: flex;
            justify-content: center;
            align-items: center;
            border: 1px solid #e3e6f0;
            box-shadow: 0 0.15rem 1.75rem 0 rgba(58, 59, 69, 0.1);
        }

        .document-viewer {
            width: 100%;
            height: 70vh;
            border: none;
            border-radius: 8px;
            background: #fff;
        }

        .document-placeholder {
            text-align: center;
            padding: 60px 20px;
            color: #858796;
        }

        .document-placeholder i {
            font-size: 64px;
            margin-bottom: 20px;
            color: #4e73df;
        }

        .document-placeholder h4 {
            margin-bottom: 15px;
            color: #5a5c69;
        }

        .card-header-custom {
            background: linear-gradient(135deg, #4e73df 0%, #224abe 100%);
            color: white;
            padding: 1rem 1.5rem;
            border-radius: 0.35rem 0.35rem 0 0;
        }

        .card-header-custom h5 {
            margin: 0;
            font-weight: 600;
        }

        .info-bar {
            background: #f8f9fc;
            border-radius: 8px;
            padding: 12px 20px;
            margin-bottom: 20px;
            border-left: 4px solid #4e73df;
        }

        .info-bar span {
            font-weight: 600;
            color: #4e73df;
        }

        .btn-download {
            background: #4e73df;
            color: white;
            border-radius: 50px;
            padding: 8px 25px;
            transition: all 0.3s;
        }

        .btn-download:hover {
            background: #224abe;
            transform: translateY(-2px);
            color: white;
        }
    </style>

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
                            <li class="nav-item">
                                <a href="Dashboard.aspx">
                                    <i class="la la-dashboard"></i>
                                    <p>Dashboard</p>
                                </a>
                            </li>
                        </ul>
                        <!--Sites-->
                        <div class="user active">
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
                                                <span class="link-collapse">Sites Details</span>
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
                                            <a href="ViewPlots.aspx">
                                                <span class="link-collapse">Plot Details</span>
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
                      
                          
                                    <!-- Alert Messages -->
                                    <asp:Label runat="server" ID="lblError" Style="font-size: large; color: #FF3300; display: block; margin-bottom: 15px;"></asp:Label>
                                    <asp:Label runat="server" ID="lblSuccess" Style="font-weight: 700; color: #009900; display: block; margin-bottom: 15px;"></asp:Label>

                                   
                                            <asp:Label runat="server" ID="lblCheck">Checking here</asp:Label>

                                            <!-- Document Viewer -->
                                            <div class="document-container">
                                                <asp:Panel ID="pnlDocumentViewer" runat="server" Width="100%">
                                                    <div class="document-placeholder">
                                                        <i class="la la-file-text-o"></i>
                                                        <h4>No Document Available</h4>
                                                        <p class="text-muted">The requested document could not be found or has not been uploaded yet.</p>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </div>
                          

                    <!-- Hidden Labels -->
                    <asp:Label runat="server" ID="lblSession" Visible="false"></asp:Label>
                    <asp:Label runat="server" ID="lblLocation" Visible="false"></asp:Label>
                    <asp:Label runat="server" ID="lblUser" Visible="false"></asp:Label>
                    <asp:Label runat="server" ID="lblPlotSketch" Visible="false"></asp:Label>

                    <!-- Success Modal -->
                    <div class="modal fade" id="successModal" tabindex="-1" role="dialog" aria-labelledby="successLabel" aria-hidden="true">
                        <div class="modal-dialog modal-dialog-centered" role="document">
                            <div class="modal-content">
                                <div class="modal-header border-0 d-flex flex-column align-items-center">
                                    <h4 class="modal-title mt-2" style="font-size: 24px; color: steelblue;" id="successLabel"></h4>
                                    <br />
                                    <hr />
                                    <i class="la la-check-circle" style="font-size: 60px; color: #2170b9;"></i>
                                </div>
                                <div class="modal-body text-center" style="font-size: 16px; color: #555;">
                                    Operation Completed Successfully!
                                </div>
                                <div class="modal-footer border-0 d-flex justify-content-center">
                                    <button type="button" class="btn btn-block btn-primary" data-dismiss="modal">Close</button>
                                </div>
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