using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TheCollection.Pages
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth", Policy = "ManagerOnly")]

    public class HRManagerModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
