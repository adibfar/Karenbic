using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Karenbic.Startup))]
namespace Karenbic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
