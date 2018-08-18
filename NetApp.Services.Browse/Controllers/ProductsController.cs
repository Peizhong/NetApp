using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetApp.Entities.Mall;
using NetApp.Repository.Interfaces;
using NetApp.Services.Lib.Controllers;

namespace NetApp.Services.Browse.Controllers
{
    public class ProductsController : ListController<Product>
    {
        public ProductsController(ILogger<ProductsController> logger, IListRepo<Product> repo, ITreeRepo<Category> treeRepo)
            : base(logger, repo)
        {
        }
    }
}
