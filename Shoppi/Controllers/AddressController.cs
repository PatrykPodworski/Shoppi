using AutoMapper;
using Microsoft.AspNet.Identity;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Exceptions;
using Shoppi.Models.Address;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Shoppi.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private IAddressServices _addressServices;

        public AddressController(IAddressServices addressServices)
        {
            _addressServices = addressServices;
        }

        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();

            var userAddresses = await _addressServices.GetByUserIdAsync(userId);

            var model = new AddressIndexViewModel();
            model.Addresses.AddRange(userAddresses.Select(x => Mapper.Map<AddressIndexPart>(x)));

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

        public async Task<ActionResult> Delete(int id)
        {
            var address = await _addressServices.GetByIdAsync(id);
            var userId = User.Identity.GetUserId();

            if (address.UserId != userId)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<AddressDeleteViewModel>(address);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(AddressDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _addressServices.DeleteAsync(model.Id);

            return RedirectToAction("Index");
        }
    }
}