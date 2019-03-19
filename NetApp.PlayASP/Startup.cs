using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NetApp.PlayASP.Startup))]
namespace NetApp.PlayASP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
