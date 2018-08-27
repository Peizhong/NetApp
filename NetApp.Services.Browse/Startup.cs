using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using NetApp.Models.Mall;
using NetApp.Repository;
using NetApp.Repository.Interfaces;
using NetApp.Services.Lib.Extensions;

namespace NetApp.Services.Browse
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var host = Configuration["host"];
            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException();
            Uri u = new Uri(host);
            ServiceEntity.IP = u.Host;
            ServiceEntity.Port = u.Port;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mysqlConnectionString = Configuration.GetConnectionString("MallDB");
            services.AddScoped(s => new MQMallRepo(mysqlConnectionString));
            services.AddScoped<IListRepo<Product>>(s => s.GetService<MQMallRepo>());
            services.AddScoped<ITreeRepo<Category>>(s => s.GetService<MQMallRepo>());

            var redisConnectionString = Configuration.GetConnectionString("Redis");
            services.AddDistributedRedisCache(opt =>
            {
                opt.InstanceName = "BrowseService";
                opt.Configuration = redisConnectionString;
            });

            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromHours(1);
                opt.Cookie.HttpOnly = true;
            });
            
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                });

            services
                .AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5050";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0", new Info { Title = "Mall API", Version = "v0", Contact = new Contact { Name = "Wang Peizhong" } });
                //c.OperationFilter<MyHeaderFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseAuthentication();

            app.UseSession();
            app.UseMvc();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v0/swagger.json", "Mall API V0");
                //serve the Swagger UI at the app's root
                c.RoutePrefix = string.Empty;
            });

            app.RegisterConsul(lifetime, new ServiceEntity
            {
                ServiceName="NetApp.Services.Browse",
                ConsulIP = "localhost",
                ConsulPort = 8500,
            });
        }

        class MyHeaderFilter : IOperationFilter
        {
            public void Apply(Operation operation, OperationFilterContext context)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<IParameter>();

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "PageInfo",
                    In = "header",
                    Type = "string",
                    Required = false // set to false if this is optional
                });
            }
        }
    }
}