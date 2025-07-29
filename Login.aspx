<!-- Login.aspx updates -->
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CarRental.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login | Car Rental</title>
    <style>
        body {
            background: linear-gradient(135deg, #007bff, #00ccff);
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .login-container {
            background: white;
            width: 100%;
            max-width: 400px;
            padding: 40px;
            border-radius: 12px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
            text-align: center;
        }
        .title {
            font-size: 28px;
            font-weight: 600;
            color: #2d3e50;
            margin-bottom: 30px;
        }
        .form-group {
            margin-bottom: 20px;
            text-align: left;
        }
        .form-label {
            display: block;
            margin-bottom: 8px;
            color: #4a5568;
            font-size: 14px;
            font-weight: 500;
        }
        .form-control {
            width: 100%;
            padding: 12px 16px;
            border: 1px solid #e2e8f0;
            border-radius: 6px;
            font-size: 14px;
            transition: border-color 0.3s ease;
        }
        .form-control:focus {
            outline: none;
            border-color: #007bff;
            box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.1);
        }
        .login-button {
            background-color: #007bff;
            color: white;
            padding: 12px 24px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-weight: 600;
            transition: background-color 0.3s ease;
            width: 100%;
            margin-top: 10px;
        }
        .login-button:hover {
            background-color: #0056b3;
        }
        .message {
            padding: 12px;
            border-radius: 6px;
            margin-top: 20px;
            text-align: center;
            font-size: 14px;
        }
        .error-message {
            background-color: #fee2e2;
            color: #dc2626;
            border: 1px solid #fca5a5;
        }
        .register-link {
            margin-top: 20px;
            text-align: center;
            color: #4a5568;
        }
        .register-link a {
            color: #007bff;
            text-decoration: none;
            font-weight: 600;
            transition: color 0.3s ease;
        }
        .register-link a:hover {
            color: #0056b3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="title">Welcome to Car Rental</div>
            
            <div class="form-group">
                <asp:Label ID="lblUsername" runat="server" CssClass="form-label" Text="Username"></asp:Label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter your username"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" 
                    ErrorMessage="Username is required" Display="Dynamic" ForeColor="Red" />
            </div>
            
            <div class="form-group">
                <asp:Label ID="lblPassword" runat="server" CssClass="form-label" Text="Password"></asp:Label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter your password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" 
                    ErrorMessage="Password is required" Display="Dynamic" ForeColor="Red" />
            </div>
            
            <div class="form-group">
                <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="login-button" OnClick="btnLogin_Click" />
            </div>

         <div class="form-group" style="margin-top: 20px; text-align: center;">
    <asp:Button ID="btnRegister" runat="server" Text="Create New Account" CausesValidation="false" 
        OnClick="btnRegister_Click" 
        Style="background: none; border: none; color: #007bff; cursor: pointer; text-decoration: underline; font-weight: 600;" />
</div>
            <asp:Label ID="lblMessage" runat="server" CssClass="message error-message" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>