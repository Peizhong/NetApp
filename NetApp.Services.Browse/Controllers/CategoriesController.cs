using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetApp.Common.Models;
using NetApp.Common.Abstractions;
using NetApp.Services.Lib.Controllers;
using Microsoft.Extensions.Caching.Distributed;

namespace NetApp.Services.Browse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : TreeController<Category>
    {
        public CategoriesController(ILogger<CategoriesController> logger, IDistributedCache cache, ITreeRepo<Category> repo)
            : base(logger, cache, repo)
        {
        }
    }
}
