using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TheCollection.Pages
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth", Policy = "MustBelongToHRDepartment")]
    public class humanresourceModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
