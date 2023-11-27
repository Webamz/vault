using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Claims;

namespace TheCollection.Pages.Users
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public List<UserInfo> ListofUsers = new List<UserInfo>();

        public void OnGet()
        {
            ListofUsers.Clear();

            try
            {
                string conString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";

                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();

                    // Check if the current user is an admin
                    if (User.IsInRole("admin"))
                    {
                        // If admin, retrieve all users
                        string sqlQuery = "SELECT user_id, user_name, user_email, user_phone, user_address, user_password, user_role FROM users";
                        using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    UserInfo userInfo = new UserInfo();
                                    userInfo.id = reader["user_id"].ToString();
                                    userInfo.name = reader["user_name"].ToString();
                                    userInfo.email = reader["user_email"].ToString();
                                    userInfo.phone = reader["user_phone"].ToString();
                                    userInfo.address = reader["user_address"].ToString();
                                    userInfo.password = reader["user_password"].ToString();
                                    userInfo.role = GetRoleName(reader["user_role"].ToString());

                                    ListofUsers.Add(userInfo);
                                }
                            }
                        }
                    }
                    else
                    {
                        // If not an admin, retrieve details of the current user
                        int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                        string sqlQuery = "SELECT user_id, user_name, user_email, user_phone, user_address, user_password, user_role FROM users WHERE user_id = @userId";

                        using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@userId", userId);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    UserInfo userInfo = new UserInfo();
                                    userInfo.id = reader["user_id"].ToString();
                                    userInfo.name = reader["user_name"].ToString();
                                    userInfo.email = reader["user_email"].ToString();
                                    userInfo.phone = reader["user_phone"].ToString();
                                    userInfo.address = reader["user_address"].ToString();
                                    userInfo.password = reader["user_password"].ToString();
                                    userInfo.role = GetRoleName(reader["user_role"].ToString());

                                    ListofUsers.Add(userInfo);
                                }
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


        private string GetRoleName(string roleId)
        {
            // Convert role ID to role name based on your logic
            if (roleId == "1")
            {
                return "admin";
            }
            else if (roleId == "2")
            {
                return "seller";
            }
            else if (roleId == "3")
            {
                return "normal";
            }

            return "unknown";
        }
    }

    public class UserInfo
    {
        public string id;
        public string name;
        public string email;
        public string phone;
        public string address;
        public string password;
        public string role;
    }
}
