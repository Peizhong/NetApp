using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetApp.Common.Models;
using NetApp.Common.Interfaces;
using NetApp.Services.Lib.Controllers;
using Microsoft.Extensions.Caching.Distributed;

namespace NetApp.Services.Browse.Controllers
{
    public class ProductsController : ListController<Product>
    {
        public ProductsController(ILogger<ProductsController> logger, IDistributedCache cache, IListRepo<Product> repo)
            : base(logger, cache, repo)
        {
        }
    }
}