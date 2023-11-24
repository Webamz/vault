using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TheCollection.Pages
{
    public class CartModel : PageModel
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public void OnGet()
        {
            // Retrieve cart items logic goes here
            // Example: CartItems = GetCartItems();
            CartItem cart = new CartItem();
            cart.Name = "Hello";
            cart.Price = 1000;
            CartItems.Add(cart);
        }


    }

    public class CartItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        // Add other necessary properties
    }
}
