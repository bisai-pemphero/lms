<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" EnableEventValidation="false" Inherits="UserLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="icon" type="image/png" href="assets/img/favicon.png" />
    <title>Login</title>
    <link rel="stylesheet" href="styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="login-header">
                <img src="assets/img/Chingonga1.png" alt="Land Management Logo" class="logo"/>
                <h1>Ching'onga Real Estate</h1>
                <asp:Label runat="server" ID="lblGreeting"></asp:Label>
                <br />
                <br />
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            </div>
            <form class="login-form">
                <div class="form-group input">
                    <div>
                        <asp:TextBox CssClass="text" placeholder="Email" ID="txtUsername" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <asp:TextBox class="form-control" placeholder="Password" ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn-block login-button" OnClick="btnLogin_Click"></asp:Button>
               
            </form>
            <div class="signup-link">
                <a href="#">Forgot Password?</a>
            </div>
        </div>
    </form>
</body>
</html>
