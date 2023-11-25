using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheCollection.Pages.Products;

namespace TheCollection.Pages
{
    public class CartModel : PageModel
    {
        public List<ProductInfo> CartItems { get; set; }
        public decimal TotalPrice { get; set; }

        public void OnGet()
        {
            CartItems = DetailsModel.CartItems;
            CalculateTotalPrice();
        }

        public IActionResult OnPostRemoveFromCart(string productId)
        {
            CartItems = DetailsModel.CartItems;

            if (!string.IsNullOrEmpty(productId))
            {
                var productToRemove = CartItems.FirstOrDefault(p => p.id == productId);
                if (productToRemove != null)
                {
                    CartItems.Remove(productToRemove);
                    CalculateTotalPrice();
                }
            }

            // Redirect back to the cart page
            return RedirectToPage();
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = CartItems.Sum(item => decimal.Parse(item.price));
        }
    }
}
