﻿using Shoppi.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Shoppi.Models
{
    public class ProductCreateViewModel
    {
        public ProductCreateViewModel(List<Category> categories)
        {
            Categories = categories.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();
        }

        public ProductCreateViewModel()
        {
            Categories = new List<SelectListItem>();
        }

        [Required]
        public List<SelectListItem> Categories { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The quantity must be greater than or equal to 0.")]
        public int Quantity { get; set; }
    }
}