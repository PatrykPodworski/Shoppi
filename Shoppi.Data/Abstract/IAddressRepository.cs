using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Data.Abstract
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetByUserIdAsync(string userId);
    }
}