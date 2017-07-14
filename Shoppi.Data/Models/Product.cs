namespace Shoppi.Data.Models
{
    public class Product
    {
        public Product(string name, int categoryId, int quantity = 0)
        {
            Name = name;
            Quantity = quantity;
            CategoryId = categoryId;
        }

        public Product()
        {
        }

        public int Id { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}