using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(XSkills.Startup))]
namespace XSkills
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
