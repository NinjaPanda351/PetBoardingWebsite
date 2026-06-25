using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PawesomePalace.Startup))]
namespace PawesomePalace
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
