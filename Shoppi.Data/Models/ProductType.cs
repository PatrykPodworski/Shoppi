namespace Shoppi.Data.Models
{
    public class ProductType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public string ProductName => this.Product.Name;

        public string Label => this.Product.TypeLabel;
    }
}