using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TheVault.Pages.Sellers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth", Policy = "RequireAdminRole")]
    public class IndexModel : PageModel
    {
        public List<SellerInfo> ListofSellers = new List<SellerInfo>();
        public void OnGet()
        {

            ListofSellers.Clear();

            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";

                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT * FROM sellers";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SellerInfo sellerInfo = new SellerInfo();
                                sellerInfo.id = reader["seller_id"].ToString();
                                sellerInfo.name = reader["seller_name"].ToString();
                                sellerInfo.email = reader["seller_email"].ToString();
                                sellerInfo.phone = reader["seller_phone"].ToString();
                                sellerInfo.address = reader["seller_address"].ToString();
                                
                                ListofSellers.Add(sellerInfo);
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

    public class SellerInfo
    {
        public string id;
        public string name;
        public string email;
        public string phone;
        public string address;

    }
}

