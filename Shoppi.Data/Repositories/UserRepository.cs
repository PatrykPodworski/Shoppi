using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Shoppi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ShoppiDbContext _context;

        public UserRepository(ShoppiDbContext context)
        {
            _context = context;
        }

        public async Task<ShoppiUser> GetByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ShoppiUser> GetByIdWithDefaultAddressAsync(string id)
        {
            return await _context.Users
                .Include(u => u.DefaultAddress)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task SetDefaultAddressIdAsync(string userId, int addressId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            _context.Entry(user).State = EntityState.Modified;
            user.DefaultAddressId = addressId;
        }
    }
}