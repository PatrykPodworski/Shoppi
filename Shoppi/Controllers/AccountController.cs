using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Shoppi.Data.Models;
using Shoppi.Models.Account;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Shoppi.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ShoppiUser> _userManager => HttpContext.GetOwinContext().Get<UserManager<ShoppiUser>>();
        private SignInManager<ShoppiUser, string> _signInManager => HttpContext.GetOwinContext().Get<SignInManager<ShoppiUser, string>>();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(AccountRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = Mapper.Map<ShoppiUser>(model);
            user.UserName = model.Email;

            var identityResult = await _userManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }

            return RedirectToAction("List", "Product");
        }

        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(AccountSignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

            if (result == SignInStatus.Failure)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View(model);
            }
            else if (result == SignInStatus.LockedOut)
            {
                return View("Lockout");
            }

            return RedirectToAction("List", "Product");
        }
    }
}