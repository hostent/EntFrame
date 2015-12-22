using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bm.Web.Startup))]
namespace Bm.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
             
        }
    }
}
