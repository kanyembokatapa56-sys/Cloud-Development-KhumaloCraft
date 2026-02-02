using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace KhumaloCraftEmporium_ST10265272.Pages.ProductInfo
{
    public class EditProductModel : PageModel
    {
        public Products Product = new Products();
        public List<string> Categories { get; } = new List<string> { "Hand-Made Crafts", "Painting", "Decor" };
        public List<string> Availabilities { get; } = new List<string> { "In Stock", "Out of Stock" };

        public string errorMessage { get; set; }
        public string successMessage { get; set; }

        public void OnGet(int id)
        {
            try
            {
                string connectionString = "Server=tcp:superserver9000.database.windows.net,1433;Initial Catalog=KHUMALO_STORE_INFO;Persist Security Info=False;User ID=superserver9000_admin;Password=@OddJobs2003;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM ProductInfo WHERE ProductID = @ProductID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Product.ID = " "+ reader.GetInt32(0);
                                Product.Name = reader.GetString(1);
                                Product.Price = reader.GetDecimal(2);
                                Product.Category = reader.GetString(3);
                                Product.Availability = reader.GetString(4);
                                Product.Description = reader.GetString(5);
                                Product.CreatedOn = reader.GetDateTime(6);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public IActionResult OnPost()
        {
            int id = int.Parse(Request.Form["ID"]);
            string name = Request.Form["Name"];
            decimal price = decimal.Parse(Request.Form["Price"]);
            string category = Request.Form["Category"];
            string availability = Request.Form["Availability"];
            string description = Request.Form["Description"];

            if (string.IsNullOrEmpty(name) || price <= 0 || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(availability) || string.IsNullOrEmpty(description))
            {
                errorMessage = "All fields are required and must be valid.";
                return Page();
            }

            try
            {
                string connectionString = "Server=tcp:superserver9000.database.windows.net,1433;Initial Catalog=KHUMALO_STORE_INFO;Persist Security Info=False;User ID=superserver9000_admin;Password=@OddJobs2003;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE ProductInfo SET Name = @Name, Price = @Price, Category = @Category, Availability = @Availability, Description = @Description WHERE ProductID = @ProductID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", id);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Availability", availability);
                        command.Parameters.AddWithValue("@Description", description);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return Page();
            }

            successMessage = "Product updated successfully!";
            return RedirectToPage("/ProductInfo/Products");
        }
    }


}


