using Shoppi.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shoppi.Models
{
    public class ProductEditViewModel
    {
        public ProductEditViewModel()
        {
            Categories = new List<Category>();
        }

        [Required]
        public List<Category> Categories { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}