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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _cartApiBaseUrl;

        public CartController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _cartApiBaseUrl = apiSettings.Value.CartApiBaseUrl;
        }

        // Display the cart by fetching it from the API
        public async Task<IActionResult> Index()
        {
            try
            {
                var cartClient = _httpClientFactory.CreateClient("CartApiClient");
                var cartItems = await cartClient.GetFromJsonAsync<List<CartItem>>("/cart");

                var cartTotal = cartItems?.Sum(item => item.ProductPrice * item.Quantity) ?? 0;
                ViewBag.CartTotal = cartTotal;

                return View(cartItems);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while fetching your cart.";
                return RedirectToAction("Index", "Home");
            }
        }



        // Remove an item from the cart via the API
        // Remove an item from the cart via the API
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            try
            {
                var cartClient = _httpClientFactory.CreateClient("CartApiClient");

                var response = await cartClient.DeleteAsync($"/cart/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");  // Refresh the cart page
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to remove the item from the cart.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["ErrorMessage"] = "An error occurred while removing the item.";
                return RedirectToAction("Index");
            }
        }


    }
}
