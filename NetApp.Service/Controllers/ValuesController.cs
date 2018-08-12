using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetApp.Entities.Mall;
using NetApp.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.Service.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly MallContext _context;

        public ValuesController(ILogger<ValuesController> logger, MallContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [ApiExplorerSettings(GroupName = "v0")]
        public async Task<IEnumerable<Category>> Categories([FromQuery]Pageable pageable)
        {
            var categories = await _context.Categories.Where(c => c.IsShow == 1).OrderBy(c => c.SortNo).Skip(pageable.StartIndex).Take(pageable.PageSize).ToListAsync();
            return categories;
        }

        [HttpGet("{categoryId}")]
        [ApiExplorerSettings(GroupName = "v0")]
        public async Task<Category> Categories(string categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            return category;
        }

        [HttpGet("{parentCategoryId}")]
        [ApiExplorerSettings(GroupName = "v0")]
        public async Task<IEnumerable<Category>> ChildCategories(string parentCategoryId)
        {
            var categories = await _context.Categories.Where(c => c.ParentCategoryId == parentCategoryId && c.IsShow == 1).OrderBy(c => c.SortNo).ToListAsync();
            return categories;
        }

        [HttpGet]
        [ApiExplorerSettings(GroupName = "v0")]
        public async Task<IEnumerable<Product>> Products([FromQuery]ProductViewModel viewModel)
        {
            IEnumerable<Product> products = null;
            var categoryId = viewModel.CategoryId;
            if (!string.IsNullOrEmpty(categoryId))
            {
                products = await _context.Products.Where(p => p.CategoryId == categoryId).OrderBy(p => p.Price).Skip(viewModel.StartIndex).Take(viewModel.PageSize).ToListAsync();
            }
            else
            {
                products = await _context.Products.OrderBy(p => p.Price).Skip(viewModel.StartIndex).Take(viewModel.PageSize).ToListAsync();
            }
            return products;
        }

        [HttpGet("{productId}")]
        [ApiExplorerSettings(GroupName = "v0")]
        public async Task<Product> Products(string productId)
        {
            var product = await _context.Products.FindAsync(productId);
            return product;
        }
    }
}