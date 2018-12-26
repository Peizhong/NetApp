using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetApp.CeleryTask.Attributes;
using NetApp.CeleryTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetApp.CeleryTask.Extensions
{
    public static class CeleryExtension
    {
        private static List<CTask> loadRegisteredTask()
        {
            var tasks= new List<CTask>();
            //var dir


            var ass = Assembly.GetEntryAssembly();
            foreach (var t in ass.DefinedTypes)
            {
                var methods = t.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
                foreach (var m in methods)
                {
                    var cTaskAttr = m.GetCustomAttribute<SharedTaskAttribute>();
                    if (cTaskAttr != null)
                    {
                        tasks.Add(new CTask
                        {
                            Assembly = t.FullName,
                            Name = m.Name,
                        });
                    }
                }

            }
            return tasks;
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
