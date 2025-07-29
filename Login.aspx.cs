using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace CarRental
{
    public partial class Login : System.Web.UI.Page
    {
        private string connectionString = @"Server=JAAFARHAJALI\MSSQLSERVER2;Database=CarRentalDB;Trusted_Connection=True;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();

                if (Session["Username"] != null)
                {
                    RedirectBasedOnRole(Session["UserRole"].ToString());
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT Username, Role 
                        FROM Users 
                        WHERE Username=@Username AND Password=@Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Session["Username"] = username;
                                Session["UserRole"] = reader["Role"].ToString();
                                RedirectBasedOnRole(reader["Role"].ToString());
                            }
                            else
                            {
                                lblMessage.Visible = true;
                                lblMessage.Text = "Invalid username or password.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "An error occurred during login. Please try again.";
            }
        }

        private void RedirectBasedOnRole(string role)
        {
            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("VehicleManagement.aspx");
            }
            else
            {
                Response.Redirect("RentCar.aspx");
            }
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Registration.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}