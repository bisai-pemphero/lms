<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SystemUsers.aspx.cs" Inherits="Users" %>

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
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.21/css/jquery.dataTables.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/buttons/1.6.2/css/buttons.dataTables.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <style>
        .error-container {
            text-align: center;
            padding: 40px 20px;
            background: #fff;
            border-radius: 10px;
            box-shadow: 0 0 20px rgba(0,0,0,0.1);
            margin: 40px auto;
            max-width: 700px;
        }
        .error-code {
            font-size: 120px;
            font-weight: 900;
            color: #2c3e50;
            line-height: 1;
            margin-bottom: 20px;
            text-shadow: 4px 4px 0 #f8f9fa;
        }
        .error-message {
            font-size: 24px;
            color: #7f8c8d;
            margin-bottom: 30px;
        }
        .error-description {
            font-size: 16px;
            color: #95a5a6;
            margin-bottom: 30px;
            line-height: 1.6;
        }
        .error-actions {
            margin-top: 30px;
        }
        .btn-error {
            background: #2c3e50;
            color: white;
            padding: 12px 30px;
            border-radius: 30px;
            font-weight: 600;
            margin: 0 10px;
            text-decoration: none;
            display: inline-block;
            transition: all 0.3s ease;
        }
        .btn-error:hover {
            background: #3498db;
            color: white;
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        .error-icon {
            font-size: 60px;
            color: #e74c3c;
            margin-bottom: 20px;
        }
        @media (max-width: 768px) {
            .error-code {
                font-size: 80px;
            }
            .error-message {
                font-size: 20px;
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
                                    </span></a>
                                <ul class="dropdown-menu dropdown-user">
                                    <a class="dropdown-item" href="/UserLogin.aspx"><i class="la la-power-off"></i>Logout</a>
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
                        <li class="nav-item">
                            <a href="Dashboard.aspx">
                                <i class="la la-dashboard"></i>
                                <p>Dashboard</p>
                            </a>
                        </li>
                    </ul>
                    
                  
                   
                </div>
            </div>

            <div class="main-panel">
                <div class="content">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-12">
                                <div class="error-container">
                                    <div class="error-icon">⚠️</div>
                                    <div class="error-code">404</div>
                                    <div class="error-message">Page Not Found</div>
                                    <div class="error-description">
                                        The page you are looking for might have been removed, had its name changed, 
                                        or is temporarily unavailable.
                                    </div>
                                    <div class="error-actions">
                                        <a href="Dashboard.aspx" class="btn-error">
                                            <i class="la la-home"></i> Go to Dashboard
                                        </a>
                                       
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

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
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.5.1.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.21/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.2/js/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.print.min.js"></script>
</body>
</html>
