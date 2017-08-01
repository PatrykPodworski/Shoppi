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

        public Task<List<Address>> GetByUserIdAsync(string userId)
        {
            return _context.Addresses.Where(x => x.UserId == userId).ToListAsync();
        }

        public void Create(Address address)
        {
            _context.Addresses.Add(address);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}