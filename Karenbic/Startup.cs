using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Karenbic.Startup))]
namespace Karenbic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //var config = new HubConfiguration();
            //config.EnableJSONP = true;
            //app.MapSignalR(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.Map("/signalr", map =>
            {
                map.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    // EnableJSONP = true                    
                    // EnableDetailedErrors = true                                     
                };
                map.RunSignalR(hubConfiguration);
            }); 

            ConfigureAuth(app);
        }
    }
}
