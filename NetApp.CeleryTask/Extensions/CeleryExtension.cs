using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetApp.CeleryTask.Attributes;
using NetApp.CeleryTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetApp.CeleryTask.Extensions
{
    public static class CeleryExtension
    {
        private static BeaterConfig beaterConfig = new BeaterConfig();
        private static WorkerConfig workerConfig = new WorkerConfig();

        /// <summary>
        /// collect task from rabbit then do it
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static void ConfigCeleryWorker(this IServiceProvider provider)
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var broker = configuration["celery_broker"];
            if (string.IsNullOrEmpty(broker))
            {
                broker = "localhost";
            }
            RabbitHelper.Instance.Init(broker);

            var worker = provider.GetRequiredService<TaskWorker>();
            worker.Init(workerConfig.QueueName);
        }

        private static List<CTask> loadRegisteredTask()
        {
            var registeredTasks = new List<CTask>();
            var asm = Assembly.GetEntryAssembly();
            foreach (var t in asm.DefinedTypes)
            {
                var methods = t.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach (var m in methods)
                {
                    var cTaskAttr = m.GetCustomAttribute<SharedTaskAttribute>();
                    if (cTaskAttr != null)
                    {
                        var task = new CTask
                        {
                            TaskName = cTaskAttr.TaskName ?? m.Name,
                            MethodName = m.Name,
                            TypeName = m.DeclaringType.AssemblyQualifiedName,
                            Params = m.GetParameters().Select(p => new CTaskParam
                            {
                                TypeName = p.ParameterType.AssemblyQualifiedName,
                                ParamName = p.Name,
                                Value = p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null
                            }).ToList()
                        };
                        registeredTasks.Add(task);
                    }
                }
            }
            return registeredTasks;
        }

        /// <summary>
        /// read database, then create task
        /// </summary>
        /// <param name="provider"></param>
        public static void ConfigCeleryBeater(this IServiceProvider provider)
        {
            using (var celeryDbContext = provider.GetRequiredService<CeleryDbContext>())
            {
                //celeryDbContext.Database.EnsureDeleted();
                //celeryDbContext.Database.EnsureCreated();
            }

            var beater = provider.GetRequiredService<TaskBeater>();
            //beater.SayHi(loadRegisteredTask());
            beater.Run(beaterConfig.QueueName, beaterConfig.IntervalMilliseconds);

            //beater.Stop();
        }

        public static IServiceCollection AddCeleryWorker(this IServiceCollection services, Action<WorkerConfig> config = null)
        {
            services.AddLogging(cfg =>
            {
                cfg.AddConsole();
            });
            services.AddScoped<TaskWorker>();
            config.Invoke(workerConfig);
            return services;
        }

        /// <summary>
        /// Dependency Injection what beater need
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCeleryBeater(this IServiceCollection services, Action<BeaterConfig> config = null)
        {
            services.AddDbContext<CeleryDbContext>(opt =>
            {
                opt.UseSqlite("Data Source=celery.db");
            });
            services.AddLogging(cfg =>
            {
                cfg.AddConsole();
            });
            services.AddScoped<TaskBeater>();
            config.Invoke(beaterConfig);
            return services;
        }
    }
}
