using Shoppi.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shoppi.Models
{
    public class ProductCreateViewModel
    {
        public ProductCreateViewModel(List<Category> categories)
        {
            Categories = categories;
        }

        public ProductCreateViewModel()
        {
            Categories = new List<Category>();
        }

        public List<Category> Categories { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}