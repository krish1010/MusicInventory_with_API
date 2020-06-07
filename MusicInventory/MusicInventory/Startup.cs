using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MusicInventory.Startup))]
namespace MusicInventory
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
