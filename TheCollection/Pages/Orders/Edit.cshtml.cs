using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TheVault.Pages.Orders
{
    public class EditModel : PageModel
    {
        public OrderInfo orderInfo = new OrderInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT * FROM orders WHERE order_id=@id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                orderInfo.seller_id = reader["seller_id"].ToString();
                                orderInfo.pdt_id = reader["product_id"].ToString();
                                orderInfo.order_price = reader["order_price"].ToString();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            orderInfo.pdt_id = Request.Form["pdt_id"];
            orderInfo.seller_id = Request.Form["seller"];
            orderInfo.order_price = Request.Form["price"];

            if (orderInfo != null &&
                (orderInfo.pdt_id == null || orderInfo.pdt_id.Length == 0) ||
                (orderInfo.seller_id == null || orderInfo.seller_id.Length == 0) ||
                (orderInfo.order_price == null || orderInfo.order_price.Length == 0))
            {
                errorMessage = "All fields are required";
                return;
            }

            //update the data
            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();

                    string sqlQuery = "UPDATE orders SET product_id=@pdt_id, seller_id=@seller, order_price=@price";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pdt_id", orderInfo.pdt_id);
                        cmd.Parameters.AddWithValue("@seller", orderInfo.seller_id);
                        cmd.Parameters.AddWithValue("@price", orderInfo.order_price);

                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            orderInfo.pdt_id = ""; orderInfo.seller_id = "";
            orderInfo.order_price = "";
            successMessage = "Order Updated successfully";

            Response.Redirect("/Orders/Index");
        }
    }

}

