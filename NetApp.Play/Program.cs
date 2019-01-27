using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetApp.CeleryTask.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetApp.Common.Extensions;
using System.Text;

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
        static void DoCelery()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(AppContext.BaseDirectory))
               .AddJsonFile("appsettings.json");
            var config = builder.Build();

            var taskQueueName = "CTASK_QUEUE";
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddCeleryWorker(cfg =>
                {
                    cfg.QueueName = taskQueueName;
                })
                .AddCeleryBeater(cfg =>
                {
                    cfg.QueueName = taskQueueName;
                    cfg.OverdueMilliseconds = 1000;
                })
                .BuildServiceProvider();

            serviceProvider.ConfigCeleryWorker();
            serviceProvider.ConfigCeleryBeater();
        }

        static void DoExpression()
        {

        }

        static void DoRedis()
        {
            var redis = new Misc.Redis();
            redis.Hello();
        }

        static void Main(string[] args)
        {
            while (true)
            {
                var questions = new StringBuilder();
                questions.AppendLine("What do you want?");
                questions.AppendLine("0.Quit");
                questions.AppendLine("1.Do Redis");
                questions.AppendLine("2.Do Celery");
                questions.AppendLine("3.Do Expression");
                Console.Write(questions.ToString());
                var answer = Console.ReadLine();
                switch (answer)
                {
                    case "0":
                        return;
                    case "1":
                        DoRedis();
                        break;
                    case "3":
                        DoExpression();
                        break;
                    default:
                        break;
                }
            }
            var ass = Enumerable.Range(1, 100).Select(n => new B
            {
                CName = n.ToString()
            }).ToList();
            var order = ass.OrderByBatch("-CName,-Name");
            Console.ReadLine();
        }
    }
}