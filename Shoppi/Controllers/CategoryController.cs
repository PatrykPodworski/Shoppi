using AutoMapper;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Exceptions;
using Shoppi.Models;
using System.Linq;
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

        public async Task<ActionResult> Edit(int id)
        {
            var category = await _categoryServices.GetByIdAsync(id);
            var headCategoryCandidates = await _categoryServices.GetAllAsync();
            var model = Mapper.Map<CategoryEditViewModel>(category);
            model.HeadCategoryCandidates = headCategoryCandidates
                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = Mapper.Map<Category>(model);

            try
            {
                await _categoryServices.EditAsync(category);
            }
            catch (CategoryValidationException e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

            return RedirectToAction("List");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var subCategories = await _categoryServices.GetSubCategoriesAsync(id);
            var category = await _categoryServices.GetByIdAsync(id);

            if (subCategories.Count > 0)
            {
                var model = new CategorySubCategoriesViewModel(category.Name, subCategories);
                return View("SubCategories", model);
            }
            else
            {
                var model = Mapper.Map<CategoryDeleteViewModel>(category);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(CategoryDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _categoryServices.DeleteAsync(model.Id);

            return RedirectToAction("List");
        }
    }
}