using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DotNetOpen.PrerenderModule.Mvc.Startup))]
namespace DotNetOpen.PrerenderModule.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
