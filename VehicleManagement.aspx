<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VehicleManagement.aspx.cs" Inherits="CarRental.VehicleManagement" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vehicle Management</title>
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
        .management-container {
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
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
        .btn-group {
            margin: 15px 0;
            display: flex;
            gap: 10px;
        }
        .btn {
            padding: 8px 16px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }
        .btn-primary {
            background-color: #007bff;
            color: white;
        }
        .btn-success {
            background-color: #28a745;
            color: white;
        }
        .btn-secondary {
            background-color: #6c757d;
            color: white;
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
        .vehicle-image {
            max-width: 200px;
            max-height: 150px;
            margin: 10px 0;
            border: 1px solid #ddd;
            border-radius: 4px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <h1>Vehicle Management</h1>
            <asp:Button ID="btnGoToRentCar" runat="server" Text="Go to Rent Car" CssClass="nav-button" OnClick="btnGoToRentCar_Click" />
        </div>

        <div class="management-container">
            <div class="form-group">
                <asp:Label ID="lblVehicleName" runat="server" CssClass="form-label" Text="Vehicle Name:"></asp:Label>
                <asp:TextBox ID="txtVehicleName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvVehicleName" runat="server" 
                    ControlToValidate="txtVehicleName" 
                    ErrorMessage="Vehicle name is required" 
                    Display="Dynamic" ForeColor="Red" />
            </div>

            <div class="form-group">
                <asp:Label ID="lblVehicleType" runat="server" CssClass="form-label" Text="Vehicle Type:"></asp:Label>
                <asp:TextBox ID="txtVehicleType" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvVehicleType" runat="server" 
                    ControlToValidate="txtVehicleType" 
                    ErrorMessage="Vehicle type is required" 
                    Display="Dynamic" ForeColor="Red" />
            </div>

            <div class="form-group">
                <asp:Label ID="lblPrice" runat="server" CssClass="form-label" Text="Price per Day:"></asp:Label>
                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPrice" runat="server" 
                    ControlToValidate="txtPrice" 
                    ErrorMessage="Price is required" 
                    Display="Dynamic" ForeColor="Red" />
                <asp:RangeValidator ID="rvPrice" runat="server" 
                    ControlToValidate="txtPrice" 
                    MinimumValue="0" 
                    MaximumValue="10000" 
                    Type="Double" 
                    ErrorMessage="Price must be between 0 and 10000" 
                    Display="Dynamic" ForeColor="Red" />
            </div>

            <div class="form-group">
                <asp:Image ID="imgVehicle" runat="server" CssClass="vehicle-image" />
                <br />
                <asp:FileUpload ID="fileUpload" runat="server" />
            </div>

            <div class="btn-group">
                <asp:Button ID="btnAddVehicle" runat="server" Text="Add Vehicle" 
                    CssClass="btn btn-primary" OnClick="btnAddVehicle_Click" />
                <asp:Button ID="btnUpdateVehicle" runat="server" Text="Update Vehicle" 
                    CssClass="btn btn-success" OnClick="btnUpdateVehicle_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear Form" 
                    CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" />
            </div>

            <asp:GridView ID="gvVehicles" runat="server" 
                AutoGenerateColumns="False"
                DataKeyNames="VehicleId"
                OnSelectedIndexChanged="gvVehicles_SelectedIndexChanged"
                CssClass="grid-view">
                <Columns>
                    <asp:BoundField DataField="VehicleId" HeaderText="ID" />
                    <asp:BoundField DataField="VehicleName" HeaderText="Name" />
                    <asp:BoundField DataField="VehicleType" HeaderText="Type" />
                    <asp:BoundField DataField="Price" HeaderText="Price/Day" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="Availability" HeaderText="Status" />
                    <asp:CommandField ShowSelectButton="True" SelectText="Select" ButtonType="Button" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>