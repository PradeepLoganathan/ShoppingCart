using Microsoft.AspNetCore.Mvc;
using ShoppingCartWeb.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ShoppingCartWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;

        public CartController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Display the cart by fetching it from the API
        public async Task<IActionResult> Index()
        {
            var cartItems = await _httpClient.GetFromJsonAsync<List<CartItem>>("http://localhost:5022/cart");  // Fetch cart from API
            var cartTotal = cartItems?.Sum(item => item.Product.Price * item.Quantity) ?? 0;
            ViewBag.CartTotal = cartTotal;

            return View(cartItems);
        }

        // Remove an item from the cart via the API
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5022/cart/{id}");  // API call to delete item from cart
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");  // Refresh the cart page
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error removing product from cart");
            }
        }
    }
}
