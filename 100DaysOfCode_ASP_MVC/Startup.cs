using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_100DaysOfCode_ASP_MVC.Startup))]
namespace _100DaysOfCode_ASP_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
