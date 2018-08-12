using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace NetApp.Service.Pages
{
    public class HealthModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public HealthModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Message { get; private set; }

        public void OnGet()
        {
            var host = HttpContext.Request.Host.Value;
            var connectionstring = _configuration.GetConnectionString("MallDB");
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Date", DateTime.Now.ToLongTimeString());
            dict.Add("Host", host);
            dict.Add("Connection String", connectionstring);

            Message = JsonConvert.SerializeObject(dict);
        }
    }
}