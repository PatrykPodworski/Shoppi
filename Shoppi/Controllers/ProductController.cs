using AutoMapper;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Shoppi.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;

        public ProductController(IProductServices productServices, ICategoryServices categoryServices)
        {
            _productServices = productServices;
            _categoryServices = categoryServices;
        }

        public async Task<ActionResult> Index()
        {
            var model = await _productServices.GetAllAsync();
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var categories = await _categoryServices.GetAllAsync();
            var model = new ProductCreateViewModel(categories);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = Mapper.Map<Product>(model);

            try
            {
                await _productServices.Create(product);
            }
            catch (System.Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}