using AutoMapper;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Exceptions;
using Shoppi.Web.Models.BrandViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Shoppi.Web.Controllers
{
    public class BrandController : Controller
    {
        private IBrandServices _brandServices;
        private IImageServices _imageServices;

        public BrandController(IBrandServices brandServices, IImageServices imageServices)
        {
            _brandServices = brandServices;
            _imageServices = imageServices;
        }

        public async Task<ActionResult> GetImage(int id)
        {
            var brand = await _brandServices.GetByIdAsync(id);
            var file = await _imageServices.GetImage(Server.MapPath("~") + brand.LogoImagePath);
            return File(file, "image/jpeg");
        }

        public ActionResult Create(string returnUrl)
        {
            var model = new BrandCreateViewModel();

            if (returnUrl == null)
            {
                model.ReturnUrl = Url.Action("Index", "Product");
            }
            else
            {
                model.ReturnUrl = returnUrl;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(BrandCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var brand = Mapper.Map<Brand>(model);

            try
            {
                await _brandServices.CreateAsync(brand);
            }
            catch (BrandValidationException e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

            return Redirect(model.ReturnUrl);
        }
    }
}