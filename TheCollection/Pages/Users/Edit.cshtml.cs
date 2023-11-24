using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TheCollection.Pages.Users
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth", Policy = "RequireAdminRole")]

    public class EditModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
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
                    string sqlQuery = "SELECT * FROM users WHERE user_id=@id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                userInfo.id = reader["user_id"].ToString();
                                userInfo.name = reader["user_name"].ToString();
                                userInfo.email = reader["user_email"].ToString();
                                userInfo.phone = reader["user_phone"].ToString();
                                userInfo.address = reader["user_address"].ToString();
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
            userInfo.id = Request.Form["id"];
            userInfo.name = Request.Form["name"];
            userInfo.email = Request.Form["email"];
            userInfo.address = Request.Form["address"];
            userInfo.phone = Request.Form["phone"];

            if (userInfo.name.Length == 0 || userInfo.email.Length == 0 ||
               userInfo.phone.Length == 0 || userInfo.address.Length == 0)
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

                    string sqlQuery = "UPDATE users SET user_name=@name, user_email=@email, user_phone=@phone, user_address=@address WHERE user_id=@id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@id", userInfo.id);
                        cmd.Parameters.AddWithValue("@name", userInfo.name);
                        cmd.Parameters.AddWithValue("@email", userInfo.email);
                        cmd.Parameters.AddWithValue("@phone", userInfo.phone);
                        cmd.Parameters.AddWithValue("@address", userInfo.address);

                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            userInfo.phone = ""; userInfo.email = "";
            userInfo.address = ""; userInfo.phone = "";

            successMessage = "User Updated successfully";

            Response.Redirect("/Users/Index");
        }
    }
}
