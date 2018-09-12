using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Consul;
using Microsoft.Extensions.Configuration;

namespace NetApp.Services.Lib.Extensions
{
    public static class ConsulExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IConfiguration config, IApplicationLifetime lifetime)
        {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            app.Map("/health", lapp => lapp.Run(async ctx => ctx.Response.StatusCode = 200));
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

            var serviceName = config["ServiceName"];
            var consulServer = config["ConsulServer"];
            var servicHost = config["host"];
            if (string.IsNullOrEmpty(servicHost) || string.IsNullOrEmpty(consulServer) || string.IsNullOrEmpty(serviceName))
                throw new ArgumentNullException("Consul appsetting is empty");
            if (string.IsNullOrEmpty(servicHost))
                servicHost = "http://localhost:5100";
            if (!servicHost.StartsWith("http"))
                servicHost = $"http://{servicHost}";
            Uri hostUrl = new Uri(servicHost);

            var consulClient = new ConsulClient(x => x.Address = new Uri(consulServer));//请求注册的 Consul 地址
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(3),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(30),
                HTTP = $"{hostUrl.AbsoluteUri}health",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = serviceName,
                Address = hostUrl.Host,
                Port = hostUrl.Port,
                Tags = new[] { $"urlprefix-/{serviceName}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };
            try
            {
                consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            }
            catch {; }
            lifetime.ApplicationStopping.Register(() =>
            {
                Console.WriteLine("life time: application closed");
                try
                {
                    consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
                }
                catch {; }
            });
            return app;
        }
    }
}