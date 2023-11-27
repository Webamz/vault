using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Text;
using TheCollection.Pages.Products;

namespace TheCollection.Pages
{
    public class OrderModel : PageModel
    {

        public List<ProductInfo> CartItems { get; set; }
        public decimal TotalPrice { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public IActionResult OnPost(string cartItems, decimal totalPrice)
        {
            // Deserialize cartItems
            var decodedCartItems = JsonConvert.DeserializeObject<List<ProductInfo>>(cartItems);

            Console.WriteLine($"cartItems count: {decodedCartItems?.Count}");
            Console.WriteLine($"totalPrice: {totalPrice}");

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                string connectionString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("OnPost method reached successfully.");

                    // Start a transaction to ensure atomicity
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string sqlQuery = "INSERT INTO orders (customer_id, product_id, quantity, total_price, order_date) " +
                                "VALUES (@customerId, @productId, @quantity, @totalPrice, @orderdate);";

                            Console.WriteLine("working on loop");
                            Console.WriteLine($"decodedCartItems count: {decodedCartItems?.Count}");

                            foreach (var productGroup in decodedCartItems.GroupBy(p => p.id))
                            {
                                var product = productGroup.First();
                                var quantity = productGroup.Count();

                                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection, transaction))
                                {
                                    // Set parameter values for order details
                                    cmd.Parameters.AddWithValue("@customerId", Convert.ToInt32(userIdClaim.Value));
                                    cmd.Parameters.AddWithValue("@orderDate", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@productId", product.id);
                                    cmd.Parameters.AddWithValue("@quantity", quantity);
                                    cmd.Parameters.AddWithValue("@totalPrice", decimal.Parse(product.price) * quantity);

                                    Console.WriteLine(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                                    // Execute the query for order details
                                    cmd.ExecuteNonQuery();
                                }

                                // Update product quantity in stock
                                string updateProductQuery = "UPDATE products SET product_instock = product_instock - @quantity WHERE product_id = @productId;";

                                using (SqlCommand updateCommand = new SqlCommand(updateProductQuery, connection, transaction))
                                {
                                    // Set parameter values for product update
                                    updateCommand.Parameters.AddWithValue("@quantity", quantity);
                                    updateCommand.Parameters.AddWithValue("@productId", product.id);

                                    // Execute the query for product update
                                    updateCommand.ExecuteNonQuery();
                                }
                            }

                            // Commit the transaction
                            transaction.Commit();

                            // Clear the cart or perform any other necessary actions
                            // ...
                            Console.WriteLine("working on database.");
                            HttpContext.Session.Remove("CartItems");

                            // Redirect to a success page or show a success message
                            return RedirectToPage("/OrderSuccess");
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction in case of an error
                            transaction.Rollback();
                            Console.WriteLine(ex.Message);

                            // Log the error or handle it appropriately
                            ErrorMessage = "An error occurred while processing the order.";
                            return Page();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                ErrorMessage = "An error occurred while processing the order.";
                return Page();
            }
        }


            public IActionResult OnGet(string cartItems, decimal totalPrice)
        {
            var decodedCartItems = JsonConvert.DeserializeObject<List<ProductInfo>>(Uri.UnescapeDataString(cartItems));

            if (decodedCartItems != null)
            {
                CartItems = decodedCartItems;
                TotalPrice = totalPrice;
                Console.WriteLine($"OnGet - CartItems count: {CartItems.Count}");
                return Page();
            }

            HttpContext.Session.Set("CartItems", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(decodedCartItems)));


            // Handle the case where the cart items couldn't be decoded
            return RedirectToPage("/Cart"); // Redirect to the cart page or handle appropriately
        }
    }

}
