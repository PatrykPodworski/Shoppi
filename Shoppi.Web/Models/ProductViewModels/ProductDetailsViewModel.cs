using System.Collections.Generic;
using System.Web.Mvc;

namespace Shoppi.Web.Models.ProductViewModels
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public decimal Price { get; set; }

        public int BrandId { get; set; }

        public string BrandName { get; set; }

        public List<SelectListItem> Types { get; set; }

        public string TypeName { get; set; }
    }
}