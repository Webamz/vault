using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Claims;

namespace TheVault.Pages.Products
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth", Policy = "RequireAdminOrSellerRole")]
    public class CreateModel : PageModel
    {
        public ProductInfo productInfo = new ProductInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
            // Check the user's role
            if (User.IsInRole("seller"))
            {
                // If the user is a seller, set the sellerId based on their ID
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    productInfo.seller = userIdClaim.Value.Trim();
                }
            }
            else
            {
                // If the user is an admin, you may leave sellerId empty or provide a way to input it
                // productInfo.sellerId = ""; // Set it as needed for your application
            }
        }

        public void OnPost()
        {
            productInfo.name = Request.Form["name"];
            productInfo.desc = Request.Form["description"];
            productInfo.price = Request.Form["price"];
            productInfo.instock = Request.Form["instock"];
            productInfo.image = Request.Form["image"];
            productInfo.category = Request.Form["category"];


            if (productInfo.name.Length == 0 || productInfo.desc.Length == 0 || productInfo.price.Length == 0 ||
                productInfo.price.Length == 0 || productInfo.instock.Length == 0 || productInfo.image.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            // Print or log the seller value to check if it has a valid value
            Console.WriteLine($"Seller ID: {productInfo.seller}");

            //saving the data
            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "INSERT INTO products(product_name, product_description, product_price, product_instock, product_image, product_category, userid)" +
                            "VALUES(@name, @desc, @price, @instock, @image, @category, @sellerId)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@name", productInfo.name);
                        cmd.Parameters.AddWithValue("@desc", productInfo.desc);
                        cmd.Parameters.AddWithValue("@price", productInfo.price);
                        cmd.Parameters.AddWithValue("@instock", productInfo.instock);
                        cmd.Parameters.AddWithValue("@image", productInfo.image);
                        cmd.Parameters.AddWithValue("@category", productInfo.category);
                        cmd.Parameters.AddWithValue("@sellerId", Convert.ToInt32(productInfo.seller));



                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            productInfo.name = ""; productInfo.desc = ""; productInfo.price = "";
            productInfo.image = ""; productInfo.category = ""; productInfo.instock = "";
            successMessage = "New Product Added successfully";

            Response.Redirect("/Products/Index");
        }
    }
}
