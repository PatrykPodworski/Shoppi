using Shoppi.Data.Models;

namespace Shoppi.Data.Abstract
{
    public interface ICartRepository
    {
        Cart GetCart();

        void AddLine(ProductType type);

        void DeleteLine(int typeId);

        CartLine GetCartLine(int typeId);

        void IncrementCartLineQuantity(int typeId);
    }
}