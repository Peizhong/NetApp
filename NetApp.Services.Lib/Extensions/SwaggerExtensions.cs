using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Services.Lib.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddMySwagger(this IServiceCollection services, string name, string version)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0", new Info { Title = name, Version = version, Contact = new Contact { Name = "Wang Peizhong" } });
                //c.OperationFilter<MyHeaderFilter>();
            });
        }

        public static void EnableMySwaggerWithUI(this IApplicationBuilder app, string name, string version)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v0/swagger.json", $"{name} {version}");
                //serve the Swagger UI at the app's root
                c.RoutePrefix = string.Empty;
            });
        }
    }
}