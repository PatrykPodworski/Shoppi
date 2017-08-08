using Shoppi.Data.Models;
using System.Threading.Tasks;

namespace Shoppi.Data.Abstract
{
    public interface IUserRepository
    {
        Task<ShoppiUser> GetByIdAsync(string id);

        Task<ShoppiUser> GetByIdWithDefaultAddressAsync(string id);

        Task SetDefaultAddressIdAsync(string userId, int addressId);

        Task SaveChangesAsync();
    }
}