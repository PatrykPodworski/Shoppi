using AutoMapper;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            var model = new ProductCreateViewModel
            {
                Categories = categories
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var product = Mapper.Map<Product>(model);
            var createStatus = await _productServices.Create(product);

            if (createStatus)
            {
                // success view
                return RedirectToAction("Index");
            }

            // failure screen
            return View();
        }
    }
}

public class ProductCreateViewModel
{
    public List<Category> Categories { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int Quantity { get; set; }
}