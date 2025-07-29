<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="CarRentalApp.Payment" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f5f5f5;
        }
        .payment-container {
            max-width: 600px;
            margin: 0 auto;
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .header {
            text-align: center;
            margin-bottom: 30px;
        }
        .form-group {
            margin-bottom: 20px;
        }
        .form-label {
            display: block;
            margin-bottom: 5px;
            font-weight: 500;
        }
        .form-control {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-sizing: border-box;
        }
        .payment-summary {
            background-color: #f8f9fa;
            padding: 15px;
            border-radius: 4px;
            margin-bottom: 20px;
        }
        .btn {
            width: 100%;
            padding: 12px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-weight: 500;
            font-size: 16px;
        }
        .btn-primary {
            background-color: #007bff;
            color: white;
        }
        .btn-primary:hover {
            background-color: #0056b3;
        }
        .error-message {
            color: #dc3545;
            margin-top: 5px;
            font-size: 0.9em;
        }
        .success-message {
            color: #28a745;
            margin-top: 5px;
            font-size: 0.9em;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="payment-container">
            <div class="header">
                <h1>Payment Details</h1>
            </div>

            <div class="payment-summary">
                <h3>Rental Summary</h3>
                <asp:Label ID="lblVehicleName" runat="server" CssClass="form-label" />
                <asp:Label ID="lblRentalDates" runat="server" CssClass="form-label" />
                <asp:Label ID="lblTotalAmount" runat="server" CssClass="form-label" />
            </div>

            <div class="form-group">
                <asp:Label runat="server" CssClass="form-label" Text="Card Holder Name:" />
                <asp:TextBox ID="txtCardHolder" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCardHolder" 
                    ErrorMessage="Card holder name is required" CssClass="error-message" Display="Dynamic" />
            </div>

            <div class="form-group">
                <asp:Label runat="server" CssClass="form-label" Text="Card Number:" />
                <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" MaxLength="16" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCardNumber" 
                    ErrorMessage="Card number is required" CssClass="error-message" Display="Dynamic" />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCardNumber"
                    ValidationExpression="^\d{16}$" ErrorMessage="Please enter a valid 16-digit card number"
                    CssClass="error-message" Display="Dynamic" />
            </div>

            <div class="form-group">
                <asp:Label runat="server" CssClass="form-label" Text="Expiry Date:" />
                <div style="display: flex; gap: 10px;">
                    <asp:DropDownList ID="ddlExpiryMonth" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Month" Value="" />
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlExpiryYear" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Year" Value="" />
                    </asp:DropDownList>
                </div>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlExpiryMonth" 
                    ErrorMessage="Expiry month is required" CssClass="error-message" Display="Dynamic" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlExpiryYear" 
                    ErrorMessage="Expiry year is required" CssClass="error-message" Display="Dynamic" />
            </div>

            <div class="form-group">
                <asp:Label runat="server" CssClass="form-label" Text="CVV:" />
                <asp:TextBox ID="txtCVV" runat="server" CssClass="form-control" MaxLength="3" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCVV" 
                    ErrorMessage="CVV is required" CssClass="error-message" Display="Dynamic" />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCVV"
                    ValidationExpression="^\d{3}$" ErrorMessage="Please enter a valid 3-digit CVV"
                    CssClass="error-message" Display="Dynamic" />
            </div>

            <asp:Button ID="btnProcessPayment" runat="server" Text="Process Payment" 
                CssClass="btn btn-primary" OnClick="btnProcessPayment_Click" />

            <asp:Label ID="lblMessage" runat="server" />
        </div>
    </form>
</body>
</html>