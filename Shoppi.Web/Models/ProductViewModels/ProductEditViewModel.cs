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
        public int? CategoryId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}