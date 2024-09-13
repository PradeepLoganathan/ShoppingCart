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

        // Display the checkout page
        public async Task<IActionResult> Checkout()
        {
            try
            {
                var cartClient = _httpClientFactory.CreateClient("CartApiClient");
                var cartItems = await cartClient.GetFromJsonAsync<List<CartItem>>("/cart");

                if (cartItems == null || cartItems.Count == 0)
                {
                    TempData["ErrorMessage"] = "Your cart is empty. Please add items to your cart.";
                    return RedirectToAction("Index");
                }

                var viewModel = new CheckoutViewModel
                {
                    CartItems = cartItems,
                    TotalPrice = cartItems.Sum(item => item.ProductPrice * item.Quantity)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while preparing the checkout.";
                return RedirectToAction("Index");
            }
        }

        // Handle the order placement (this is where the form submits)
        [HttpPost]
        public IActionResult PlaceOrder(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Checkout", model);
            }

            // Process payment (Dummy logic)
            bool paymentSuccessful = ProcessPayment(model.CardNumber, model.CardExpiry, model.CardCVV);

            if (!paymentSuccessful)
            {
                TempData["ErrorMessage"] = "Payment failed. Please check your card details.";
                return View("Checkout", model); // Return to checkout page on payment failure
            }

            // Payment succeeded: Proceed with order placement
            TempData["Message"] = "Thank you for your order! Your order has been placed successfully.";

            return RedirectToAction("Confirmation");
        }

        // Dummy payment processing logic
        private bool ProcessPayment(string cardNumber, string cardExpiry, string cardCVV)
        {
            // In a real application, you would call a payment gateway API here.
            // For now, this method always returns true for demonstration purposes.
            return true;
        }

        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var cartClient = _httpClientFactory.CreateClient("CartApiClient");

                // Call the API to clear the cart (assuming you have an endpoint to clear the cart)
                var response = await cartClient.DeleteAsync($"{_cartApiBaseUrl}/cart/clear"); // Example endpoint

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Your cart has been cleared.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to clear your cart.";
                }

                return RedirectToAction("Index", "Products");  // Redirect to the product listing page
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while clearing your cart.";
                return RedirectToAction("Index", "Products"); // Redirect to product listing page on error
            }
        }

        // Confirmation page after placing the order
        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
