using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using System.Collections.Generic;
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

        public async Task<List<Brand>> GetAllAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public void Create(Brand brand)
        {
            _context.Brands.Add(brand);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}