using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Owin;
using Moq;
using NetApp.PlayASP.Abstract;
using NetApp.PlayASP.Concrete;
using NetApp.PlayASP.Entities;
using Owin;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(NetApp.PlayASP.Startup))]
namespace NetApp.PlayASP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // STANDARD MVC SETUP:

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // make a mock repo
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products()).Returns(new List<Product>
            {
                new Product { Name = "Football", Price = 25 },
                new Product { Name = "Surf board", Price = 179 },
                new Product { Name = "Running shoes", Price = 95 }
            });
            //builder.Register(ctx => mock.Object);
            builder.RegisterType<DPProductRepository>().As<IProductRepository>();

            // Run other optional steps, like registering model binders,
            // web abstractions, etc., then set the dependency resolver
            // to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // OWIN MVC SETUP:

            // Register the Autofac middleware FIRST, then the Autofac MVC middleware.
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            ConfigureAuth(app);
        }
    }
}
