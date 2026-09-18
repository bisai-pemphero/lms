<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeFile="UserLogin.aspx.cs" EnableEventValidation="false" Inherits="UserLogin" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="icon" type="image/png" href="assets/img/favicon.png" />
    <title>Innobuild</title>
    <link rel="stylesheet" href="styles.css" />
    
   <style>
    /* Modern Mobile-First UI Styles */
    :root {
        --bg-color: #f4f6f9;
        --card-bg: #ffffff;
        --text-color: #333333;
        --input-border: #cccccc;
    }

    /* Force the HTML, Body, and ASP.NET Form to fill 100% height and flex-center */
    html, body, #form1 {
        height: 100% !important;
        min-height: 100vh !important;
        margin: 0 !important;
        padding: 0 !important;
        background-color: var(--bg-color);
    }

    body, #form1 {
        display: flex !important;
        justify-content: center !important;
        align-items: center !important;
    }

    .login-container {
        background-color: var(--card-bg);
        padding: 2.5rem 2rem;
        border-radius: 12px;
        box-shadow: 0 8px 24px rgba(0, 0, 0, 0.08);
        width: 100%;
        max-width: 400px;
        box-sizing: border-box;
        margin: auto !important; /* Forces layout systems to push it to the exact middle */
        transition: all 0.3s ease;
    }

    .login-header {
        text-align: center;
        margin-bottom: 2rem;
    }

    .login-header .logo {
        max-width: 120px;
        height: auto;
        margin-bottom: 1rem;
    }

    .login-header h1 {
        font-size: 1.5rem;
        color: var(--text-color);
        margin: 0 0 0.5rem 0;
        font-weight: 600;
    }

    .error-message {
        display: block;
        margin-top: 0.75rem;
        font-size: 0.9rem;
    }

    .form-group {
        margin-bottom: 1.25rem;
    }

    .form-control-custom {
        width: 100%;
        padding: 12px 16px;
        font-size: 1rem;
        border: 1px solid var(--input-border);
        border-radius: 6px;
        background-color: #fafafa;
        box-sizing: border-box;
        transition: border-color 0.2s, background-color 0.2s;
    }

    .form-control-custom:focus {
        background-color: #ffffff;
        outline: none;
        box-shadow: 0 0 0 3px rgba(0, 86, 179, 0.15);
    }

    .login-button-custom {
        width: 100%;
        padding: 12px;
        font-size: 1rem;
        font-weight: 600;
        border: none;
        border-radius: 6px;
        cursor: pointer;
        transition: background-color 0.2s, transform 0.1s;
        margin-top: 0.5rem;
    }

    .login-button-custom:active {
        transform: scale(0.98);
    }

    @media (max-width: 360px) {
        .login-container {
            padding: 1.5rem 1.25rem;
        }
    }
</style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            
            <div class="login-header">
                <img src="assets/img/Innobuild logo small.png" alt="Land Management Logo" class="logo"/>
               
                <asp:Label runat="server" ID="lblGreeting" ViewStateMode="Disabled"></asp:Label>
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="true" CssClass="error-message"></asp:Label>
            </div>

            <div class="form-group">
                <asp:TextBox CssClass="form-control-custom" placeholder="Email" ID="txtUsername" runat="server" TextMode="Email"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <asp:TextBox CssClass="form-control-custom" placeholder="Password" ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            </div>

            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="login-button-custom btn-block login-button" OnClick="btnLogin_Click"></asp:Button>
               
        </div>
    </form>
</body>
</html>