using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Shoppi.Data.Models;

[assembly: OwinStartup(typeof(Shoppi.Startup))]

namespace Shoppi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            app.CreatePerOwinContext(() => new ShoppiDbContext());

            app.CreatePerOwinContext<UserStore<ShoppiUser>>(
                (opt, cont) => new UserStore<ShoppiUser>(cont.Get<ShoppiDbContext>()));

            app.CreatePerOwinContext<UserManager<ShoppiUser, string>>(
                (opt, cont) => new UserManager<ShoppiUser, string>(cont.Get<UserStore<ShoppiUser>>()));

            app.CreatePerOwinContext<SignInManager<ShoppiUser, string>>(
                (opt, cont) => new SignInManager<ShoppiUser, string>(cont.Get<UserManager<ShoppiUser, string>>(), cont.Authentication));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });
        }
    }
}