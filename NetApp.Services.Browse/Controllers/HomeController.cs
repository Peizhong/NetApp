using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace NetApp.Services.Browse.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Health()
        {
            return Request.Host.Value;
        }

        [Authorize]
        [HttpGet]
        public string Authorized()
        {
            return Request.Path;
        }
    }
}