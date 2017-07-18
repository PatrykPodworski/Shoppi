using Shoppi.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shoppi.Models
{
    public class ProductListViewModel
    {
        public ProductListViewModel()
        {
            Products = new List<Product>();
        }

        public ProductListViewModel(List<Product> products)
        {
            Products = products;
        }

        public ProductListViewModel(List<Product> products, int categoryId)
        {
            Products = products;
            ChosenCategoryId = categoryId;
        }

        [Required]
        public List<Product> Products { get; set; }

        public int? ChosenCategoryId { get; set; }
    }
}