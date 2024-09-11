using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ShoppingCartWeb.Models;

namespace ShoppingCartWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>("http://localhost:5022/products"); // URL of your API
            return View(products);
        }

        // Action to add a product to the cart
        // Action to add a product to the cart by calling the API
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            // Fetch the product details from the API
            var product = await _httpClient.GetFromJsonAsync<Product>($"http://localhost:5022/products/{id}");

            if (product == null)
            {
                return NotFound("Product not found");
            }

            // Create the CartItem object to send to the API
            var cartItem = new CartItem
            {
                ProductId = product.Id,
                Product = product,
                Quantity = 1  // Default quantity
            };

            // Post the cart item to the API's /cart endpoint
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5022/cart", cartItem);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");  // Redirect back to the products page
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error adding product to cart");
            }
        }
    }
}
