using AutoMapper;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Exceptions;
using Shoppi.Web.Models.ProductViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Shoppi.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        private readonly IImageServices _imageServices;
        private readonly IBrandServices _brandServices;

        public ProductController(IProductServices productServices,
                                 ICategoryServices categoryServices,
                                 IImageServices imageServices,
                                 IBrandServices brandServices)
        {
            _productServices = productServices;
            _categoryServices = categoryServices;
            _imageServices = imageServices;
            _brandServices = brandServices;
        }

        public async Task<ActionResult> Create()
        {
            var model = await CreateProductCreateViewModel();

            return View(model);
        }

        private async Task<ProductCreateViewModel> CreateProductCreateViewModel()
        {
            var categories = await _categoryServices.GetAllFinalCategoriesAsync();
            var brands = await _brandServices.GetAllAsync();

            return new ProductCreateViewModel()
            {
                Categories = ToSelectListItems(categories),
                Brands = ToSelectListItems(brands)
            };
        }

        private List<SelectListItem> ToSelectListItems(List<Category> categories)
        {
            return categories
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GetFullCategoryPathName() })
                .ToList();
        }

        private List<SelectListItem> ToSelectListItems(List<Brand> categories)
        {
            return categories
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                .ToList();
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

        private async Task<ProductIndexViewModel> CreateProductListViewModel(int? categoryId)
        {
            if (categoryId == null)
            {
                return await CreateListModelWithoutCategory();
            }
            return await CreateListModelWithCategory(categoryId.Value);
        }

        private async Task<ProductIndexViewModel> CreateListModelWithoutCategory()
        {
            var products = await _productServices.GetAllAsync();
            return new ProductIndexViewModel(products);
        }

        private async Task<ProductIndexViewModel> CreateListModelWithCategory(int categoryId)
        {
            var products = await _productServices.GetByCategoryIdAsync(categoryId);
            return new ProductIndexViewModel(products, categoryId);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var product = await _productServices.GetByIdAsync(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            var model = await CreateEditViewModel(product);

            return View(model);
        }

        private async Task<ProductEditViewModel> CreateEditViewModel(Product product)
        {
            var categories = await _categoryServices.GetAllFinalCategoriesAsync();
            var brands = await _brandServices.GetAllAsync();
            var model = Mapper.Map<ProductEditViewModel>(product);
            model.Categories = ToSelectListItems(categories);
            model.Brands = ToSelectListItems(brands);

            return model;
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

        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productServices.GetByIdAsync(id);

            if (product == null)
            {
                return HttpNotFound();
            }

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

        public async Task<ActionResult> GetImage(int id)
        {
            var product = await _productServices.GetByIdAsync(id);
            var file = await _imageServices.GetImage(Server.MapPath("~") + product.ImagePath);
            return File(file, "image/jpeg");
        }

        public async Task<ActionResult> Details(int id)
        {
            var product = await _productServices.GetByIdAsync(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<ProductDetailsViewModel>(product);

            return View(model);
        }
    }
}