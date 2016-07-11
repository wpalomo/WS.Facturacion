using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Facturacion.Startup))]
namespace Facturacion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
