using Shoppi.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Shoppi.Web.Models.CategoryViewModels
{
    public class CategoryEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Head Category")]
        public int? HeadCategoryId { get; set; }

        public List<SelectListItem> HeadCategoryCandidates { get; set; }

        public CategoryEditViewModel(ICollection<Category> categories)
        {
            HeadCategoryCandidates = categories.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        public CategoryEditViewModel()
        {
            HeadCategoryCandidates = new List<SelectListItem>();
        }
    }
}