using System.ComponentModel.DataAnnotations;

namespace Shoppi.Web.Models.ProductViewModels
{
    public class ProductDeleteViewModel
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Brand")]
        public string BrandName { get; set; }

        public decimal Price { get; set; }
    }
}