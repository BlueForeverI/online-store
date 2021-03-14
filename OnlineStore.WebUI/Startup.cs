using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineStore.WebUI.Startup))]
namespace OnlineStore.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}