using Shoppi.Data.Models;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface ICartServices
    {
        Cart GetCart();

        Task AddAsync(int productId);

        int Remove(int productId);
    }
}