using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Shoppi.Web.Models.CategoryViewModels
{
    public class CategoryCreateViewModel
    {
        public List<SelectListItem> HeadCategoryCandidates { get; set; }
        public string Name { get; set; }
        public int? HeadCategoryId { get; set; }

        public CategoryCreateViewModel(List<Category> categories)
        {
            HeadCategoryCandidates = categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        public CategoryCreateViewModel()
        {
            HeadCategoryCandidates = new List<SelectListItem>();
        }
    }
}