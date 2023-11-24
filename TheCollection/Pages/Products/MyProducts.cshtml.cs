using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Claims;

namespace TheCollection.Pages.Products
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth", Policy = "RequireSellerRole")]
    public class MyProductsModel : PageModel
    {
        public List<ProductInfo> ListofProducts = new List<ProductInfo>();
        public List<CategoryInfo> ListofCategories = new List<CategoryInfo>();

        public void OnGet()
        {
            ListofProducts.Clear();
            try
            {
                int seller = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

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
                                CategoryInfo categoryInfo = new CategoryInfo();
                                categoryInfo.id = reader["category_id"].ToString();
                                categoryInfo.name = reader["category_name"].ToString();

                                ListofCategories.Add(categoryInfo);
                            }
                        }
                    }

                    string sqlQuery = "SELECT * FROM products where seller=@seller";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@seller", seller);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductInfo productInfo = new ProductInfo();
                                productInfo.id = reader["product_id"].ToString();
                                productInfo.name = reader["product_name"].ToString();
                                productInfo.desc = reader["product_description"].ToString();
                                productInfo.price = reader["product_price"].ToString();
                                productInfo.instock = reader["product_instock"].ToString();
                                productInfo.image = reader["product_image"].ToString();
                                productInfo.category = reader["product_category"].ToString();
                                productInfo.seller = reader["seller"].ToString();

                                ListofProducts.Add(productInfo);
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

    public class ProductInfo
    {
        public string id;
        public string name;
        public string desc;
        public string price;
        public string instock;
        public string image;
        public string category;
        public string seller;
    }

    public class CategoryInfo
    {
        public string id;
        public string name;
    }
}
