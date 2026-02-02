using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace KhumaloCraftEmporium_ST10265272.Pages
{
    public class MyWork2Model : PageModel
    {
        public List<Product> listProducts = new List<Product>();

        private readonly string connectionString = "Server=tcp:superserver9000.database.windows.net,1433;Initial Catalog=KHUMALO_STORE_INFO;Persist Security Info=False;User ID=superserver9000_admin;Password=@OddJobs2003;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public void OnGet()
        {
            // Load products from the database
            string query = "SELECT ProductID, Name, Price, Description FROM ProductInfo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listProducts.Add(new Product
                            {
                                ProductID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDecimal(2),
                                Description = reader.GetString(3)
                          
                            });
                        }
                    }
                }
            }
        }

        public IActionResult OnPostAddToCart(int productId, decimal unitPrice, int quantity)
        {
            // Calculate the total price
            decimal totalPrice = unitPrice * quantity;

            // Insert the product into the cart
            string query = "INSERT INTO Cart (UserID, ProdID, UnitPrice, Quantity, TotalPrice) VALUES (@UserID, @ProdID, @UnitPrice, @Quantity, @TotalPrice)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", 2); 
                    command.Parameters.AddWithValue("@ProdID", productId);
                    command.Parameters.AddWithValue("@UnitPrice", unitPrice);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@TotalPrice", totalPrice);

                    command.ExecuteNonQuery();
                }
            }

            return RedirectToPage("/AddCart/Cart");
        }

        public class Product
        {
            public int ProductID { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }

        }

        public class CartItem
        {
            public int CartID { get; set; }
            public int UserID { get; set; }
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public string ProductDescription { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }
        }
    }
}


