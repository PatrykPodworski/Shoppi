﻿using System.Collections.Generic;

namespace Shoppi.Web.Models.CartViewModels
{
    public class CartIndexViewModel
    {
        public List<CartLineViewModel> Lines { get; set; }
    }

    public class CartLineViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}