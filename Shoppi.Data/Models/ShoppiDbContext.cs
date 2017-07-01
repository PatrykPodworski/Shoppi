using System.Data.Entity;

namespace Shoppi.Data.Models
{
    public class ShoppiDbContext : DbContext
    {
        public ShoppiDbContext() : base("ShoppiConnectionString")
        {
            Database.SetInitializer(new ShoppiDbInitializer());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }

    public class ShoppiDbInitializer : DropCreateDatabaseAlways<ShoppiDbContext>
    {
        protected override void Seed(ShoppiDbContext context)
        {
            var sportCategory = new Category("Sport");
            context.Categories.Add(sportCategory);

            var footballCategory = new Category("Football", sportCategory);
            context.Categories.Add(footballCategory);

            var cyclingCategory = new Category("Cycling", sportCategory);
            context.Categories.Add(cyclingCategory);

            var videoGamesCategory = new Category("Video games");
            context.Categories.Add(videoGamesCategory);

            context.Products.Add(new Product("Adidas Adizero 5-Star", footballCategory, 12));
            context.Products.Add(new Product("Nike Inter Milan Prestige Football", footballCategory, 36));
            context.Products.Add(new Product("Cannondale Synapse Sm 105 5 Disc Road", cyclingCategory, 2));
            context.Products.Add(new Product("Altura Hammock Waist Short", cyclingCategory, 58));
            context.Products.Add(new Product("Altura Icarus Short Sleeve Tee", cyclingCategory, 92));
            context.Products.Add(new Product("Sealskinz Waterproof Bobble Hat", cyclingCategory, 24));
            context.Products.Add(new Product("Uncharted: The Nathan Drake Collection", videoGamesCategory, 111));
            context.Products.Add(new Product("God of War Collection", videoGamesCategory, 66));
            context.Products.Add(new Product("Bayonetta 2", videoGamesCategory, 39));

            context.SaveChangesAsync();

            base.Seed(context);
        }
    }
}