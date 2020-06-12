using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CoaPedido.WebUI.Startup))]
namespace CoaPedido.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
