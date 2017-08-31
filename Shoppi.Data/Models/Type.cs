namespace Shoppi.Data.Models
{
    public class Type
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}