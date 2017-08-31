using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Shoppi.Data.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private ShoppiDbContext _context;

        public BrandRepository(ShoppiDbContext context)
        {
            _context = context;
        }

        public async Task<Brand> GetByIdAsync(int id)
        {
            return await _context.Brands.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}