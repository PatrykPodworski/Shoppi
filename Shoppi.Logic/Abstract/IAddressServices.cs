using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface IAddressServices
    {
        Task<List<Address>> GetByUserIdAsync(string userId);
    }
}