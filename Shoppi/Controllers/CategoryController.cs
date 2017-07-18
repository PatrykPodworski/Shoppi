using Shoppi.Logic.Abstract;
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
    }
}