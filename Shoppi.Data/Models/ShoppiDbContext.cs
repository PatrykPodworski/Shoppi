using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Shoppi.Data.Models
{
    public class ShoppiDbContext : IdentityDbContext<ShoppiUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}