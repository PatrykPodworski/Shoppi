﻿using System.ComponentModel.DataAnnotations;

namespace Shoppi.Models
{
    public class ProductDeleteViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}