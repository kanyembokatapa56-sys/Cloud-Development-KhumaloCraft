using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace KhumaloCraftEmporium_ST10265272.Pages.ProductInfo
{
    public class ProductsModel : PageModel
    {
        public List<Products> listProducts = new List<Products>();

        public void OnGet()
        {

      
            try
            {
                string connectionString = "Server=tcp:superserver9000.database.windows.net,1433;Initial Catalog=KHUMALO_STORE_INFO;Persist Security Info=False;User ID=superserver9000_admin;Password=@OddJobs2003;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "Select * From ProductInfo";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Products product = new Products();
                                 product.ID = "" + reader.GetInt32(0);
                                 product.Name = reader.GetString(1);
                                 product.Price = reader.GetDecimal(2);
                                 product.Category = reader.GetString(3);
                                 product.Availability = reader.GetString(4);
                                 product.Description = reader.GetString(5);
                                 product.CreatedOn = reader.GetDateTime(6);

                                listProducts.Add(product);

                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }


        }
    }
        
    public class Products
    {
        public string ID;
        public string Name;
        public decimal Price;
        public string Category;
        public string Availability;
        public string Description;
        public DateTime CreatedOn;
    }

}
