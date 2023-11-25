using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TheCollection.Pages.Products;

namespace TheCollection.Pages
{
    public class OrderModel : PageModel
    {

        public List<ProductInfo> CartItems { get; set; }
        public decimal TotalPrice { get; set; }

        public IActionResult OnGet(string cartItems, decimal totalPrice)
        {
            var decodedCartItems = JsonConvert.DeserializeObject<List<ProductInfo>>(Uri.UnescapeDataString(cartItems));

            if (decodedCartItems != null)
            {
                CartItems = decodedCartItems;
                TotalPrice = totalPrice;
                return Page();
            }

            // Handle the case where the cart items couldn't be decoded
            return RedirectToPage("/Cart"); // Redirect to the cart page or handle appropriately
        }
    }

}
