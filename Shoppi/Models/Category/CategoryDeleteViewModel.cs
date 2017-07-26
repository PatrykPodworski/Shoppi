using System.ComponentModel.DataAnnotations;

namespace Shoppi.Models
{
    public class CategoryDeleteViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}