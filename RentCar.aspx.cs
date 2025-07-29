using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarRentalApp
{
    public partial class RentCar : System.Web.UI.Page
    {
        private readonly string connectionString = @"Server=JAAFARHAJALI\MSSQLSERVER2;Database=CarRentalDB;Integrated Security=True;";

        private int? SelectedVehicleId
        {
            get { return (int?)ViewState["SelectedVehicleId"]; }
            set { ViewState["SelectedVehicleId"] = value; }
        }

        private decimal SelectedVehiclePrice
        {
            get { return ViewState["SelectedVehiclePrice"] != null ? (decimal)ViewState["SelectedVehiclePrice"] : 0M; }
            set { ViewState["SelectedVehiclePrice"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadVehicleData();
                SetMinimumDates();
                InitializeControls();
            }
        }

        private void InitializeControls()
        {
            btnBook.Enabled = false;
            lblError.Text = string.Empty;
            lblTotalAmount.Text = "Total Amount: $0.00";
        }

        private void SetMinimumDates()
        {
            txtRentalDate.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
            txtReturnDate.Attributes["min"] = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
        }

        private void LoadVehicleData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        v.VehicleId, 
                        v.VehicleName, 
                        v.VehicleType, 
                        v.Price, 
                        v.Availability,
                        CASE 
                            WHEN v.Availability = 'Available' THEN GETDATE()
                            ELSE (
                                SELECT TOP 1 ReturnDate 
                                FROM Rentals 
                                WHERE VehicleId = v.VehicleId 
                                    AND Status = 'Active' 
                                    AND ReturnDate >= GETDATE()
                                ORDER BY ReturnDate DESC
                            )
                        END AS NextAvailableDate,
                        (SELECT COUNT(*) 
                         FROM Rentals 
                         WHERE VehicleId = v.VehicleId 
                            AND Status = 'Active' 
                            AND ReturnDate >= GETDATE()) as ActiveRentals
                    FROM Vehicles v
                    ORDER BY 
                        CASE WHEN v.Availability = 'Available' THEN 0 ELSE 1 END,
                        NextAvailableDate";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        DataTable dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());
                        gvVehicles.DataSource = dt;
                        gvVehicles.DataBind();

                        foreach (GridViewRow row in gvVehicles.Rows)
                        {
                            string status = row.Cells[3].Text;
                            if (status == "Rented")
                            {
                                row.Cells[3].ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                row.Cells[3].ForeColor = System.Drawing.Color.Green;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage($"Error loading vehicles: {ex.Message}");
                    }
                }
            }
        }

        protected void gvVehicles_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvVehicles.SelectedRow;
            if (row != null)
            {
                try
                {
                    SelectedVehicleId = Convert.ToInt32(gvVehicles.DataKeys[row.RowIndex].Value);
                    SelectedVehiclePrice = decimal.Parse(row.Cells[2].Text.Replace("$", "").Replace(",", ""));

                    lblVehicleName.Text = $"Vehicle Name: {row.Cells[0].Text}";
                    lblVehicleType.Text = $"Vehicle Type: {row.Cells[1].Text}";
                    lblPrice.Text = $"Price per Day: {row.Cells[2].Text}";
                    lblAvailability.Text = $"Status: {row.Cells[3].Text}";

                    DateTime nextAvailable;
                    if (DateTime.TryParse(row.Cells[4].Text, out nextAvailable))
                    {
                        if (row.Cells[3].Text == "Rented")
                        {
                            lblAvailability.Text += $" (Available from {nextAvailable:d})";
                            txtRentalDate.Attributes["min"] = nextAvailable.ToString("yyyy-MM-dd");
                            txtReturnDate.Attributes["min"] = nextAvailable.AddDays(1).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            txtRentalDate.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
                            txtReturnDate.Attributes["min"] = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
                        }
                    }

                    LoadVehicleImage(SelectedVehicleId.Value);
                    btnBook.Enabled = true;
                    CalculateTotalAmount(null, null);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Error loading vehicle details: {ex.Message}");
                }
            }
        }

        private void LoadVehicleImage(int vehicleId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Image FROM Vehicles WHERE VehicleId = @VehicleId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@VehicleId", vehicleId);
                    try
                    {
                        conn.Open();
                        byte[] imageData = cmd.ExecuteScalar() as byte[];

                        if (imageData != null && imageData.Length > 0)
                        {
                            string base64String = Convert.ToBase64String(imageData);
                            imgVehicle.ImageUrl = $"data:image/jpeg;base64,{base64String}";
                        }
                        else
                        {
                            imgVehicle.ImageUrl = "~/Images/no-image-available.png";
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage($"Error loading vehicle image: {ex.Message}");
                    }
                }
            }
        }

        protected void CalculateTotalAmount(object sender, EventArgs e)
        {
            if (SelectedVehicleId.HasValue &&
                DateTime.TryParse(txtRentalDate.Text, out DateTime rentalDate) &&
                DateTime.TryParse(txtReturnDate.Text, out DateTime returnDate))
            {
                if (rentalDate > returnDate)
                {
                    ShowErrorMessage("Return date must be after rental date");
                    lblTotalAmount.Text = "Total Amount: $0.00";
                    return;
                }

                int days = (returnDate - rentalDate).Days + 1;
                decimal totalAmount = SelectedVehiclePrice * days;
                lblTotalAmount.Text = $"Total Amount: {totalAmount:C}";
                lblError.Text = string.Empty;
            }
            else
            {
                lblTotalAmount.Text = "Total Amount: $0.00";
            }
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            if (!ValidateBooking(out DateTime rentalDate, out DateTime returnDate))
            {
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Check availability
                        bool isAvailable = CheckVehicleAvailability(conn, transaction, rentalDate, returnDate);
                        if (!isAvailable)
                        {
                            throw new Exception("Vehicle is not available for the selected dates.");
                        }

                        // Calculate total amount
                        int days = (returnDate - rentalDate).Days + 1;
                        decimal totalAmount = SelectedVehiclePrice * days;

                        // Insert rental record
                        int newRentalId = CreateRentalRecord(conn, transaction, rentalDate, returnDate, totalAmount);

                        // Update vehicle availability
                        UpdateVehicleAvailability(conn, transaction);

                        transaction.Commit();

                        // Redirect to payment page
                        Response.Redirect($"Payment.aspx?rentalId={newRentalId}", true);
                    }
                    catch (Exception ex)
                    {
                        if (transaction.Connection != null)
                        {
                            transaction.Rollback();
                        }
                        ShowErrorMessage($"Error booking vehicle: {ex.Message}");
                    }
                }
            }
        }

        private bool ValidateBooking(out DateTime rentalDate, out DateTime returnDate)
        {
            rentalDate = DateTime.MinValue;
            returnDate = DateTime.MinValue;

            if (!SelectedVehicleId.HasValue)
            {
                ShowErrorMessage("Please select a vehicle first.");
                return false;
            }

            if (!DateTime.TryParse(txtRentalDate.Text, out rentalDate) ||
                !DateTime.TryParse(txtReturnDate.Text, out returnDate))
            {
                ShowErrorMessage("Please enter valid dates.");
                return false;
            }

            if (rentalDate > returnDate)
            {
                ShowErrorMessage("Return date must be after rental date.");
                return false;
            }

            if (rentalDate.Date < DateTime.Today)
            {
                ShowErrorMessage("Rental date cannot be in the past.");
                return false;
            }

            return true;
        }

        private bool CheckVehicleAvailability(SqlConnection conn, SqlTransaction transaction, DateTime rentalDate, DateTime returnDate)
        {
            using (SqlCommand checkCmd = new SqlCommand(@"
                SELECT COUNT(*) 
                FROM Rentals 
                WHERE VehicleId = @VehicleId 
                    AND Status = 'Active'
                    AND (@RentalDate BETWEEN RentalDate AND ReturnDate
                        OR @ReturnDate BETWEEN RentalDate AND ReturnDate
                        OR RentalDate BETWEEN @RentalDate AND @ReturnDate)", conn, transaction))
            {
                checkCmd.Parameters.AddWithValue("@VehicleId", SelectedVehicleId.Value);
                checkCmd.Parameters.AddWithValue("@RentalDate", rentalDate);
                checkCmd.Parameters.AddWithValue("@ReturnDate", returnDate);

                int conflictingBookings = (int)checkCmd.ExecuteScalar();
                return conflictingBookings == 0;
            }
        }

        private int CreateRentalRecord(SqlConnection conn, SqlTransaction transaction, DateTime rentalDate, DateTime returnDate, decimal totalAmount)
        {
            using (SqlCommand insertCmd = new SqlCommand(@"
                INSERT INTO Rentals (VehicleId, RentalDate, ReturnDate, TotalAmount, Status, PaymentStatus)
                VALUES (@VehicleId, @RentalDate, @ReturnDate, @TotalAmount, 'Active', 'Pending');
                SELECT SCOPE_IDENTITY();", conn, transaction))
            {
                insertCmd.Parameters.AddWithValue("@VehicleId", SelectedVehicleId.Value);
                insertCmd.Parameters.AddWithValue("@RentalDate", rentalDate);
                insertCmd.Parameters.AddWithValue("@ReturnDate", returnDate);
                insertCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);

                return Convert.ToInt32(insertCmd.ExecuteScalar());
            }
        }

        private void UpdateVehicleAvailability(SqlConnection conn, SqlTransaction transaction)
        {
            using (SqlCommand updateCmd = new SqlCommand(@"
                UPDATE Vehicles 
                SET Availability = 'Rented'
                WHERE VehicleId = @VehicleId", conn, transaction))
            {
                updateCmd.Parameters.AddWithValue("@VehicleId", SelectedVehicleId.Value);
                updateCmd.ExecuteNonQuery();
            }
        }

        protected void btnGoToManagement_Click(object sender, EventArgs e)
        {
            if (Session["UserRole"]?.ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase) == true)
            {
                Response.Redirect("VehicleManagement.aspx");
            }
            else
            {
                ShowErrorMessage("Access denied. Admin privileges required.");
            }
        }

        private void ClearForm()
        {
            SelectedVehicleId = null;
            SelectedVehiclePrice = 0;
            lblVehicleName.Text = string.Empty;
            lblVehicleType.Text = string.Empty;
            lblPrice.Text = string.Empty;
            lblAvailability.Text = string.Empty;
            lblTotalAmount.Text = "Total Amount: $0.00";
            txtRentalDate.Text = string.Empty;
            txtReturnDate.Text = string.Empty;
            imgVehicle.ImageUrl = string.Empty;
            btnBook.Enabled = false;
            gvVehicles.SelectedIndex = -1;
            lblError.Text = string.Empty;
            SetMinimumDates();
        }

        private void ShowErrorMessage(string message)
        {
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = message;
        }

        private void ShowSuccessMessage(string message)
        {
            lblError.ForeColor = System.Drawing.Color.Green;
            lblError.Text = message;
        }
    }
}