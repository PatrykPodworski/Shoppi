using Shoppi.Data.Models;
using System.Data.Entity.Migrations;

namespace Shoppi.Data.Seeders
{
    public class BrandSeeder
    {
        private ShoppiDbContext _context;

        public BrandSeeder(ShoppiDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            SeedBrands();
            _context.SaveChanges();
        }

        private void SeedBrands()
        {
            _context.Brands.AddOrUpdate(x => x.Name, new Brand { Name = "Calvin Klein", LogoImagePath = "\\images\\brands\\CalvinKlein.jpg" });
            _context.Brands.AddOrUpdate(x => x.Name, new Brand { Name = "Zign", LogoImagePath = "\\images\\brands\\Zign.jpg" });
            _context.Brands.AddOrUpdate(x => x.Name, new Brand { Name = "Pepe Jeans", LogoImagePath = "\\images\\brands\\PepeJeans.jpg" });
            _context.Brands.AddOrUpdate(x => x.Name, new Brand { Name = "Adidas", LogoImagePath = "\\images\\brands\\Adidas.jpg" });
            _context.Brands.AddOrUpdate(x => x.Name, new Brand { Name = "Nike", LogoImagePath = "\\images\\brands\\Nike.jpg" });
        }
    }
}