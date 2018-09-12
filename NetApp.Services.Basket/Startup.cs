using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using NetApp.Repository;
using NetApp.Services.Lib.Extensions;

namespace NetApp.Services.Basket
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
            services.AddDbContext<BasketDBContext>(opt =>
            {
                opt.UseInMemoryDatabase("BasketDBContext");
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromHours(1);
                opt.Cookie.HttpOnly = true;
            });

            services.AddMySwagger("Basket API", "v0");
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

            app.EnableMySwaggerWithUI("Basket API", "v0");

            app.RegisterConsul(Configuration, lifetime);
        }
    }
}