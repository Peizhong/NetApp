using Microsoft.AspNetCore.Mvc;
using NetApp.Entities.Mall;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetApp.Controllers
{
    public class MallController : Controller
    {
        private readonly HttpClient _httpClient;
        public MallController(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> Products()
        {
            var products = Enumerable.Range(1, 30).Select(n => new Product
            {
                ProductId = Guid.NewGuid().ToString(),
                ProductName = $"Test Product {n}",
                Price = 14.3m,
                Description = $"Tell me something about {n}"
            });
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla /5.0 (Windows NT 10.0; Win64; x64; rv:52.0) Gecko/20100101 Firefox/52.0");
            var response = await _httpClient.GetAsync(@"http://localhost/api/mallservice/users");
            var data = await response.Content.ReadAsStringAsync();
            if (Request.Headers["accept"] == "application/json")
            {
                return Ok(products);
            }
            return View(products);
        }
    }
}