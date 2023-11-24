using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TheCollection.Pages.Users
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth", Policy = "RequireAdminRole")]
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
