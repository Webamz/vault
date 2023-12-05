using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, userRole)
        };

        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = credential.rememberMe
        };

        await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);
        return RedirectToPage("/products/index");
    }

    // Show an alert message for incorrect credentials
    ViewData["ErrorMessage"] = "Invalid username or password. Please try again.";

    return Page();
}


        private (bool, string, string) IsValidUser(string username, string password)
        {
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
                            string storedPassword = reader["user_password"].ToString();
                            if (VerifyPassword(password, storedPassword))
                            {
                                string role_name = GetRoleName(reader["user_role"].ToString());
                                string userId = reader["user_id"].ToString();

                                return (true, role_name, userId);
                            }
                        }
                    }
                }
            }

            return (false, null, null);
        }

        private bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] enteredPasswordBytes = Encoding.UTF8.GetBytes(enteredPassword);
                byte[] hashedBytes = sha256.ComputeHash(enteredPasswordBytes);
                string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return storedHashedPassword == hashedPassword;
            }
        }

        private string GetRoleName(string roleId)
        {
            switch (roleId)
            {
                case "1":
                    return "admin";
                case "2":
                    return "seller";
                case "3":
                    return "normal";
                default:
                    return "";
            }
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
