﻿using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Exceptions;
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

        public async Task CreateAsync(Address address)
        {
            ValidateAddress(address);
            _repository.Create(address);
            await _repository.SaveAsync();
        }

        private void ValidateAddress(Address address)
        {
            if (string.IsNullOrWhiteSpace(address.Name))
            {
                throw new AddressValidationException("Address name can't be empty.");
            }

            if (string.IsNullOrWhiteSpace(address.AddressLine))
            {
                throw new AddressValidationException("Address line can't be empty.");
            }

            if (string.IsNullOrWhiteSpace(address.City))
            {
                throw new AddressValidationException("City can't be empty.");
            }
            if (string.IsNullOrWhiteSpace(address.Country))
            {
                throw new AddressValidationException("Country can't be empty.");
            }

            if (string.IsNullOrWhiteSpace(address.ZipCode))
            {
                throw new AddressValidationException("Zip code can't be empty.");
            }

            if (address.UserId == null)
            {
                throw new AddressValidationException("Address must have an user.");
            }
        }

        public async Task<List<Address>> GetByUserIdAsync(string userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task<Address> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();
        }

        public async Task DeleteUserAddressAsync(string userId, int addressId)
        {
            await ReturnAddressIfItBelongsToUser(userId, addressId);
            await _repository.DeleteAsync(addressId);
            await _repository.SaveAsync();
        }

        private async Task<Address> ReturnAddressIfItBelongsToUser(string userId, int addressId)
        {
            var address = await _repository.GetByIdAsync(addressId);

            if (address == null)
            {
                throw new AddressUnauthorizedAccessException("There is no address with given id.");
            }

            if (address.UserId != userId)
            {
                throw new AddressUnauthorizedAccessException("Address does not belong to given user.");
            }

            return address;
        }

        public async Task<Address> GetUserAddressByIdAsync(string userId, int addressId)
        {
            return await ReturnAddressIfItBelongsToUser(userId, addressId);
        }

        public async Task EditUserAddressAsync(string userId, Address address)
        {
            ValidateAddress(address);
            await ReturnAddressIfItBelongsToUser(userId, address.Id);
            await _repository.EditAsync(address);
            await _repository.SaveAsync();
        }

        public async Task<bool> DoesAddressBelongsToUserAsync(string userId, int addressId)
        {
            try
            {
                await ReturnAddressIfItBelongsToUser(userId, addressId);
            }
            catch (AddressUnauthorizedAccessException)
            {
                return false;
            }
            return true;
        }
    }
}