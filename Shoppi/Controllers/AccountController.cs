using AutoMapper;
using Microsoft.AspNet.Identity;
using Shoppi.Data.Models;
using Shoppi.Models.Account;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Shoppi.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ShoppiUser> _userManager;

        public AccountController(UserManager<ShoppiUser> userManager)
        {
            _userManager = userManager;
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
    }
}