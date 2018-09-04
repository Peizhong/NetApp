using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetApp.Common.Abstractions;
using NetApp.Common.Models;
using Microsoft.EntityFrameworkCore;
using NetApp.EventBus;
using NetApp.Repository;
using NetApp.Services.Catalog.Events;
using NetApp.Services.Lib.Extensions;
using Newtonsoft.Json;
using System;

namespace NetApp.Services.Catalog
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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mysqlConnectionString = Configuration.GetConnectionString("MallDB");

            services.AddScoped(s => new MQMallRepo(mysqlConnectionString));
            services.AddScoped<IListRepo<Product>>(s => s.GetRequiredService<MQMallRepo>());
            services.AddScoped<ITreeRepo<Category>>(s => s.GetRequiredService<MQMallRepo>());

            var redisConnectionString = Configuration.GetConnectionString("Redis");
            services.AddDistributedRedisCache(opt =>
            {
                opt.InstanceName = "CatalogService";
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

            services.AddOptions();

            services.AddMyIdentityServerAuthentication("http://localhost:5050", "api1");

            services.AddDbContext<IntegrationEventLogContext>(opt =>
            {
                opt.UseMySql(Configuration.GetConnectionString("EventLogDB"));
            });
            services.AddMyIntegrationServices(Configuration);
            services.AddMyEventBus(Configuration);
            services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();

            services.AddMySwagger("Catalog API", "v0");

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            //var content = app.ApplicationServices.GetRequiredService<IntegrationEventLogContext>();
            //content.Database.EnsureCreated();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseSession();
            app.UseMvc();

            app.EnableMySwaggerWithUI("Catalog API", "v0");

            app.RegisterConsul(lifetime, new ServiceEntity
            {
                ServiceName = "NetApp.Services.Catalog",
                ConsulIP = "localhost",
                ConsulPort = 8500,
            });
        }
    }
}