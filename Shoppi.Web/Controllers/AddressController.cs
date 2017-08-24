using AutoMapper;
using Microsoft.AspNet.Identity;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Exceptions;
using Shoppi.Web.Models.AddressViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Shoppi.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private IAddressServices _addressServices;
        private IUserServices _userServices;

        public AddressController(IAddressServices addressServices, IUserServices userServices)
        {
            _addressServices = addressServices;
            _userServices = userServices;
        }

        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var defaultAddressId = await _userServices.GetUsersDefaultAddressIdAsync(userId);
            var userAddresses = await _addressServices.GetByUserIdAsync(userId);

            var model = new AddressIndexViewModel();
            model.Addresses.AddRange(userAddresses.Select(x => Mapper.Map<AddressIndexPart>(x)));
            model.DefaultAddressId = defaultAddressId;

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(AddressCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var address = Mapper.Map<Address>(model);
            address.UserId = User.Identity.GetUserId();

            try
            {
                await _addressServices.CreateAsync(address);
            }
            catch (AddressValidationException e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var address = await _addressServices.GetUserAddressByIdAsync(userId, id);
                var model = Mapper.Map<AddressEditViewModel>(address);
                return View(model);
            }
            catch (AddressUnauthorizedAccessException)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AddressEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userId = User.Identity.GetUserId();
                var address = Mapper.Map<Address>(model);
                await _addressServices.EditUserAddressAsync(userId, address);
                return RedirectToAction("Index");
            }
            catch (AddressValidationException e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
            catch (AddressUnauthorizedAccessException)
            {
                return HttpNotFound();
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var address = await _addressServices.GetUserAddressByIdAsync(userId, id);
                var model = Mapper.Map<AddressDeleteViewModel>(address);
                return View(model);
            }
            catch (AddressUnauthorizedAccessException)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(AddressDeleteViewModel model)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                await _addressServices.DeleteUserAddressAsync(userId, model.Id);
            }
            catch (AddressUnauthorizedAccessException)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index");
        }
    }
}