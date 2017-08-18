using System.ComponentModel.DataAnnotations;

namespace Shoppi.Models.Address
{
    public class AddressCreateViewModel
    {
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
    }
}