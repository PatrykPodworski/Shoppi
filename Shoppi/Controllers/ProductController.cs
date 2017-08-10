using AutoMapper;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Exceptions;
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
                await _productServices.CreateAsync(product);
            }
            catch (ProductValidationException e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Index(int? id)
        {
            var model = await CreateProductListViewModel(id);
            return View(model);
        }

        private async Task<ProductListViewModel> CreateProductListViewModel(int? categoryId)
        {
            if (categoryId == null)
            {
                return await CreateListModelWithoutCategory();
            }
            return await CreateListModelWithCategory(categoryId.Value);
        }

        private async Task<ProductListViewModel> CreateListModelWithoutCategory()
        {
            var products = await _productServices.GetAllAsync();
            return new ProductListViewModel(products);
        }

        private async Task<ProductListViewModel> CreateListModelWithCategory(int categoryId)
        {
            var products = await _productServices.GetByCategoryIdAsync(categoryId);
            return new ProductListViewModel(products, categoryId);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var product = await _productServices.GetByIdAsync(id);
            var categories = await _categoryServices.GetAllAsync();
            var model = Mapper.Map<ProductEditViewModel>(product);
            model.Categories = categories;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ProductEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = Mapper.Map<Product>(model);

            try
            {
                await _productServices.EditAsync(product);
            }
            catch (ProductValidationException e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(Product product)
        {
            var model = Mapper.Map<ProductDeleteViewModel>(product);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(ProductDeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _productServices.DeleteAsync(model.Id);
            return RedirectToAction("Index");
        }
    }
}