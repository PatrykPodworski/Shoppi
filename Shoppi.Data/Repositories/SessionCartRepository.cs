using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using System.Web;

namespace Shoppi.Data.Repositories
{
    public class SessionCartRepository : ICartRepository
    {
        public Cart GetCart()
        {
            var cart = (Cart)HttpContext.Current.Session["Cart"];

            if (cart == null)
            {
                cart = new Cart();
            }

            return cart;
        }

        public void AddLine(Product product)
        {
            var cart = GetCart();
            cart.Lines.Add(new CartLine() { Product = product, Quantity = 1 });
            HttpContext.Current.Session["Cart"] = cart;
        }
    }
}