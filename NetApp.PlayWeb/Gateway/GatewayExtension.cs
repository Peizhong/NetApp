using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.PlayWeb.Gateway
{
    public static class GatewayExtension
    {
        public static IApplicationBuilder UseMyGateway(this IApplicationBuilder applicationBuilder)
        {
            var builder = new PipelineBuilder();
            var entryPoint = builder.BuildPipeline(applicationBuilder.ApplicationServices);
            var context = new PipelineContext(null);
            entryPoint.Invoke(context).Wait();
            return applicationBuilder;
        }
    }
}
