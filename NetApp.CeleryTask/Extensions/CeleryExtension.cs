using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetApp.CeleryTask.Attributes;
using NetApp.CeleryTask.Models;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NetApp.CeleryTask.Extensions
{
    public static class CeleryExtension
    {

        
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

            loadRegisteredTask();
            registerServerTask(provider);
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
                celeryDbContext.Database.EnsureCreated();
            }

            var beater = provider.GetRequiredService<TaskBeater>();
            //beater.SayHi(registeredTasks.Values);
            beater.Run(TASK_QUEUE_NAME);

            //beater.Stop();
        }

        /// <summary>
        /// Dependency Injection what beater need
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCeleryBeater(this IServiceCollection services)
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
            return services;
        }
    }
}
