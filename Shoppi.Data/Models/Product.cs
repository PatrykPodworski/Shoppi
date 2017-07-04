namespace Shoppi.Data.Models
{
    public class Product
    {
        public Product(string name, Category category, int quantity = 0)
        {
            Name = name;
            Quantity = quantity;
            Category = category;
        }

        private Product()
        {
        }

        public int Id { get; protected set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}