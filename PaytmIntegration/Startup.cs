using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PaytmIntegration.Startup))]
namespace PaytmIntegration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
