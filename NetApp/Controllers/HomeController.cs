using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IdentityModel.Client;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace NetApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Index page says hello");
            return View();
        }

        [Authorize]
        public IActionResult Authorize()
        {
            //OpenID Connect middleware asks for the profile scope by default.
            return View();
        }

        public async Task<IActionResult> IdentitySample()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5050");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return null;
            }
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "api1");
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return NotFound("tokenResponse.IsError");
            }
            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync("http://192.168.1.102:5010/ClientService/home/SecretService");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                return NotFound("!response.IsSuccessStatusCode");
            }
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            return Ok(content);
        }

        public async Task OpenIDLogout()
        {
            //make a roundtrip to IdentityServer to clear the central single sign-on session
            //exact protocol steps are implemented inside the OpenID Connect middleware
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //
            base.OnActionExecuted(context);
        }
    }
}