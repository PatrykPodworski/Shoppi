using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Data.Abstract
{
    public interface IAddressRepository
    {
        void Create(Address address);

        Task<List<Address>> GetByUserIdAsync(string userId);

        void Delete(int id);

        Task SaveAsync();
    }
}