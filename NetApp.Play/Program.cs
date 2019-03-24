using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetApp.CeleryTask.Extensions;
using NetApp.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApp.Play
{
    // Contravariant interface.
    // 父类可以赋给子类
    interface IContravariant<in T> 
    {
        string whoareyou();
    }

    // Implementing contravariant interface.
    class Sample<T> : IContravariant<T> where T : class, new()
    {
        private List<T> _as = new List<T>();

        public Sample()
        {
            for (int n = 0; n < 3; n++)
            {
                _as.Add(new T());
            }
        }

        public string whoareyou()
        {
            foreach (var c in _as)
            {

            }
            return typeof(T).Name;
        }
    }


    class Program
    {
        class A
        {
            public virtual string Name => "A";
        }

        class B : A
        {
            public override string Name => "B";
            public string CName { get; set; } = "A_B";
        }

        static void DoAvmt()
        {
            Utils.MirgrateAvmt mirgrate = new Utils.MirgrateAvmt();
            //mirgrate.GetClassifies();
            mirgrate.GetDevices();
        }

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

            //serviceProvider.ConfigCeleryWorker();
            //serviceProvider.ConfigCeleryBeater();
        }

        static void DoExpression()
        {
            var ass = Enumerable.Range(1, 100).Select(n => new B
            {
                CName = n.ToString()
            }).ToList();
            var order = ass.OrderByBatch("-CName,-Name");
        }

        static void DoRedis()
        {
            var redis = new Misc.Redis();
            redis.Hello();
        }

        static void DoMySQL()
        {
            var mysq = new Book.Database();
            mysq.CallProcedure();
        }

        static void DoCPP()
        {
            using (var handler = new Book.CPPHandler("A"))
            {
                handler.GetInfo();
            }
            var handler2 = new Book.CPPHandler("B");
            var weakHandler2 = new WeakReference<Book.CPPHandler>(handler2);
            GC.Collect();
            if (weakHandler2.TryGetTarget(out var target))
            {
                target.GetInfo();
            }
            GC.Collect();
        }

        static void DoBenchmark()
        {
            var benchmark = new Book.Benchmark();
            benchmark.Attack(() => {; }, 150, 5000);
        }

        static void DoADONET()
        {
            var ado = new Utils.MirgrateAvmt();
            ado.RawCopyAsync(@"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=C:\Users\wxyz\source\repos\NetApp\NetApp.PlayASP\App_Data\aspnet-NetApp.PlayASP-20190319112700.mdf;Integrated Security=True",
                @"data source=C:/Users/wxyz/Desktop/avmt.db").GetAwaiter().GetResult();
        }

        static void Main(string[] args)
        {
            var actions = new List<Action>
            {
                DoAvmt,
                DoRedis,
                DoCelery,
                DoExpression,
                DoMySQL,
                DoCPP,
                DoADONET,
                DoBenchmark
            };
            while (true)
            {
                var questions = new StringBuilder(124);
                questions.AppendLine("do what you want");
                questions.AppendLine("0.Quit");
                int index = 1;
                actions.ForEach(a => questions.AppendLine($"{index++}.{a.Method.Name}"));
                Console.Write(questions.ToString());
                var str = Console.ReadLine();
                if (int.TryParse(str, out int selection))
                {
                    if (selection <= 0)
                    {
                        return;
                    }
                    if (selection <= actions.Count)
                    {
                        var action = actions[selection - 1];
                        action.Invoke();
                    }
                }
                Console.WriteLine("--------");
            }
        }
    }
}