using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TheVault.Pages.Sellers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]
    [Authorize(Policy = "RequireAdminRole")]
    public class CreateModel : PageModel
    {
        public SellerInfo SellerInfo = new SellerInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            SellerInfo.name = Request.Form["name"];
            SellerInfo.email = Request.Form["email"];
            SellerInfo.address = Request.Form["address"];
            SellerInfo.phone = Request.Form["phone"];

            if (SellerInfo.name.Length == 0 || SellerInfo.email.Length == 0 ||
                SellerInfo.phone.Length == 0 || SellerInfo.address.Length == 0)
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

                    string sqlQuery = "INSERT INTO sellers(seller_name, seller_email, seller_phone, seller_address) VALUES(@name, @email, @phone, @address)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@name", SellerInfo.name);
                        cmd.Parameters.AddWithValue("@email", SellerInfo.email);
                        cmd.Parameters.AddWithValue("@phone", SellerInfo.phone);
                        cmd.Parameters.AddWithValue("@address", SellerInfo.address);

                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            SellerInfo.phone = ""; SellerInfo.email = "";
            SellerInfo.address = ""; SellerInfo.phone = "";

            successMessage = "New Seller Added successfully";

            Response.Redirect("/Sellers/Index");

        }
    }
}
