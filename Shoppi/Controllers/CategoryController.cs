using AutoMapper;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Exceptions;
using Shoppi.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Shoppi.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        public async Task<ActionResult> List()
        {
            var categories = await _categoryServices.GetAllAsync();
            var model = new CategoryListViewModel(categories);
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var categories = await _categoryServices.GetAllAsync();
            var model = new CategoryCreateViewModel(categories);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = Mapper.Map<Category>(model);

            try
            {
                await _categoryServices.CreateAsync(category);
            }
            catch (CategoryValidationException e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

            return RedirectToAction("List");
        }
    }
}