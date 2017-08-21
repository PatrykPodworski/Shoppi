namespace Shoppi.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Shoppi.Data.Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

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

            SeedRoles();

            base.Seed(context);
        }

        private void SeedRoles()
        {
            SeedRole("Admin");
            SeedRole("User");
        }

        private void SeedRole(string name)
        {
            if (RoleDoesNotExists(name))
            {
                CreateRole(name);
            }
        }

        private bool RoleDoesNotExists(string name)
        {
            return !_context.Roles.Any(x => x.Name == name);
        }

        private void CreateRole(string name)
        {
            var roleStore = new RoleStore<IdentityRole>(_context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var adminRole = new IdentityRole { Name = name };

            roleManager.Create(adminRole);
        }
    }
}