using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ManagingPersonalContacts.Startup))]
namespace ManagingPersonalContacts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
