using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TheVault.Pages.Sellers
{
    public class EditModel : PageModel
    {
        public SellerInfo sellerInfo = new SellerInfo();
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
                    string sqlQuery = "SELECT * FROM sellers WHERE seller_id=@id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                sellerInfo.id = reader["seller_id"].ToString();
                                sellerInfo.name = reader["seller_name"].ToString();
                                sellerInfo.email = reader["seller_email"].ToString();
                                sellerInfo.phone = reader["seller_phone"].ToString();
                                sellerInfo.address = reader["seller_address"].ToString();
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
            sellerInfo.id = Request.Form["id"];
            sellerInfo.name = Request.Form["name"];
            sellerInfo.email = Request.Form["email"];
            sellerInfo.address = Request.Form["address"];
            sellerInfo.phone = Request.Form["phone"];

            if (sellerInfo.name.Length == 0 || sellerInfo.email.Length == 0 ||
               sellerInfo.phone.Length == 0 || sellerInfo.address.Length == 0)
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

                    string sqlQuery = "UPDATE sellers SET seller_name=@name, seller_email=@email, seller_phone=@phone, seller_address=@address WHERE seller_id=@id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@id", sellerInfo.id);
                        cmd.Parameters.AddWithValue("@name", sellerInfo.name);
                        cmd.Parameters.AddWithValue("@email", sellerInfo.email);
                        cmd.Parameters.AddWithValue("@phone", sellerInfo.phone);
                        cmd.Parameters.AddWithValue("@address", sellerInfo.address);

                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            sellerInfo.phone = ""; sellerInfo.email = "";
            sellerInfo.address = ""; sellerInfo.phone = "";

            successMessage = "Seller Updated successfully";

            Response.Redirect("/Sellers/Index");
        }
    }
}
