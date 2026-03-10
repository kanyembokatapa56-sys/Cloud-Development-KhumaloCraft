using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace KhumaloCraftEmporium_ST10265272.Pages.OrderBackEnd
{
    public class OrderStatusModel : PageModel
    {
        public List<OrderHistoryItem> OrderHistoryItems { get; set; } = new List<OrderHistoryItem>();

        private readonly string connectionString = "Server=DESKTOP-31QPGQF\\SQLEXPRESS01;Database=KhumaloCraft_DB;Integrated Security=True;TrustServerCertificate=True;";

        public void OnGet()
        {
            LoadOrderHistoryItems();
        }

        private void LoadOrderHistoryItems()
        {
            OrderHistoryItems.Clear();
            string query = @"
                SELECT p.Name, o.Quantity, o.TotalPrice
                FROM OrderHistory o
                JOIN ProductInfo p ON o.ProdID = p.ProductID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderHistoryItems.Add(new OrderHistoryItem
                            {
                                ProductName = reader.GetString(0),
                                Quantity = reader.GetInt32(1),
                                TotalPrice = reader.GetDecimal(2)
                            });
                        }
                    }
                }
            }
        }

        public class OrderHistoryItem
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }
        }
    }
}
