using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace KhumaloCraftEmporium_ST10265272.Pages.Login
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
         
        }

        public IActionResult OnPost()
        {
          
            string connectionString = "Server=DESKTOP-31QPGQF\\SQLEXPRESS01;Database=KhumaloCraft_DB;Integrated Security=True;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {        
                connection.Open();
                string sql = "INSERT INTO Users (FirstName, LastName, Email, Password, CreatedOn) VALUES (@FirstName, @LastName, @Email, @Password, @CreatedOn)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                    // Execute the SQL command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // User successfully registered
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        // Registration failed
                        return Page();
                    }
                }
            }
        }
    }
}
