using Shoppi.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shoppi.Web.Models.ProductViewModels
{
    public class ProductIndexViewModel
    {
        public ICollection<Product> Products { get; set; }

        [Required]
        public int? Page { get; set; }

        [Required]
        public int? ProductsPerPage { get; set; }
    }
}