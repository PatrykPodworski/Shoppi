using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Logic.Implementation
{
    public class AddressServices : IAddressServices
    {
        private IAddressRepository _repository;

        public AddressServices(IAddressRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Address>> GetByUserIdAsync(string userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }
    }
}