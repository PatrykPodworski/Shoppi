using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Models.Account;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Shoppi.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ShoppiUser, string> _userManager => HttpContext.GetOwinContext().Get<UserManager<ShoppiUser, string>>();
        private SignInManager<ShoppiUser, string> _signInManager => HttpContext.GetOwinContext().Get<SignInManager<ShoppiUser, string>>();
        private IUserServices _userServices;

        public AccountController(IUserServices userServices)
        {
            _userServices = userServices;
        }

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

            if (PasswordsAreNotTheSame(model.Password, model.PasswordConfirm))
            {
                ModelState.AddModelError("", "Passwords do not match.");
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

        private bool PasswordsAreNotTheSame(string password, string confirmation)
        {
            return !password.Equals(confirmation);
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

        [Authorize]
        [HttpPost]
        public ActionResult SignOut()
        {
            _signInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<ActionResult> MyAccount()
        {
            var model = await CreateMyAccountViewModel();
            return View(model);
        }

        private async Task<MyAccountViewModel> CreateMyAccountViewModel()
        {
            var userId = User.Identity.GetUserId();
            var user = await _userServices.GetByIdWithDefaultAddressAsync(userId);
            var model = Mapper.Map<MyAccountViewModel>(user);
            return model;
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(AccountChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (PasswordsAreNotTheSame(model.NewPassword, model.NewPasswordConfirm))
            {
                ModelState.AddModelError("", "New passwords do not match.");
                return View(model);
            }

            var result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }

            return RedirectToAction("MyAccount");
        }
    }
}