using Shoppi.Data.Models;

namespace Shoppi.Data.Seeders
{
    public class ProductSeeder
    {
        private ShoppiDbContext _context;

        public ProductSeeder(ShoppiDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
        }
    }
}