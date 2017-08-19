using Shoppi.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shoppi.Web.Models.CategoryViewModels
{
    public class CategorySubCategoriesViewModel
    {
        [Required]
        public List<Category> SubCategories { get; set; }

        [Required]
        public string HeadCategoryName { get; set; }

        public CategorySubCategoriesViewModel(string headCategoryName, List<Category> subCategories)
        {
            HeadCategoryName = headCategoryName;
            SubCategories = subCategories;
        }

        public CategorySubCategoriesViewModel()
        {
            SubCategories = new List<Category>();
        }
    }
}