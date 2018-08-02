using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NetApp.OldMVC.Startup))]
namespace NetApp.OldMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
