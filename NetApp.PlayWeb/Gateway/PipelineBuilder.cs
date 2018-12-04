using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NetApp.PlayWeb.Gateway
{
    public class PipelineBuilder
    {
        public List<PipelineMiddleware> Middlewares = new List<PipelineMiddleware>();

        public void Use(PipelineMiddleware middleware)
        {
            Middlewares.Add(middleware);
        }

        public PipelineDelegate BuildPipeline(IServiceProvider serviceProvider)
        {
            //var fileConfig = serviceProvider.GetService<IOptions<GatewayOption>>();

            // 创建middleware，生成对象时输入依赖，再取其delegate
            IEnumerable<Type> middlewares = new[]
            {
                typeof(ExceptionMiddleware),
                typeof(CacheMiddleware),
                typeof(ReRouteMiddleware),
                typeof(RequestMiddleware),
                typeof(ResponseMiddleware)
            };

            PipelineDelegate entryPoint = (pctx) =>
            {
                return Task.CompletedTask;
            };

            foreach (var middlewareType in middlewares.Reverse())
            {
                var instance = (PipelineMiddleware)ActivatorUtilities.CreateInstance(serviceProvider, middlewareType, entryPoint);
                entryPoint = instance.Invoke;
            }
            
            return entryPoint;
        }
    }
}
