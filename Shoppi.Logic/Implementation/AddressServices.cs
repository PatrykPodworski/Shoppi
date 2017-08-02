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
            _repository.Delete(id);
            await _repository.SaveAsync();
        }
    }
}