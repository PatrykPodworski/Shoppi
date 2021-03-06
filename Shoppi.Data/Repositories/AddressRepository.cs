﻿using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppi.Data.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private ShoppiDbContext _context;

        public AddressRepository(ShoppiDbContext context)
        {
            _context = context;
        }

        public void Create(Address address)
        {
            _context.Addresses.Add(address);
        }

        public Task<List<Address>> GetByUserIdAsync(string userId)
        {
            return _context.Addresses.Where(x => x.UserId == userId).ToListAsync();
        }

        public Task<Address> GetByIdAsync(int id)
        {
            return _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task DeleteAsync(int id)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);
            _context.Addresses.Remove(address);
        }

        public async Task EditAsync(Address address)
        {
            var addressToEdit = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == address.Id);
            _context.Entry(addressToEdit).State = EntityState.Modified;
            EditAddressValues(addressToEdit, address);
        }

        private void EditAddressValues(Address addressToEdit, Address editValues)
        {
            addressToEdit.Name = editValues.Name;
            addressToEdit.AddressLine = editValues.AddressLine;
            addressToEdit.City = editValues.City;
            addressToEdit.ZipCode = editValues.ZipCode;
            addressToEdit.Country = editValues.Country;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}