using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NetApp.Service.Extensions;
using NLog.Web;
using System;

namespace NetApp.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                var ps = string.Join(";", args);
                logger.Info($"start up with args: {ps}");

                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)//calls UseKestrel behind the scenes
                .UseNLog()
                .UseStartup<Startup>();

            var m = builder.GetSetting("ASPNETCORE_URLS");
            builder = builder.UseUrls(m);

            Uri u = new Uri(m);
            ServiceEntity.IP = u.Host;
            ServiceEntity.Port = u.Port;
            
            return builder;
        }
    }
}
