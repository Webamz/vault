using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using TheVault.Pages.Products;

namespace TheCollection.Pages.Products
{
    [AllowAnonymous]

    public class DetailsModel : PageModel
    {

        public ProductInfo productInfo = new ProductInfo();
        public CategoryInfo categoryInfo = new CategoryInfo();
        public List<CategoryInfo> ListofCategories = new List<CategoryInfo>();
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
                    string sqlQuery = "SELECT * FROM products WHERE product_id=@id";

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
    }
}
