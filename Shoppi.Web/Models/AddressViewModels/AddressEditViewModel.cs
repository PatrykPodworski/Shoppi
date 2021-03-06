﻿using System.ComponentModel.DataAnnotations;

namespace Shoppi.Web.Models.AddressViewModels
{
    public class AddressEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address line")]
        public string AddressLine { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [Display(Name = "Zip code")]
        public string ZipCode { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}