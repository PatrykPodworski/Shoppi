using System.ComponentModel.DataAnnotations;

namespace Shoppi.Web.Models.BrandViewModels
{
    public class BrandCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Brand Logo")]
        public string ImagePath { get; set; }

        public string ReturnUrl { get; set; }
    }
}