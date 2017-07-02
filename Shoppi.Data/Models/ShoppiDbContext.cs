using System.Data.Entity;

namespace Shoppi.Data.Models
{
    public class ShoppiDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}