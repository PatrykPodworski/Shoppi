using System.ComponentModel.DataAnnotations;

namespace Shoppi.Models.Address
{
    public class AddressDeleteViewModel
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }
    }
}