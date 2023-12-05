using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TheVault.Pages.Products
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth", Policy = "RequireAdminOrSellerRole")]

    public class EditModel : PageModel
    {
        public ProductInfo productInfo = new ProductInfo();
        public List<CategoryInfo> ListofCategories = new List<CategoryInfo>();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {

            FetchCategory();
            String id = Request.Query["id"];
            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT * FROM products WHERE product_id=@id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productInfo.id = reader["product_id"].ToString();
                                productInfo.name = reader["product_name"].ToString();
                                productInfo.desc = reader["product_description"].ToString();
                                productInfo.price = reader["product_price"].ToString();
                                productInfo.instock = reader["product_instock"].ToString();
                                productInfo.image = reader["product_image"].ToString();
                                productInfo.category = reader["product_category"].ToString();
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
            productInfo.id = Request.Form["id"];
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

            //update the data
            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();

                    string sqlQuery = "UPDATE products SET product_name=@name, product_description=@desc, product_price=@price, product_instock=@instock," +
                        "product_image=@image, product_category=@category WHERE product_id=@id";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@id", productInfo.id);
                        cmd.Parameters.AddWithValue("@name", productInfo.name);
                        cmd.Parameters.AddWithValue("@desc", productInfo.desc);
                        cmd.Parameters.AddWithValue("@price", productInfo.price);
                        cmd.Parameters.AddWithValue("@instock", productInfo.instock);
                        cmd.Parameters.AddWithValue("@image", productInfo.image);
                        cmd.Parameters.AddWithValue("@category", productInfo.category);


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
            successMessage = "Product Updated successfully";

            Response.Redirect("/Products/Index");
        }

        public List<CategoryInfo> FetchCategory()
        {
            string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string sqlQuery2 = "SELECT * FROM category";

                using (SqlCommand cmd = new SqlCommand(sqlQuery2, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryInfo category = new CategoryInfo();
                            category.id = reader["category_id"].ToString();
                            category.name = reader["category_name"].ToString();

                            ListofCategories.Add(category);
                        }
                    }
                }
            }

            return ListofCategories;
        }
    }
}


