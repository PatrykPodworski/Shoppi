using Shoppi.Data.Models;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface ICartServices
    {
        Cart GetCart();

        Task AddAsync(int typeId);

        void Delete(int typeId);

        int DecrementCartLineQuantity(int typeId);

        int IncrementCartLineQuantity(int typeId);

        int GetNumberOfProducts();
    }
}