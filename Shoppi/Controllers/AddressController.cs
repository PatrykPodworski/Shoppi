using AutoMapper;
using Microsoft.AspNet.Identity;
using Shoppi.Logic.Abstract;
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
    }
}