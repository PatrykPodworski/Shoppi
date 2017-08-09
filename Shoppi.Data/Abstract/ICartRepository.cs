using Shoppi.Data.Models;

namespace Shoppi.Data.Abstract
{
    public interface ICartRepository
    {
        Cart GetCart();

        void AddLine(Product product);
    }
}