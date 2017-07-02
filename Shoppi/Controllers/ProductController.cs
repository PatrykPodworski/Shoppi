using Shoppi.Logic.Abstract;
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

        public ActionResult Index()
        {
            var model = _productServices.GetAll();
            return View(model);
        }
    }
}