using Shoppi.Logic.Abstract;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Shoppi.Web.Controllers
{
    public class BrandController : Controller
    {
        private IBrandServices _brandServices;
        private IImageServices _imageServices;

        public BrandController(IBrandServices brandServices, IImageServices imageServices)
        {
            _brandServices = brandServices;
            _imageServices = imageServices;
        }

        public async Task<ActionResult> GetImage(int id)
        {
            var brand = await _brandServices.GetByIdAsync(id);
            var file = await _imageServices.GetImage(Server.MapPath("~") + brand.LogoImagePath);
            return File(file, "image/jpeg");
        }
    }
}