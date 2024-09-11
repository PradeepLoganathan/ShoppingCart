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
    }
}
