namespace Shoppi.Data.Migrations
{
    using Shoppi.Data.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ShoppiDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ShoppiDbContext context)
        {
            var sportCategory = new Category("Sport");

            var categories = new[]
            {
                sportCategory,
                new Category("Football", sportCategory),
                new Category("Cycling", sportCategory),
                new Category("Video games")
            };

            context.Categories.AddOrUpdate(c => c.Name, categories);

            var products = new[]
            {
                new Product("Adidas Adizero 5-Star", categories[1].Id, 12),
                new Product("Nike Inter Milan Prestige Football", categories[1].Id, 36),
                new Product("Cannondale Synapse Sm 105 5 Disc Road", categories[2].Id, 2),
                new Product("Altura Hammock Waist Short", categories[2].Id, 58),
                new Product("Altura Icarus Short Sleeve Tee", categories[2].Id, 92),
                new Product("Sealskinz Waterproof Bobble Hat", categories[2].Id, 24),
                new Product("Uncharted: The Nathan Drake Collection", categories[3].Id, 111),
                new Product("God of War Collection", categories[3].Id, 66),
                new Product("Bayonetta 2", categories[3].Id, 39)
            };

            context.Products.AddOrUpdate(p => p.Name, products);
            context.SaveChanges();
        }
    }
}