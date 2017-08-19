using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Shoppi.Web.Models.ProductViewModels
{
    public class ProductEditViewModel
    {
        public ProductEditViewModel()
        {
            Categories = new List<SelectListItem>();
        }

        [Required]
        public List<SelectListItem> Categories { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The quantity must be greater than or equal to 0.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The price must be greater than 0.")]
        public decimal Price { get; set; }
    }
}