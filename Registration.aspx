<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="CarRental.Registration" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register | Car Rental</title>
    <style>
        body {
            background: linear-gradient(135deg, #007bff, #00ccff);
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
        }
        .registration-container {
            background: white;
            width: 100%;
            max-width: 400px;
            padding: 40px;
            border-radius: 12px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
            text-align: center;
            position: relative;
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
            position: relative;
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
            box-sizing: border-box;
        }
        .form-control:focus {
            outline: none;
            border-color: #007bff;
            box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.1);
        }
        .register-button {
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
        .register-button:hover {
            background-color: #0056b3;
        }
        .clear-button {
            background: none;
            color: #4a5568;
            padding: 12px 24px;
            border: 1px solid #e2e8f0;
            border-radius: 6px;
            cursor: pointer;
            font-weight: 600;
            transition: all 0.3s ease;
            width: 100%;
            margin-top: 10px;
        }
        .clear-button:hover {
            background-color: #f8f9fa;
        }
        .back-to-login {
            margin-top: 20px;
            text-align: center;
            color: #4a5568;
        }
        .back-to-login a {
            color: #007bff;
            text-decoration: none;
            font-weight: 600;
            transition: color 0.3s ease;
        }
        .back-to-login a:hover {
            color: #0056b3;
        }
        .checkbox-group {
            margin: 15px 0;
            display: flex;
            align-items: center;
        }
        .checkbox-group input {
            margin-right: 8px;
        }
        .message-container {
            position: relative;
            margin-top: 20px;
            min-height: 40px;
        }
        .message {
            padding: 12px;
            border-radius: 6px;
            text-align: center;
            font-size: 14px;
            width: 100%;
            box-sizing: border-box;
            position: absolute;
            top: 0;
            left: 0;
        }
        .error-message {
            background-color: #fee2e2;
            color: #dc2626;
            border: 1px solid #fca5a5;
            animation: fadeIn 0.3s ease-in-out;
        }
        @keyframes fadeIn {
            from {
                opacity: 0;
                transform: translateY(-10px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }
        .validation-error {
            color: #dc2626;
            font-size: 12px;
            margin-top: 4px;
            display: block;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="registration-container">
            <div class="title">Create Account</div>
            
            <div class="form-group">
                <asp:Label ID="lblUsername" runat="server" CssClass="form-label" Text="Username"></asp:Label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter your username"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                    ControlToValidate="txtUsername" 
                    ErrorMessage="Username is required" 
                    Display="Dynamic" 
                    CssClass="validation-error" />
            </div>
            
            <div class="form-group">
                <asp:Label ID="lblPassword" runat="server" CssClass="form-label" Text="Password"></asp:Label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter your password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                    ControlToValidate="txtPassword" 
                    ErrorMessage="Password is required" 
                    Display="Dynamic" 
                    CssClass="validation-error" />
            </div>
            
            <div class="form-group">
                <asp:Label ID="lblConfirmPassword" runat="server" CssClass="form-label" Text="Confirm Password"></asp:Label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirm your password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                    ControlToValidate="txtConfirmPassword" 
                    ErrorMessage="Please confirm your password" 
                    Display="Dynamic" 
                    CssClass="validation-error" />
                <asp:CompareValidator ID="cvPassword" runat="server" 
                    ControlToValidate="txtConfirmPassword" 
                    ControlToCompare="txtPassword" 
                    ErrorMessage="Passwords do not match" 
                    Display="Dynamic" 
                    CssClass="validation-error" />
            </div>
            
            <div class="checkbox-group">
                <asp:CheckBox ID="chkShowPassword" runat="server" Text="Show Password" AutoPostBack="true" 
                    OnCheckedChanged="chkShowPassword_CheckedChanged" />
            </div>
            
            <div class="form-group">
                <asp:Button ID="btnRegister" runat="server" Text="Create Account" CssClass="register-button" 
                    OnClick="btnRegister_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear Form" CssClass="clear-button" 
                    OnClick="btnClear_Click" CausesValidation="false" />
            </div>
            
            <div class="back-to-login">
                <asp:LinkButton ID="lnkBackToLogin" runat="server" 
                    Text="Already have an account? Sign In" 
                    OnClick="lnkBackToLogin_Click" 
                    CausesValidation="false">
                </asp:LinkButton>
            </div>

            <div class="message-container">
                <asp:Label ID="lblMessage" runat="server" CssClass="message error-message" EnableViewState="false" Visible="false"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>