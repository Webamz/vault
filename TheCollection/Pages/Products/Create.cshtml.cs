using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Claims;

namespace TheVault.Pages.Products
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth", Policy = "RequireAdminOrSellerRole")]
    public class CreateModel : PageModel
    {
        public ProductInfo productInfo = new ProductInfo();
        public List<CategoryInfo> ListofCategories = new List<CategoryInfo>();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
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
        }

        public void OnPost()
        {
            if (User.IsInRole("seller"))
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    productInfo.seller = userIdClaim.Value.ToString();
                    Console.WriteLine("before retrieval id : "+ productInfo.seller);  
                }
            }
            else
            {
                productInfo.seller = Request.Form["seller"];
            }
            productInfo.name = Request.Form["name"];
            productInfo.desc = Request.Form["description"];
            productInfo.price = Request.Form["price"];
            productInfo.instock = Request.Form["instock"];
            productInfo.image = Request.Form["image"];
            productInfo.category = Request.Form["category"];

            if (string.IsNullOrEmpty(productInfo.name) || string.IsNullOrEmpty(productInfo.desc) ||
                string.IsNullOrEmpty(productInfo.price) || string.IsNullOrEmpty(productInfo.instock) ||
                string.IsNullOrEmpty(productInfo.image) || string.IsNullOrEmpty(productInfo.category))
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    Console.WriteLine("Runnig query for retrieval");
                    //// Retrieve seller_id based on the logged-in user's id
                    //string sqlQuery2 = "SELECT * FROM sellers WHERE seller_id = @id";
                    //using (SqlCommand cmd = new SqlCommand(sqlQuery2, con))
                    //{
                    //    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(productInfo.seller));
                    //    using (SqlDataReader reader = cmd.ExecuteReader())
                    //    {
                    //        if (reader.Read())
                    //        {
                    //            int sellerIdFromQuery = Convert.ToInt32(reader["id"]);
                    //            productInfo.seller = sellerIdFromQuery.ToString();
                    //            Console.WriteLine("Retrieved id " + productInfo.seller);
                    //        }
                    //        else
                    //        {
                    //            errorMessage = "Seller ID not found in sellers table.";
                    //            return;
                    //        }
                    //    }
                    //}

                    // Insert data into the products table
                    string sqlQuery = "INSERT INTO products(product_name, product_description, product_price, product_instock, product_image, product_category, seller)" +
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

                    // Refresh the list of categories
                    string sqlQuery3 = "SELECT * FROM category";
                    ListofCategories.Clear(); // Clear existing categories
                    using (SqlCommand cmd = new SqlCommand(sqlQuery3, con))
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
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            productInfo.name = ""; productInfo.desc = ""; productInfo.price = "";
            productInfo.image = ""; productInfo.instock = "";
            successMessage = "New Product Added successfully";

            Response.Redirect("/Products/Index");
        }
    }
}
