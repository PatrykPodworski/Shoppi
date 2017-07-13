using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public void Create(Product product)
        {
            _context.Products.Add(product);
        }

        public Task<List<Product>> GetAllAsync()
        {
            return _context.Products.ToListAsync();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            return _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Product GetByName(string name)
        {
            return _context.Products.FirstOrDefault(p => p.Name == name);
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}