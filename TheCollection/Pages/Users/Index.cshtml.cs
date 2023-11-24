using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

                if ((User.FindFirst(ClaimTypes.Role).Value == "admin"))
                    {

                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        con.Open();
                        string sqlQuery = "SELECT * FROM users";

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

                                    ListofUsers.Add(userInfo);
                                }
                            }
                        }

                    }
                }else
                {
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        con.Open();
                        int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                        Console.WriteLine("userId : " + userId);
                        string sqlQuery = "SELECT * FROM users where user_id=@userId";

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
