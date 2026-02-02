using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace KhumaloCraftEmporium_ST10265272.Pages.ProductInfo
{
    public class DeleteProductModel : PageModel
    {
        public Products Product = new Products();


        public string ErrorMessage = "";
        public string SuccessMessage = "";
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

                                Product.ID = " " + reader.GetInt32(0);
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
                ErrorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            int id = int.Parse(Request.Form["ID"]);

            try
            {
                string connectionString = "Server=tcp:superserver9000.database.windows.net,1433;Initial Catalog=KHUMALO_STORE_INFO;Persist Security Info=False;User ID=superserver9000_admin;Password=@OddJobs2003;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM ProductInfo WHERE ProductID = @ProductID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", id);
                        command.ExecuteNonQuery();
                    }
                }
                SuccessMessage = "Product deleted successfully!";
                RedirectToPage("/ProductInfo/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
              
            }
        }
    }
}
