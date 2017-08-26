using Shoppi.Data.Models;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface ICartServices
    {
        Cart GetCart();

        Task AddAsync(int productId);

        void Remove(int productId);

        int DecrementProductQuantity(int productId);

        int IncrementProductQuantity(int productId);
    }
}