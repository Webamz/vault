using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TheVault.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public List<OrderInfo> ListofOrders = new List<OrderInfo>();
        public void OnGet()
        {

            ListofOrders.Clear();

            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";

                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT * FROM orders";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderInfo orderInfo = new OrderInfo();
                                orderInfo.id = reader["order_id"].ToString();
                                orderInfo.seller_id = reader["seller_id"].ToString();
                                orderInfo.pdt_id = reader["product_id"].ToString();
                                orderInfo.order_price = reader["order_price"].ToString();

                                ListofOrders.Add(orderInfo);
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

        }
    }

    public class OrderInfo
    {
        public string id;
        public string pdt_id;
        public string seller_id;
        public string order_price;
    }
}
