using System.ComponentModel.DataAnnotations;

namespace Shoppi.Web.Models.CategoryViewModels
{
    public class CategoryDeleteViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Head category")]
        public string HeadCategoryName { get; set; }
    }
}