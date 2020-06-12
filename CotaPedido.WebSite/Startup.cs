using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CotaPedido.WebSite.Startup))]
namespace CotaPedido.WebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
