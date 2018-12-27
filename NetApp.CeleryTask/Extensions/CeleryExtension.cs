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
            var tasks = new List<CTask>();
            var asm = Assembly.GetEntryAssembly();
            foreach (var t in asm.DefinedTypes)
            {
                var methods = t.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (var m in methods)
                {
                    var cTaskAttr = m.GetCustomAttribute<SharedTaskAttribute>();
                    if (cTaskAttr != null)
                    {
                        tasks.Add(new CTask
                        {
                            Assembly = t.FullName,
                            Name = m.Name,
                            Type = m.DeclaringType,
                            Params = m.GetParameters().Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null).ToList()
                        });
                    }
                }

            }
            Console.WriteLine($"found {tasks.Count} tasks");
            tasks.ForEach(t => Console.WriteLine($"{t.Assembly}: {t.Name}"));
            return tasks;
        }

        private static void tryActivate(IServiceProvider provider, CTask task)
        {
            var instance = ActivatorUtilities.CreateInstance(provider, task.Type);
            var method = task.Type.GetMethod(task.Name);
            var res = method.Invoke(instance, task.Params?.ToArray());
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
            tasks.ForEach(t => tryActivate(services,t));
            return services;
        }
    }
}
