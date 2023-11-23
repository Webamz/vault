using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TheCollection.Pages.Users
{
    public class CreateModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            userInfo.name = Request.Form["name"];
            userInfo.email = Request.Form["email"];
            userInfo.address = Request.Form["address"];
            userInfo.phone = Request.Form["phone"];
            userInfo.password = Request.Form["password"];

            if (userInfo.name.Length == 0 || userInfo.email.Length == 0 ||
                userInfo.phone.Length == 0 || userInfo.address.Length == 0 || userInfo.password.Length == 0)
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

                    string sqlQuery = "INSERT INTO users(user_name, user_email, user_phone, user_address, user_password) VALUES(@name, @email, @phone, @address, @password)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@name", userInfo.name);
                        cmd.Parameters.AddWithValue("@email", userInfo.email);
                        cmd.Parameters.AddWithValue("@phone", userInfo.phone);
                        cmd.Parameters.AddWithValue("@address", userInfo.address);
                        cmd.Parameters.AddWithValue("@password", userInfo.password);


                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            userInfo.phone = ""; userInfo.email = ""; userInfo.password = "";
            userInfo.address = ""; userInfo.phone = "";

            successMessage = "New User Added successfully";

            Response.Redirect("/Users/Index");

        }
    }
}
