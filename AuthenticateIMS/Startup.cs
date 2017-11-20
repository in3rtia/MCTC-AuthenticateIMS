using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AuthenticateIMS.Startup))]
namespace AuthenticateIMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
