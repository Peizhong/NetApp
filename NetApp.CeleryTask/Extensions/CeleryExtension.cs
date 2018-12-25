using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetApp.CeleryTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetApp.CeleryTask.Extensions
{
    public static class CeleryExtension
    {
        private static List<CTask> loadRegisteredTask()
        {
            return new List<CTask>();
        }

        public static IServiceProvider ConfigCeleryWorker(this IServiceProvider services)
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            var broker = configuration["celery_broker"];
            if (string.IsNullOrEmpty(broker))
            {
                broker = "localhost";
            }
            var tasks = loadRegisteredTask();
            return services;
        }
    }
}
