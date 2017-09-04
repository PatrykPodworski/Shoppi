namespace Shoppi.Data.Models
{
    public class CartLine
    {
        public ProductType Type { get; set; }

        public int Quantity { get; set; }

        public string ProductName => this.Type.ProductName;
    }
}