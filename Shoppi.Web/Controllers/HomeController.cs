using System.Web.Mvc;

namespace Shoppi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Product");
        }
    }
}