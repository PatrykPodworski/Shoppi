using System.Collections.Generic;

namespace Shoppi.Data.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; }

        public Cart()
        {
            Lines = new List<CartLine>();
        }
    }
}