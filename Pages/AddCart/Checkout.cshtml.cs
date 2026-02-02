using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace KhumaloCraftEmporium_ST10265272.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        public List<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
        public decimal OverallTotal { get; set; }
        public string UserName { get; set; }

        private readonly string connectionString = "Server=tcp:superserver9000.database.windows.net,1433;Initial Catalog=KHUMALO_STORE_INFO;Persist Security Info=False;User ID=superserver9000_admin;Password=@OddJobs2003;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public void OnGet(int customerId)
        {
            LoadInvoiceItems();
            LoadUserName(customerId);
        }

        public IActionResult OnPostDownloadInvoice(int customerId)
        {
            LoadInvoiceItems();
            LoadUserName(customerId);

           
            string invoiceContent = GenerateInvoiceContent();

          
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.txt");
            System.IO.File.WriteAllText(filePath, invoiceContent);

         
            ClearOrderItems();

            
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = "Invoice.txt";
            return File(fileBytes, "text/plain", fileName);
        }

        public void OnPageHandlerExecuting()
        {
            
            ClearOrderItems();
        }

        private void ClearOrderItems()
        {
            string query = "DELETE FROM OrderItems";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private string GenerateInvoiceContent()
        {
            string invoiceContent = $"User: {UserName}\n\n";
            invoiceContent += "Product\tDescription\tQuantity\tTotal Price\n";
            foreach (var item in InvoiceItems)
            {
                invoiceContent += $"{item.ProductName}\t{item.Description}\t{item.Quantity}\t{item.TotalPrice.ToString("C")}\n";
            }
            invoiceContent += $"\nOverall Total: {OverallTotal.ToString("C")}";
            return invoiceContent;
        }

        private void LoadInvoiceItems()
        {
            InvoiceItems.Clear();
            string query = @"
                SELECT p.Name, p.Description, o.Quantity, o.TotalPrice
                FROM OrderItems o
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
                            InvoiceItems.Add(new InvoiceItem
                            {
                                ProductName = reader.GetString(0),
                                Description = reader.GetString(1),
                                Quantity = reader.GetInt32(2),
                                TotalPrice = reader.GetDecimal(3)
                            });
                        }
                    }
                }
            }

            OverallTotal = InvoiceItems.Sum(item => item.TotalPrice);
        }

        private void LoadUserName(int customerId)
        {
            string query = "SELECT FirstName, LastName FROM Users WHERE CustomerID = @CustomerID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", 2);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserName = $"{reader.GetString(0)} {reader.GetString(1)}";
                        }
                    }
                }
            }
        }

        public class InvoiceItem
        {
            public string ProductName { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }
        }
    }
}
