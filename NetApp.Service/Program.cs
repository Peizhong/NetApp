using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetApp.Service.Extensions;

namespace NetApp.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            if (args?.Any() == true)
            {
                Regex regexHost = new Regex(@"\d{2,4}");
                foreach (var a in args)
                {
                    var p = regexHost.Match(a);
                    if (p != null)
                    {
                        Console.WriteLine($"found the port is : [{p.Value}]");
                        ServiceEntity.Port = int.Parse(p.Value);
                    }
               }
            }
            var builder = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
            //.UseUrls("http://*:5001");
            return builder;
        }
    }
}
