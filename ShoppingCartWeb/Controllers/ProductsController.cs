using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ShoppingCartWeb.Models;
using Microsoft.Extensions.Options;
using ShoppingCartWeb.Configuration;

namespace ShoppingCartWeb.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient("ProductApiClient");
            var products = await httpClient.GetFromJsonAsync<List<Product>>("/products");
            return View(products);
        }

        // Action to add a product to the cart
        // Action to add a product to the cart by calling the API
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            var productClient = _httpClientFactory.CreateClient("ProductApiClient");
            var product = await productClient.GetFromJsonAsync<Product>($"/products/{id}");

            if (product == null)
            {
                TempData["Message"] = "Product not found";
                return RedirectToAction("Index");
            }

            var cartItem = new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductPrice = product.Price,
                Quantity = 1  // Default quantity
            };

            var cartClient = _httpClientFactory.CreateClient("CartApiClient");
            var response = await cartClient.PostAsJsonAsync("/cart", cartItem);

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Item added to cart!";
            }
            else
            {
                TempData["Message"] = "Failed to add item to cart.";
            }

            return RedirectToAction("Index");
        }

    }

}
