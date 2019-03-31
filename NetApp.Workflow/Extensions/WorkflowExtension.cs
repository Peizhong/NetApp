using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;

namespace NetApp.Workflow.Extensions
{
    public static class WorkflowExtension
    {
        public static void AddMyWorkflow(this IServiceCollection services)
        {
            services.AddWorkflow(cfg =>
            {
                cfg.UseMongoDB(@"mongodb://192.168.3.19:27017", "workflow");
                //cfg.UseElasticsearch(new ConnectionSettings(new Uri("http://elastic:9200")), "workflows");
            });
        }

        public static void UseMyWorkflow(this IApplicationBuilder app, Action<IWorkflowHost> register)
        {
            var host = app.ApplicationServices.GetService<IWorkflowHost>();
            //host.RegisterWorkflow<TestWorkflow, MyDataClass>();
            register.Invoke(host);
            host.Start();
        }
    }
}
