using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Exceptions;
using System.Threading.Tasks;

namespace Shoppi.Logic.Implementation
{
    public class UserServices : IUserServices
    {
        private IUserRepository _repository;
        private IAddressServices _addressServices;

        public UserServices(IUserRepository userRepository, IAddressServices addressServices)
        {
            _repository = userRepository;
            _addressServices = addressServices;
        }

        public async Task SetDefaultAddressAsync(string userId, int addressId)
        {
            await CheckIfAddressBelongsToUser(userId, addressId);
            await _repository.SetDefaultAddressIdAsync(userId, addressId);
            await _repository.SaveChangesAsync();
        }

        private async Task CheckIfAddressBelongsToUser(string userId, int addressId)
        {
            if (await AddressDoesNotBelongToUser(userId, addressId))
            {
                throw new AddressUnauthorizedAccessException("Can't set address that does not belong to user.");
            }
        }

        private async Task<bool> AddressDoesNotBelongToUser(string userId, int addressId)
        {
            var temp = await _addressServices.DoesAddressBelongsToUserAsync(userId, addressId);
            return !(temp);
        }

        public async Task<ShoppiUser> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<ShoppiUser> GetByIdWithDefaultAddressAsync(string id)
        {
            return await _repository.GetByIdWithDefaultAddressAsync(id);
        }
    }
}