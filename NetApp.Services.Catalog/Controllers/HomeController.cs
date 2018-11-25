using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace NetApp.Services.Catalog.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Health()
        {
            return $"{Request.Host.Value} at {DateTime.Now}";
        }

        [Authorize]
        [HttpGet]
        public IActionResult SecretService()
        {
            //var user = from c in User.Claims select new { c.Type, c.Value };
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
            //return $"oh you found it on {Request.Path} of {Request.Host.Value} at {DateTime.Now}";
        }
    }
}