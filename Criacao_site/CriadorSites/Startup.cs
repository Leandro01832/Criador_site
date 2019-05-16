using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CriadorSites.Startup))]
namespace CriadorSites
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
