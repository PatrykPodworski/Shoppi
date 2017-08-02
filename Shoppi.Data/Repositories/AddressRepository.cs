using Shoppi.Data.Abstract;
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

        public void Delete(int id)
        {
            var address = new Address() { Id = id };
            _context.Addresses.Attach(address);
            _context.Addresses.Remove(address);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}