using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace TheCollection.Pages.Users
{
    [AllowAnonymous]
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
            string confirmPassword = Request.Form["confirmPassword"];

            // Validate password and confirmation
            if (userInfo.password != confirmPassword)
            {
                errorMessage = "Password and Confirm Password do not match.";
                return;
            }

            // Validate other fields as needed

            // Hash the password
            string hashedPassword = HashPassword(userInfo.password);
            userInfo.password = hashedPassword;

            // Saving the data
            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    int role_id = 3;

                    string sqlQuery = "INSERT INTO users(user_name, user_email, user_phone, user_address, user_password, user_role) VALUES(@name, @email, @phone, @address, @password, @role_id)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@name", userInfo.name);
                        cmd.Parameters.AddWithValue("@email", userInfo.email);
                        cmd.Parameters.AddWithValue("@phone", userInfo.phone);
                        cmd.Parameters.AddWithValue("@address", userInfo.address);
                        cmd.Parameters.AddWithValue("@password", userInfo.password);
                        cmd.Parameters.AddWithValue("@role_id", role_id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            userInfo.phone = "";
            userInfo.email = "";
            userInfo.password = "";
            userInfo.address = "";
            userInfo.phone = "";

            successMessage = "New User Added successfully";

            Response.Redirect("/Users/Index");
        }

        // Hash password using SHA-256 algorithm
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
