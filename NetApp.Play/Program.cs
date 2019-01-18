using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetApp.CeleryTask.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetApp.Common.Extensions;

namespace NetApp.Play
{
    // Contravariant interface.
    // 父类可以赋给子类
    interface IContravariant<in A> 
    {
        string whoareyou();
    }

    // Implementing contravariant interface.
    class Sample<A> : IContravariant<A> where A : class, new()
    {
        private List<A> _as = new List<A>();

        public Sample()
        {
            for (int n = 0; n < 3; n++)
            {
                _as.Add(new A());
            }
        }

        public string whoareyou()
        {
            foreach (var c in _as)
            {

            }
            return typeof(A).Name;
        }
    }

    class A
    {
        public virtual string Name => "A";
    }

    class B : A
    {
        public override string Name => "B";
        public string CName { get; set; } = "A_B";
    }

    class Program
    {
        static void Main(string[] args)
        {
            var ass = Enumerable.Range(1, 100).Select(n => new B
            {
                CName = n.ToString()
            }).ToList();
            var order = ass.OrderByBatch("-CName,-Name");

            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json");
            var config = builder.Build();
            
            var taskQueueName = "CTASK_QUEUE";
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddCeleryWorker(cfg=>
                {
                    cfg.QueueName = taskQueueName;
                })
                .AddCeleryBeater(cfg=>
                {
                    cfg.QueueName = taskQueueName;
                    cfg.OverdueMilliseconds = 1000;
                })
                .BuildServiceProvider();

            serviceProvider.ConfigCeleryWorker();
            serviceProvider.ConfigCeleryBeater();

            Console.ReadLine();
        }
    }
}