using Shoppi.Data.Models;

namespace Shoppi.Logic.Abstract
{
    public interface ICartServices
    {
        Cart GetCart();

        void Add(Product product);
    }
}