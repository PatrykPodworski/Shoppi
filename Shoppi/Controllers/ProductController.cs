using Shoppi.Logic.Abstract;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Shoppi.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        public async Task<ActionResult> Index()
        {
            var model = await _productServices.GetAllAsync();
            return View(model);
        }
    }
}