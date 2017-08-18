using System.Collections.Generic;

namespace Shoppi.Models.Cart
{
    public class CartIndexViewModel
    {
        public List<CartLineViewModel> Lines { get; set; }
    }

    public class CartLineViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}