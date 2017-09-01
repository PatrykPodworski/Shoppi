using System.Collections.Generic;

namespace Shoppi.Data.Models
{
    public class Product
    {
        public Product()
        {
            TypeName = "Size";
        }

        public int Id { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImagePath { get; set; }

        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual List<ProductType> Types { get; set; }

        public string TypeName { get; set; }
    }
}