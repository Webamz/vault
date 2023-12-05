using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using TheCollection.Pages.Users;

namespace TheCollection.Pages.Accounts
{
    public class ForgotPasswordModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            userInfo.email = Request.Form["email"];
            string new_password = HashPassword(Request.Form["password"]);
            string confirmPassword = HashPassword(Request.Form["confirmPassword"]);

            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();

                    // Use the correct parameter name in the WHERE clause
                    string sqlQuery = "SELECT * FROM users WHERE user_email=@email";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        // Use the correct parameter name for the email
                        cmd.Parameters.AddWithValue("@email", userInfo.email);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userInfo.id = reader["user_id"].ToString();
                                userInfo.email = reader["user_email"].ToString();
                                Console.WriteLine("The id is: " + userInfo.id);
                            }
                        }
                    }

                    // Use the correct parameter name in the UPDATE query
                    string sqlQuery2 = "UPDATE users SET user_password=@new_password WHERE user_id=@id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery2, con))
                    {
                        cmd.Parameters.AddWithValue("@id", userInfo.id);
                        cmd.Parameters.AddWithValue("@new_password", new_password);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            if (HashPassword(new_password) != HashPassword(confirmPassword))
            {
                errorMessage = "Password and Confirm Password do not match.";
                return;
            }



            // Redirect to the login page
            Response.Redirect("/Accounts/Login");
        }



        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
