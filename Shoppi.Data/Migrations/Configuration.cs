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
            SeedUsers();
            SeedAddresses();

            base.Seed(context);
        }

        private void SeedRoles()
        {
            SeedRole("Admin");
            SeedRole("User");
        }

        private void SeedRole(string name)
        {
            if (RoleDoesNotExist(name))
            {
                CreateRole(name);
            }
        }

        private bool RoleDoesNotExist(string name)
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

        private void SeedUsers()
        {
            SeedUser("Admin@admin.com", "Admin");
            SeedUser("SampleUser@user.com", "User");
            SeedUser("SampleUser2@user.com", "User");
        }

        private void SeedUser(string userName, string role)
        {
            if (UserDoesNotExist(userName))
            {
                CreateUser(userName, role);
            }
        }

        private bool UserDoesNotExist(string name)
        {
            return !_context.Users.Any(x => x.UserName == name);
        }

        private void CreateUser(string userName, string role)
        {
            var userStore = new UserStore<ShoppiUser>(_context);
            var userManager = new UserManager<ShoppiUser>(userStore);
            var user = new ShoppiUser { UserName = userName, Email = userName };

            userManager.Create(user, "password");
            userManager.AddToRole(user.Id, role);
        }

        private void SeedAddresses()
        {
            SeedAddressesForUser("Admin@admin.com", 4);
            SeedAddressesForUser("SampleUser@user.com", 3);
            SeedAddressesForUser("SampleUser2@user.com", 7);
        }

        private void SeedAddressesForUser(string userName, int numberOfAddresses)
        {
            var userId = GetUserId(userName);
            CreateSampleAddresses(numberOfAddresses, userId);
        }

        private string GetUserId(string userName)
        {
            var userStore = new UserStore<ShoppiUser>(_context);
            var userManager = new UserManager<ShoppiUser>(userStore);
            return userManager.FindByName(userName).Id;
        }

        private void CreateSampleAddresses(int numberOfAddresses, string userId)
        {
            for (int i = 0; i < numberOfAddresses; i++)
            {
                _context.Addresses.AddOrUpdate(y => new { y.Name, y.UserId }, GetSampleAddress(i, userId));
            }
        }

        private Address GetSampleAddress(int addressIndex, string userId)
        {
            return new Address
            {
                Name = "Sample address " + addressIndex,
                AddressLine = "Sample address line " + addressIndex,
                City = "Sample city" + addressIndex,
                ZipCode = "Sample zip code " + addressIndex,
                Country = "Sample country " + addressIndex,
                UserId = userId
            };
        }
    }
}