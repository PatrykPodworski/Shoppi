using System.ComponentModel.DataAnnotations;

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

        [Required]
        public Category Category { get; set; }

        public string Name { get; set; }

        public int Quantity { get; protected set; }
    }
}