namespace Shoppi.Data.Migrations
{
    using Shoppi.Data.Models;
    using Shoppi.Data.Seeders;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ShoppiDbContext>
    {
        private ShoppiDbContext _context;

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ShoppiDbContext context)
        {
            _context = context;

            var userSeeder = new UserSeeder(context);
            userSeeder.Seed();

            var categorySeeder = new CategorySeeder(context);
            categorySeeder.Seed();

            var productSeeder = new ProductSeeder(context);
            productSeeder.Seed();

            base.Seed(context);
        }

        private void SeedProducts()
        {
        }
    }
}