using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using System.Linq;
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

        public void AddLine(ProductType type)
        {
            var cart = GetCart();

            var cartLine = new CartLine { Type = type, Quantity = 1 };

            cart.Lines.Add(cartLine);
            HttpContext.Current.Session["Cart"] = cart;
        }

        public void DeleteLine(int typeId)
        {
            GetCart()
                .Lines
                .RemoveAll(x => x.Type.Id == typeId);
        }

        public CartLine GetCartLine(int typeId)
        {
            return GetCart()
                .Lines
                .FirstOrDefault(x => x.Type.Id == typeId);
        }

        public void IncrementCartLineQuantity(int typeId)
        {
            GetCart()
                .Lines
                .FirstOrDefault(x => x.Type.Id == typeId)
                .Quantity++;
        }
    }
}