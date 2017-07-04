using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Shoppi.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShoppiDbContext _context;

        public ProductRepository(ShoppiDbContext context)
        {
            _context = context;
        }

        public Task<List<Product>> GetAllAsync()
        {
            return _context.Products.ToListAsync();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            return _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}