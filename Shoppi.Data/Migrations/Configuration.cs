namespace Shoppi.Data.Migrations
{
    using Shoppi.Data.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Shoppi.Data.Models.ShoppiDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ShoppiDbContext context)
        {
            var sportCategory = new Category("Sport");
            context.Categories.AddOrUpdate(sportCategory);

            var footballCategory = new Category("Football", sportCategory);
            context.Categories.AddOrUpdate(footballCategory);

            var cyclingCategory = new Category("Cycling", sportCategory);
            context.Categories.AddOrUpdate(cyclingCategory);

            var videoGamesCategory = new Category("Video games");
            context.Categories.AddOrUpdate(videoGamesCategory);

            context.Products.AddOrUpdate(new Product("Adidas Adizero 5-Star", footballCategory, 12));
            context.Products.AddOrUpdate(new Product("Nike Inter Milan Prestige Football", footballCategory, 36));
            context.Products.AddOrUpdate(new Product("Cannondale Synapse Sm 105 5 Disc Road", cyclingCategory, 2));
            context.Products.AddOrUpdate(new Product("Altura Hammock Waist Short", cyclingCategory, 58));
            context.Products.AddOrUpdate(new Product("Altura Icarus Short Sleeve Tee", cyclingCategory, 92));
            context.Products.AddOrUpdate(new Product("Sealskinz Waterproof Bobble Hat", cyclingCategory, 24));
            context.Products.AddOrUpdate(new Product("Uncharted: The Nathan Drake Collection", videoGamesCategory, 111));
            context.Products.AddOrUpdate(new Product("God of War Collection", videoGamesCategory, 66));
            context.Products.AddOrUpdate(new Product("Bayonetta 2", videoGamesCategory, 39));

            context.SaveChanges();
        }
    }
}