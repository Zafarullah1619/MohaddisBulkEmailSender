using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASE.Startup))]
namespace ASE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
