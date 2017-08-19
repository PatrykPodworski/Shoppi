namespace Shoppi.Web.Models.AccountViewModels
{
    public class MyAccountViewModel
    {
        public DefaultAddressViewModel DefaultAddress { get; set; }
    }

    public class DefaultAddressViewModel
    {
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }
}