using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarRental
{
    public partial class Registration : System.Web.UI.Page
    {
        // Connection string (update this as per your configuration)
        private string connectionString = @"Server=JAAFARHAJALI\MSSQLSERVER2;Database=CarRentalDB;Trusted_Connection=True;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any existing session
                Session.Clear();
            }
        }

        protected void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            TextBoxMode mode = chkShowPassword.Checked ? TextBoxMode.SingleLine : TextBoxMode.Password;

            txtPassword.TextMode = mode;
            txtConfirmPassword.TextMode = mode;

            if (chkShowPassword.Checked)
            {
                txtPassword.Attributes.Add("value", txtPassword.Text);
                txtConfirmPassword.Attributes.Add("value", txtConfirmPassword.Text);
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                lblMessage.Text = "Please fill in all fields.";
                return;
            }

            if (password != confirmPassword)
            {
                lblMessage.Text = "Passwords do not match.";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // First check if username already exists
                    string checkQuery = "SELECT COUNT(1) FROM Users WHERE Username=@Username";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Username", username);
                        int exists = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (exists > 0)
                        {
                            lblMessage.Text = "Username already exists. Please choose a different username.";
                            return;
                        }
                    }

                    // If username doesn't exist, proceed with registration
                    string insertQuery = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // Registration successful - redirect to login page
                            Response.Redirect("~/Login.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            lblMessage.Text = "Registration failed. Please try again.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"An error occurred: {ex.Message}";
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            chkShowPassword.Checked = false;
            lblMessage.Text = string.Empty;
        }

        protected void lnkBackToLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}