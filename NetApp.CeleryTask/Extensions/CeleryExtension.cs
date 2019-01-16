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
        public static string TASK_QUEUE_NAME = "CTASK_QUEUE";

        private static Dictionary<string, CTask> registeredTasks = new Dictionary<string, CTask>();

        /// <summary>
        /// 查找当前程序集中已标记[SharedTask]的方法，保存到registeredTasks
        /// </summary>
        /// <returns></returns>
        private static void loadRegisteredTask()
        {
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
                        registeredTasks.Add(task.TaskName, task);
                    }
                }
            }
            Console.WriteLine($"found {registeredTasks.Count} tasks");
            foreach(var t in registeredTasks.Values)
            {
                Console.WriteLine($"{t.TypeName}");
            }
        }
        
        private static RemoteCTask deserializeRemoteCTask(byte[] data)
        {
            try
            {
                using (var rStream = new MemoryStream(data))
                {
                    var task = Serializer.Deserialize<RemoteCTask>(rStream);
                    return task;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static void registerServerTask(IServiceProvider provider)
        {
            RabbitHelper.Instance.RegisterQueue(TASK_QUEUE_NAME, data =>
            {
                Console.WriteLine($"{TASK_QUEUE_NAME} recive {data?.Length} bytes data");
                var task = deserializeRemoteCTask(data);
                if (task != null)
                {
                    Console.WriteLine($"Start Task {task.TaskName}:[{task.Id}]");
                    var result = activateTask(provider, task);
                    Console.WriteLine($"Task [{task.Id}] excute result:[{result.Result}]");
                    return result;
                }
                return new TaskExcuteResult
                {
                    Code = "9999",
                    Message = "Deserialize remote ctask fail"
                };
            });
        }

        /// <summary>
        /// 触发任务
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="task"></param>
        private static TaskExcuteResult activateTask(IServiceProvider provider, RemoteCTask remoteTask)
        {
            if (!registeredTasks.TryGetValue(remoteTask.TaskName, out CTask task))
            {
                return new TaskExcuteResult
                {
                    Code = "9999",
                    Message = $"Task {remoteTask.TaskName} is not support by worker"
                };
            }
            var remoteParam = remoteTask.Params;
            foreach (var p in task.Params)
            {
                var ptype = Type.GetType(p.TypeName);
                var converter = TypeDescriptor.GetConverter(ptype);
                //match by name
                if (remoteParam.TryGetValue(p.ParamName, out var value))
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        p.Value = converter.ConvertFrom(value);
                    }
                }
                else
                {
                    p.Value = ptype.IsValueType ? Activator.CreateInstance(ptype) : null;
                }
            }
            var type = Type.GetType(task.TypeName);
            var instance = ActivatorUtilities.CreateInstance(provider, type);
            var method = instance.GetType().GetMethod(task.MethodName);
            var res = method.Invoke(instance, task.Params.Select(p => p.Value).ToArray());
            return new TaskExcuteResult
            {
                Code = "0000",
                Result = res,
            };
        }

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
                //celeryDbContext.Database.EnsureCreated();
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
