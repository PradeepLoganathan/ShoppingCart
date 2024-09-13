using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShoppingCartWeb.Configuration;
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
        private readonly string _apiBaseUrl;

        public CartController(HttpClient httpClient,  IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiBaseUrl = apiSettings.Value.BaseUrl;
        }

        // Display the cart by fetching it from the API
        public async Task<IActionResult> Index()
        {
            var cartItems = await _httpClient.GetFromJsonAsync<List<CartItem>>($"{_apiBaseUrl}/cart");
            var cartTotal = cartItems?.Sum(item => item.Product.Price * item.Quantity) ?? 0;
            ViewBag.CartTotal = cartTotal;

            return View(cartItems);
        }

        // Remove an item from the cart via the API
        // Remove an item from the cart via the API
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/cart/{id}");            
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
