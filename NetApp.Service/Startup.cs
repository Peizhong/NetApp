using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetApp.Service.Extensions;
using NetApp.Service.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

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
            var connectionString = Configuration.GetConnectionString("MallDB");
            services.AddDbContext<MallContext>(opt => opt.UseMySql(connectionString));

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                }); ;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0", new Info { Title = "Mall API", Version = "v0", Contact = new Contact { Name = "Wang Peizhong" } });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v0/swagger.json", "Mall API V0");
            });


            ServiceEntity serviceEntity = new ServiceEntity
            {
                ServiceName = "NetApp_Service",
                ConsulIP = "127.0.0.1",
                ConsulPort = 8500
            };
            app.RegisterConsul(lifetime, serviceEntity);
        }
    }
}
