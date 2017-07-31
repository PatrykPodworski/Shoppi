namespace Shoppi.Data.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public string UserId { get; set; }
        public ShoppiUser User { get; set; }
    }
}