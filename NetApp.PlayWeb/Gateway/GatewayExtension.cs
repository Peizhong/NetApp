using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.PlayWeb.Gateway
{
    public static class GatewayExtension
    {
        public static void AddMyGateway(this IServiceCollection services)
        {
            var service = services.First(x => x.ServiceType == typeof(IConfiguration));
            IConfiguration configuration = (IConfiguration)service.ImplementationInstance;

            services.Configure<GatewayOption>(configuration);

            services.AddSingleton<IDownstreamSelector, DownstreamSelector>();
        }

        public static IApplicationBuilder UseMyGateway(this IApplicationBuilder app)
        {
            var builder = new PipelineBuilder();
            var entryPoint = builder.BuildPipeline(app.ApplicationServices);

            app.Use(async (context, task) =>
            {
                var pipelineContext = new PipelineContext(context);
                await entryPoint.Invoke(pipelineContext);
            });

            return app;
        }
    }
}
