using AutoMapper;
using Shoppi.Logic.Abstract;
using Shoppi.Web.Models.CartViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Shoppi.Controllers
{
    public class CartController : Controller
    {
        private ICartServices _cartServices;

        public CartController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }

        public ActionResult Index()
        {
            var cart = _cartServices.GetCart();
            var model = Mapper.Map<CartIndexViewModel>(cart);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(int id)
        {
            await _cartServices.AddAsync(id);
            return Json(new { numberOfProducts = _cartServices.GetNumberOfProducts() });
        }

        public ActionResult GetNumberOfProducts()
        {
            return Json(new { numberOfProducts = _cartServices.GetNumberOfProducts() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DecrementQuantity(int id)
        {
            var newQuantity = _cartServices.DecrementCartLineQuantity(id);
            return Json(new { quantity = newQuantity });
        }

        [HttpPost]
        public ActionResult IncrementQuantity(int id)
        {
            var newQuantity = _cartServices.IncrementCartLineQuantity(id);
            return Json(new { quantity = newQuantity });
        }

        [HttpPost]
        public void Delete(int id)
        {
            _cartServices.Delete(id);
        }
    }
}