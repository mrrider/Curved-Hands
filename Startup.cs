using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TTP_Project.Startup))]
namespace TTP_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
