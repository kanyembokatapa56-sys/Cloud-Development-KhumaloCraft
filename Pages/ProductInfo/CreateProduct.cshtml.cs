using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;


namespace KhumaloCraftEmporium_ST10265272.Pages.ProductInfo
{
    public class CreateProductModel : PageModel
    {
        public Products product = new Products();

        public List<string> Categories { get; } = new List<string> { "Hand-Made Crafts", "Painting", "Decor" };
        public List<string> Availabilities { get; } = new List<string> { "In Stock", "Out of Stock" };


        public string ErrorMessage = "";
        public string SuccessMessage = "";


        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Retrieve form data using Request.Form
            string name = Request.Form["Product.Name"];
            decimal price = Convert.ToDecimal(Request.Form["Product.Price"]);
            string category = Request.Form["Product.Category"];
            string availability = Request.Form["Product.Availability"];
            string description = Request.Form["Product.Description"];

            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill out all required fields.";
                return Page();
            }

  
            try
            {
                string connectionString = "Server=tcp:superserver9000.database.windows.net,1433;Initial Catalog=KHUMALO_STORE_INFO;Persist Security Info=False;User ID=superserver9000_admin;Password=@OddJobs2003;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO ProductInfo (Name, Price, Category, Availability, Description, CreatedOn) VALUES (@Name, @Price, @Category, @Availability, @Description, @CreatedOn);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Set parameter values
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Availability", availability);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                        // Execute the query
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

            SuccessMessage = "Product added successfully!";
            return RedirectToPage("/ProductInfo/Products");
        }


    }

}
