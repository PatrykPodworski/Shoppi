using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Shoppi.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShoppiDbContext _context;

        public ProductRepository(ShoppiDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}