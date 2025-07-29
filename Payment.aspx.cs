using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace CarRentalApp
{
    public partial class Payment : System.Web.UI.Page
    {
        private readonly string connectionString = @"Server=JAAFARHAJALI\MSSQLSERVER2;Database=CarRentalDB;Integrated Security=True;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["rentalId"] == null)
                {
                    Response.Redirect("RentCar.aspx");
                    return;
                }

                LoadRentalDetails();
                PopulateExpiryDropdowns();
            }
        }

        private void PopulateExpiryDropdowns()
        {
            // Populate months
            for (int i = 1; i <= 12; i++)
            {
                ddlExpiryMonth.Items.Add(new System.Web.UI.WebControls.ListItem(
                    i.ToString("00"), i.ToString()));
            }

            // Populate years (current year + 10 years)
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear; i <= currentYear + 10; i++)
            {
                ddlExpiryYear.Items.Add(new System.Web.UI.WebControls.ListItem(
                    i.ToString(), i.ToString()));
            }
        }

        private void LoadRentalDetails()
        {
            int rentalId = Convert.ToInt32(Request.QueryString["rentalId"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT r.RentalId, r.RentalDate, r.ReturnDate, r.TotalAmount,
                           v.VehicleName
                    FROM Rentals r
                    JOIN Vehicles v ON r.VehicleId = v.VehicleId
                    WHERE r.RentalId = @RentalId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RentalId", rentalId);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblVehicleName.Text = $"Vehicle: {reader["VehicleName"]}";
                                lblRentalDates.Text = $"Rental Period: {Convert.ToDateTime(reader["RentalDate"]).ToShortDateString()} - {Convert.ToDateTime(reader["ReturnDate"]).ToShortDateString()}";
                                lblTotalAmount.Text = $"Total Amount: {Convert.ToDecimal(reader["TotalAmount"]):C}";
                            }
                            else
                            {
                                Response.Redirect("RentCar.aspx");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage($"Error loading rental details: {ex.Message}");
                    }
                }
            }
        }

        protected void btnProcessPayment_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int rentalId = Convert.ToInt32(Request.QueryString["rentalId"]);

            // In a real application, you would integrate with a payment gateway here
            // For demo purposes, we'll just update the rental status
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    UPDATE Rentals 
                    SET PaymentStatus = 'Paid',
                        PaymentDate = GETDATE()
                    WHERE RentalId = @RentalId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RentalId", rentalId);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        ShowSuccessMessage("Payment processed successfully!");
                        // Redirect to a confirmation page or back to the rental page after a delay
                        ScriptManager.RegisterStartupScript(this, GetType(),
                            "RedirectScript",
                            "setTimeout(function(){ window.location.href = 'RentCar.aspx'; }, 2000);",
                            true);
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage($"Error processing payment: {ex.Message}");
                    }
                }
            }
        }

        private void ShowErrorMessage(string message)
        {
            lblMessage.CssClass = "error-message";
            lblMessage.Text = message;
        }

        private void ShowSuccessMessage(string message)
        {
            lblMessage.CssClass = "success-message";
            lblMessage.Text = message;
        }
    }
}