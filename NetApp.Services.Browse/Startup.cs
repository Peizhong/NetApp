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
using NetApp.Entities.Mall;
using NetApp.Repository;
using NetApp.Repository.Interfaces;

namespace NetApp.Services.Browse
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0", new Info { Title = "Mall API", Version = "v0", Contact = new Contact { Name = "Wang Peizhong" } });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v0/swagger.json", "Mall API V0");
            });
        }
    }
}
