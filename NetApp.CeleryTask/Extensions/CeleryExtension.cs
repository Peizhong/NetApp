using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetApp.CeleryTask.Attributes;
using NetApp.CeleryTask.Models;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetApp.CeleryTask.Extensions
{
    public static class CeleryExtension
    {
        /// <summary>
        /// 查找当前程序集中已标记[SharedTask]的方法
        /// </summary>
        /// <returns></returns>
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
                            TaskName = cTaskAttr.TaskName ?? m.Name,
                            MethodName = m.Name,
                            TypeName = m.DeclaringType.AssemblyQualifiedName,
                            Params = m.GetParameters().Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null).ToList()
                        });
                    }
                }
            }
            Console.WriteLine($"found {tasks.Count} tasks");
            tasks.ForEach(t => Console.WriteLine($"{t.TypeName}"));
            return tasks;
        }

        /// <summary>
        /// Declaring rabbitmq queue,
        /// is idempotent - it will only be created if it doesn't exist already
        /// </summary>
        /// <param name="tasks"></param>
        private static void updateServerTask(IEnumerable<CTask> tasks)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, tasks);
                var data = stream.GetBuffer();
                using (var rStream = new MemoryStream(data))
                {
                    var rTasks = Serializer.Deserialize<IEnumerable<CTask>>(rStream);
                }
            }

            foreach (var t in tasks)
            {
                RabbitHelper.Instance.DeclareQueue(t.TaskName);
            }
        }

        private static void registerServerTask(IServiceProvider provider, IEnumerable<CTask> tasks)
        {
            foreach (var t in tasks)
            {
                RabbitHelper.Instance.RegisterQueue(t.TaskName, data =>
                {
                    Console.WriteLine($"{t.TaskName} recive {data?.Length} bytes data");
                    tryActivate(provider, t, data);
                });
            }
        }

        /// <summary>
        /// 测试用，触发任务
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="task"></param>
        private static void tryActivate(IServiceProvider provider, CTask task, byte[] data = null)
        {
            var type = Type.GetType(task.TypeName);
            var instance = ActivatorUtilities.CreateInstance(provider, type);
            var method = instance.GetType().GetMethod(task.MethodName);
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
            RabbitHelper.Instance.Init(broker);

            var tasks = loadRegisteredTask();
            tasks.ForEach(t => tryActivate(services, t));
            updateServerTask(tasks);
            registerServerTask(services, tasks);
            return services;
        }
    }
}
