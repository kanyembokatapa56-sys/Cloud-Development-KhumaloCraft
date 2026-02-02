using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace KhumaloCraftEmporium_ST10265272.Pages.Cart
{
    public class CartModel : PageModel
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal OverallTotal { get; set; }

        private readonly string connectionString = "Server=tcp:superserver9000.database.windows.net,1433;Initial Catalog=KHUMALO_STORE_INFO;Persist Security Info=False;User ID=superserver9000_admin;Password=@OddJobs2003;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public void OnGet()
        {
            LoadCartItems();
        }

        public IActionResult OnPost(string productId, string remove)
        {
           

                string removeQuery = "DELETE FROM Cart WHERE ProdID = @ProductId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(removeQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.ExecuteNonQuery();
                    }
                }

               
                LoadCartItems();



            return RedirectToPage();
        }

        public IActionResult OnPostCheckout()
        {
            string moveQuery = @"
            INSERT INTO OrderItems (ProdID, Quantity, TotalPrice)
            SELECT ProdID, Quantity, TotalPrice FROM Cart";

            string insertQuery = @"
            INSERT INTO OrderHistory (ProdID, Quantity, TotalPrice)
            SELECT ProdID, Quantity, TotalPrice FROM Cart";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(moveQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            string clearQuery = "DELETE FROM Cart";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(clearQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToPage("/AddCart/Checkout");
        }

        private void LoadCartItems()
        {
            CartItems.Clear();
            string query = @"
            SELECT p.Name, p.Description, c.Quantity, c.TotalPrice, c.ProdID
            FROM Cart c
            JOIN ProductInfo p ON c.ProdID = p.ProductID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CartItems.Add(new CartItem
                            {
                                ProductName = reader.GetString(0),
                                Description = reader.GetString(1),
                                Quantity = reader.GetInt32(2),
                                TotalPrice = reader.GetDecimal(3),
                                ProductId = " "+reader.GetInt32(4)
                        }) ;
                        }
                    }
                }
            }

            OverallTotal = CartItems.Sum(item => item.TotalPrice);
        }

        public class CartItem
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }
        }
    }
}
