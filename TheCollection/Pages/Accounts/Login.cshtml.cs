using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TheCollection.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public Credential credential { get; set; }

        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Verify credentials against the database
            var (isValid, userRole, userId) = IsValidUser(credential.username, credential.password);

            if (isValid)
            {
                // Security context
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, credential.username),
            new Claim(ClaimTypes.NameIdentifier, userId), // Add the user's ID as a claim
            new Claim("Department", "HR"),
            new Claim("Admin", "true"),
            new Claim("Manager", "true"),
            new Claim("SellerAdmin", "seller"),
            new Claim(ClaimTypes.Role, userRole) // Include the user role in claims
        };
                // Output claims for debugging
                foreach (var claim in claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = credential.rememberMe
                };

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);
                return RedirectToPage("/products/index");
            }

            return Page();
        }

        private (bool, string, string) IsValidUser(string username, string password)
        {
            // Retrieve user information from the database and verify the password
            string connectionString = "Data Source=.;Initial Catalog=vault_ecommerce;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT user_password, user_role, user_id FROM users WHERE user_email = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // You may need to hash the stored password and compare it with the input password
                            string storedPassword = reader["user_password"].ToString();
                            if (storedPassword == password)
                            {
                                // Return the role, user ID, along with the validation result
                                string role_name = "";
                                if (reader["user_role"].ToString() == "1")
                                {
                                    role_name = "admin";
                                }
                                else if (reader["user_role"].ToString() == "2")
                                {
                                    role_name = "seller";
                                }
                                else if (reader["user_role"].ToString() == "3")
                                {
                                    role_name = "normal";
                                }

                                string userId = reader["user_id"].ToString();

                                return (true, role_name, userId);
                            }
                        }
                    }
                }
            }

            return (false, null, null);
        }



        public class Credential
        {
            [Required]
            [Display(Name = "Email")]
            public string username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string password { get; set; }

            [Display(Name = "Remember Me")]
            public bool rememberMe { get; set; }
        }
    }
}

