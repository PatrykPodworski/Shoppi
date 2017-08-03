using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface IAddressServices
    {
        Task CreateAsync(Address address);

        Task<List<Address>> GetByUserIdAsync(string userId);

        Task<Address> GetByIdAsync(int id);

        Task DeleteAsync(int id);

        Task DeleteUserAddressAsync(string userId, int addressId);

        Task<Address> GetUserAddressByIdAsync(string userId, int addressId);

        Task EditUserAddressAsync(string userId, Address address);
    }
}