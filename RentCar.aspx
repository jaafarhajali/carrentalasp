<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RentCar.aspx.cs" Inherits="CarRentalApp.RentCar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rent a Car</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f5f5f5;
        }
        .header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
        }
        .nav-button {
            background-color: #4CAF50;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            text-decoration: none;
        }
        .nav-button:hover {
            background-color: #45a049;
        }
        .rental-container {
            display: flex;
            gap: 20px;
        }
        .grid-container {
            flex: 2;
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .details-container {
            flex: 1;
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .grid-view {
            width: 100%;
            margin-top: 20px;
            border-collapse: collapse;
        }
        .grid-view th, .grid-view td {
            padding: 12px;
            border: 1px solid #ddd;
            text-align: left;
        }
        .grid-view th {
            background-color: #f8f9fa;
            font-weight: 600;
        }
        .grid-view tr:nth-child(even) {
            background-color: #f8f9fa;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-label {
            display: block;
            margin-bottom: 5px;
            font-weight: 500;
        }
        .form-control {
            width: 100%;
            padding: 8px;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-sizing: border-box;
        }
        .vehicle-image {
            width: 100%;
            max-height: 200px;
            object-fit: cover;
            border-radius: 4px;
            margin-bottom: 15px;
        }
        .btn {
            width: 100%;
            padding: 10px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            background-color: #007bff;
            color: white;
            font-weight: 500;
        }
        .btn:hover {
            background-color: #0056b3;
        }
        .availability-date {
            color: #28a745;
            font-weight: 500;
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
        <div class="header">
            <h1>Rent a Car</h1>
            <asp:Button ID="btnGoToManagement" runat="server" Text="Go to Management" CssClass="nav-button" OnClick="btnGoToManagement_Click" />
        </div>

        <div class="rental-container">
            <div class="grid-container">
                <asp:GridView ID="gvVehicles" runat="server" 
                    AutoGenerateColumns="False" 
                    OnSelectedIndexChanged="gvVehicles_SelectedIndexChanged" 
                    DataKeyNames="VehicleId"
                    CssClass="grid-view">
                    <Columns>
                        <asp:BoundField DataField="VehicleName" HeaderText="Vehicle Name" />
                        <asp:BoundField DataField="VehicleType" HeaderText="Type" />
                        <asp:BoundField DataField="Price" HeaderText="Price/Day" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="Availability" HeaderText="Status" />
                        <asp:BoundField DataField="NextAvailableDate" HeaderText="Available From" DataFormatString="{0:d}" />
                        <asp:CommandField ShowSelectButton="True" ButtonType="Button" SelectText="Select" />
                    </Columns>
                </asp:GridView>
            </div>

            <div class="details-container">
                <asp:Image ID="imgVehicle" runat="server" CssClass="vehicle-image" />
                
                <div class="form-group">
                    <asp:Label ID="lblVehicleName" runat="server" CssClass="form-label" />
                </div>
                
                <div class="form-group">
                    <asp:Label ID="lblVehicleType" runat="server" CssClass="form-label" />
                </div>
                
                <div class="form-group">
                    <asp:Label ID="lblPrice" runat="server" CssClass="form-label" />
                </div>
                
                <div class="form-group">
                    <asp:Label ID="lblAvailability" runat="server" CssClass="form-label" />
                </div>
                
                <div class="form-group">
                    <asp:Label runat="server" CssClass="form-label" Text="Rental Date:" />
                    <asp:TextBox ID="txtRentalDate" runat="server" CssClass="form-control" TextMode="Date" AutoPostBack="true" OnTextChanged="CalculateTotalAmount" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRentalDate" 
                        ErrorMessage="Rental date is required" CssClass="error-message" Display="Dynamic" />
                </div>
                
                <div class="form-group">
                    <asp:Label runat="server" CssClass="form-label" Text="Return Date:" />
                    <asp:TextBox ID="txtReturnDate" runat="server" CssClass="form-control" TextMode="Date" AutoPostBack="true" OnTextChanged="CalculateTotalAmount" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtReturnDate" 
                        ErrorMessage="Return date is required" CssClass="error-message" Display="Dynamic" />
                </div>
                
                <div class="form-group">
                    <asp:Label ID="lblTotalAmount" runat="server" CssClass="form-label" />
                </div>
                
                <asp:Button ID="btnBook" runat="server" Text="Book Vehicle" CssClass="btn" OnClick="btnBook_Click" />
                
                <asp:Label ID="lblError" runat="server" CssClass="error-message" />
            </div>
        </div>
    </form>
</body>
</html>