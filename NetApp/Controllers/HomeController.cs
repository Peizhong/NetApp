using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IdentityModel.Client;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace NetApp.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            var u = User.Claims;
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
                return null;
            }
            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync("http://localhost:5100/api/home/Authorized");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            return Ok("aa");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //
            base.OnActionExecuted(context);
        }
    }
}