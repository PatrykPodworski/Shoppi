using Shoppi.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shoppi.Web.Models.ProductViewModels
{
    public class ProductIndexViewModel
    {
        public ProductIndexViewModel()
        {
            Products = new List<Product>();
        }

        public ProductIndexViewModel(List<Product> products)
        {
            Products = products;
        }

        public ProductIndexViewModel(List<Product> products, int categoryId)
        {
            Products = products;
            ChosenCategoryId = categoryId;
        }

        [Required]
        public List<Product> Products { get; set; }

        public int? ChosenCategoryId { get; set; }
    }
}