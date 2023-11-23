using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TheVault.Pages.Orders
{
    public class CreateModel : PageModel
    {
        public OrderInfo orderInfo = new OrderInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
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

            //saving the data
            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();

                    string sqlQuery = "INSERT INTO orders(product_id, seller_id, order_price) VALUES(@pdt_id, @seller, @price)";
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
            successMessage = "New Order Added successfully";

            Response.Redirect("/Orders/Index");

        }
    }

}

