using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
namespace KhumaloCraftEmporium_ST10265272.Pages.Login
{
    public class LoginPageModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "All fields are required.";
                return Page();
            }

            try
            {
                string connectionString = "Server=tcp:superserver9000.database.windows.net,1433;Initial Catalog=KHUMALO_STORE_INFO;Persist Security Info=False;User ID=superserver9000_admin;Password=@OddJobs2003;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Password", Password);

                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            if (Email == "admin@khumalocraftempo.co.za" && Password == "Abmnormalize2000")
                            {
                                return RedirectToPage("/ProductInfo/Products");
                            }
                            else
                            {
                                return RedirectToPage("/Index");
                            }
                        }
                        else
                        {
                            ErrorMessage = "Invalid email or password.";
                            return Page();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred: " + ex.Message;
                return Page();
            }
        }
    }
}
