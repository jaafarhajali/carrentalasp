using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarRental
{
    public partial class VehicleManagement : System.Web.UI.Page
    {
        private readonly string connectionString = @"Server=JAAFARHAJALI\MSSQLSERVER2;Database=CarRentalDB;Trusted_Connection=True;";
        private int? SelectedVehicleId
        {
            get { return (int?)ViewState["SelectedVehicleId"]; }
            set { ViewState["SelectedVehicleId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is admin
            if (Session["UserRole"] == null || !Session["UserRole"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadVehicleData();
                btnUpdateVehicle.Enabled = false;
            }
        }

        private void LoadVehicleData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT VehicleId, VehicleName, VehicleType, Price, Availability 
                               FROM Vehicles 
                               ORDER BY VehicleId DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        DataTable dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());
                        gvVehicles.DataSource = dt;
                        gvVehicles.DataBind();
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("Error loading vehicles: " + ex.Message);
                    }
                }
            }
        }

        protected void btnAddVehicle_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            byte[] imageBytes = null;
            if (fileUpload.HasFile)
            {
                imageBytes = fileUpload.FileBytes;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Vehicles (VehicleName, VehicleType, Price, Availability, Image) 
                               VALUES (@Name, @Type, @Price, 'Available', @Image)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@Name", txtVehicleName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Type", txtVehicleType.Text.Trim());
                        cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                        cmd.Parameters.AddWithValue("@Image", (object)imageBytes ?? DBNull.Value);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        ShowSuccessMessage("Vehicle added successfully!");
                        ClearForm();
                        LoadVehicleData();
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("Error adding vehicle: " + ex.Message);
                    }
                }
            }
        }

        protected void btnUpdateVehicle_Click(object sender, EventArgs e)
        {
            if (!SelectedVehicleId.HasValue || !Page.IsValid) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Vehicles 
                               SET VehicleName = @Name, 
                                   VehicleType = @Type, 
                                   Price = @Price";

                if (fileUpload.HasFile)
                {
                    query += ", Image = @Image";
                }

                query += " WHERE VehicleId = @VehicleId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@VehicleId", SelectedVehicleId.Value);
                        cmd.Parameters.AddWithValue("@Name", txtVehicleName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Type", txtVehicleType.Text.Trim());
                        cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));

                        if (fileUpload.HasFile)
                        {
                            cmd.Parameters.AddWithValue("@Image", fileUpload.FileBytes);
                        }

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        ShowSuccessMessage("Vehicle updated successfully!");
                        LoadVehicleData();
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("Error updating vehicle: " + ex.Message);
                    }
                }
            }
        }

        protected void gvVehicles_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvVehicles.SelectedRow;
            if (row != null)
            {
                SelectedVehicleId = Convert.ToInt32(gvVehicles.DataKeys[row.RowIndex].Value);
                LoadVehicleDetails();
                btnUpdateVehicle.Enabled = true;
                btnAddVehicle.Enabled = false;
            }
        }

        private void LoadVehicleDetails()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Vehicles WHERE VehicleId = @VehicleId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@VehicleId", SelectedVehicleId);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtVehicleName.Text = reader["VehicleName"].ToString();
                                txtVehicleType.Text = reader["VehicleType"].ToString();
                                txtPrice.Text = reader["Price"].ToString();

                                if (reader["Image"] != DBNull.Value)
                                {
                                    byte[] imageData = (byte[])reader["Image"];
                                    string base64String = Convert.ToBase64String(imageData);
                                    imgVehicle.ImageUrl = "data:image/jpeg;base64," + base64String;
                                }
                                else
                                {
                                    imgVehicle.ImageUrl = "";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage("Error loading vehicle details: " + ex.Message);
                    }
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void btnGoToRentCar_Click(object sender, EventArgs e)
        {
            Response.Redirect("RentCar.aspx");
        }

        private void ClearForm()
        {
            txtVehicleName.Text = string.Empty;
            txtVehicleType.Text = string.Empty;
            txtPrice.Text = string.Empty;
            imgVehicle.ImageUrl = string.Empty;
            SelectedVehicleId = null;
            btnUpdateVehicle.Enabled = false;
            btnAddVehicle.Enabled = true;
        }

        private void ShowErrorMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(),
                "ShowError", $"alert('{message}');", true);
        }

        private void ShowSuccessMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(),
                "ShowSuccess", $"alert('{message}');", true);
        }
    }
}