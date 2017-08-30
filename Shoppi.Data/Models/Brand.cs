using System.Collections.Generic;

namespace Shoppi.Data.Models
{
    public class Brand
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LogoImagePath { get; set; }

        public List<Product> Products { get; set; }
    }
}