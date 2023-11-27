using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace TheCollection.Pages.Users
{
    [Authorize]
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
                                userInfo.role = reader["user_role"].ToString();
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
            userInfo.role = Request.Form["role"];

            if (userInfo.name.Length == 0 || userInfo.email.Length == 0 ||
                userInfo.phone.Length == 0 || userInfo.address.Length == 0 || userInfo.role.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            // Update the data, including user_role
            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();

                    // Check if the user_role is changed to 2 (seller)
                    if (userInfo.role == "2")
                    {
                        // Update the user information in the users table
                        UpdateUserInformation(con);

                        // Create a new seller in the seller table
                        CreateSeller(con);
                    }
                    else
                    {
                        // If the role is not seller, update only the user information
                        UpdateUserInformation(con);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            userInfo.phone = ""; userInfo.email = "";
            userInfo.address = ""; userInfo.phone = ""; userInfo.role = "";

            successMessage = "User Updated successfully";

            Response.Redirect("/Users/Index");
        }

        // Method to update user information in the users table
        private void UpdateUserInformation(SqlConnection con)
        {
            string sqlQuery = "UPDATE users SET user_name=@name, user_email=@email, user_phone=@phone, user_address=@address, user_role=@role WHERE user_id=@id";
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                cmd.Parameters.AddWithValue("@id", userInfo.id);
                cmd.Parameters.AddWithValue("@name", userInfo.name);
                cmd.Parameters.AddWithValue("@email", userInfo.email);
                cmd.Parameters.AddWithValue("@phone", userInfo.phone);
                cmd.Parameters.AddWithValue("@address", userInfo.address);
                cmd.Parameters.AddWithValue("@role", userInfo.role);

                cmd.ExecuteNonQuery();
            }
        }

        // Method to create a new seller in the seller table
        private void CreateSeller(SqlConnection con)
        {
            // Check if a seller with the same email already exists
            string checkEmailQuery = "SELECT COUNT(*) FROM sellers WHERE seller_email=@email";
            using (SqlCommand checkEmailCmd = new SqlCommand(checkEmailQuery, con))
            {
                checkEmailCmd.Parameters.AddWithValue("@email", userInfo.email);
                int existingSellerCount = (int)checkEmailCmd.ExecuteScalar();

                if (existingSellerCount == 0)
                {
                    // Create a new seller
                    string createSellerQuery = "INSERT INTO sellers (seller_name, seller_email, seller_phone, seller_address, seller_id) VALUES (@name, @email, @phone, @address, @id)";
                    using (SqlCommand createSellerCmd = new SqlCommand(createSellerQuery, con))
                    {
                        createSellerCmd.Parameters.AddWithValue("@id", userInfo.id);
                        createSellerCmd.Parameters.AddWithValue("@name", userInfo.name);
                        createSellerCmd.Parameters.AddWithValue("@email", userInfo.email);
                        createSellerCmd.Parameters.AddWithValue("@phone", userInfo.phone);
                        createSellerCmd.Parameters.AddWithValue("@address", userInfo.address);

                        createSellerCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    errorMessage = "Seller with the same email already exists";
                }
            }
        }
    }
}
