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
    }
}