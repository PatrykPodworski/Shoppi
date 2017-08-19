using Shoppi.Data.Models;
using System.Collections.Generic;

namespace Shoppi.Web.Models.CategoryViewModels
{
    public class CategoryListViewModel
    {
        public List<Category> Categories { get; set; }

        public CategoryListViewModel()
        {
            Categories = new List<Category>();
        }

        public CategoryListViewModel(List<Category> categories)
        {
            Categories = categories;
        }
    }
}