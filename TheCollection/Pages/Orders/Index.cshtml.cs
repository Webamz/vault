using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Claims;

namespace TheVault.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public List<OrderInfo> ListofOrders = new List<OrderInfo>();

        public void OnGet()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("admin");

            ListofOrders.Clear();

            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";

                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();

                    string sqlQuery;

                    if (isAdmin)
                    {
                        sqlQuery = "SELECT o.order_id, u.user_name AS customer_name, p.product_name, o.quantity, o.total_price, o.order_date " +
                                   "FROM orders o " +
                                   "JOIN products p ON o.product_id = p.product_id " +
                                   "JOIN users u ON o.customer_id = u.user_id";
                    }
                    else
                    {
                        int userId = Convert.ToInt32(userIdClaim.Value);
                        sqlQuery = "SELECT o.order_id, p.product_name, o.quantity, o.total_price, o.order_date " +
                                   "FROM orders o " +
                                   "JOIN products p ON o.product_id = p.product_id " +
                                   "WHERE o.customer_id = @userId";
                    }

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        if (!isAdmin)
                        {
                            int userId = Convert.ToInt32(userIdClaim.Value);
                            cmd.Parameters.AddWithValue("@userId", userId);
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderInfo orderInfo = new OrderInfo
                                {
                                    Id = reader["order_id"].ToString(),
                                    CustomerName = isAdmin ? reader["customer_name"]?.ToString() : null,
                                    ProductName = reader["product_name"].ToString(),
                                    Quantity = reader["quantity"].ToString(),
                                    TotalPrice = reader["total_price"].ToString(),
                                    OrderDate = reader["order_date"].ToString()
                                };

                                ListofOrders.Add(orderInfo);
                            }
                        }
                    }
                }

                // Debug prints to check the retrieved data
                Console.WriteLine($"Number of orders: {ListofOrders.Count}");
                foreach (var order in ListofOrders)
                {
                    Console.WriteLine($"Order ID: {order.Id}, Customer Name: {order.CustomerName}, Product Name: {order.ProductName}, Quantity: {order.Quantity}, Total Price: {order.TotalPrice}, Order Date: {order.OrderDate}");
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Exception: {sqlEx.Message}");
                // Handle or log the SQL exception
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                // Handle or log other exceptions
            }
        }
    }

    public class OrderInfo
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string TotalPrice { get; set; }
        public string OrderDate { get; set; }
    }
}
