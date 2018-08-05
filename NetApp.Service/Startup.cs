using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using NetApp.Service.Models;

namespace NetApp.Service
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
            services.AddLogging();
            
            var connectionString = Configuration.GetConnectionString("MallDB");
            services.AddDbContext<MallContext>(opt => opt.UseSqlServer(connectionString));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            /*
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                try
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<MallContext>();

                    //context.Database.EnsureDeleted();
                    //var user = context.Users.FirstOrDefault();
                    //var detail = context.OrderDetails.Include(d => d.Product).Include(d => d.Order).ThenInclude(o => o.User).Where(o => o.Order.User == user).ToArray();
                    //context.Database.EnsureCreated();
                    //SeedData.Initialize(context);
                    context.Dispose();
                }
                catch (Exception ex)
                {
                    var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
            */
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
