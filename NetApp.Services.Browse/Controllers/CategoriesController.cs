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
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : TreeController<Category>
    {
        public CategoriesController(ILogger<CategoriesController> logger, ITreeRepo<Category> repo)
            : base(logger, repo)
        {
        }
    }
}
