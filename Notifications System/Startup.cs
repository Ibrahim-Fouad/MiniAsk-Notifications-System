using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Notifications_System.Startup))]
namespace Notifications_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
