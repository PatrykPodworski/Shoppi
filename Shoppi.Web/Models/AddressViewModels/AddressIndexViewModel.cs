using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shoppi.Web.Models.AddressViewModels
{
    public class AddressIndexViewModel
    {
        [Required]
        public List<AddressIndexPart> Addresses { get; set; }

        public AddressIndexViewModel()
        {
            Addresses = new List<AddressIndexPart>();
        }
    }

    public class AddressIndexPart
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AddressLine { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string Country { get; set; }
    }
}