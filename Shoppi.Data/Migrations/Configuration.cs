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
            SeedCategories();

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
            SetDefaultAddress(userId);
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

        private void SetDefaultAddress(string userId)
        {
            var addressId = _context.Addresses.FirstOrDefault(x => x.UserId == userId).Id;
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            user.DefaultAddressId = addressId;
        }

        private void SeedCategories()
        {
            SeedCategory("Women", null);
            SeedCategory("Men", null);
            SeedCategory("Kids", null);
            _context.SaveChanges();

            SeedWomenSubCategories();
            SeedMenSubCategories();
            SeedKidsSubCategories();
        }

        private void SeedCategory(string name, int? headCategoryId)
        {
            var queryResult = _context.Categories.FirstOrDefault(x => x.Name == name && x.HeadCategoryId == headCategoryId);

            if (queryResult == null)
            {
                _context.Categories.Add(new Category { Name = name, HeadCategoryId = headCategoryId });
            }
        }

        private void SeedWomenSubCategories()
        {
            var womenCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Women").Id;

            SeedCategory("Clothing", womenCategoryId);
            SeedCategory("Shoes", womenCategoryId);
            SeedCategory("Accessories", womenCategoryId);
            _context.SaveChanges();

            var womenClothingCategoryId = _context.Categories
                .FirstOrDefault(x => x.Name == "Clothing" && x.HeadCategoryId == womenCategoryId).Id;

            SeedCategory("Dresses", womenClothingCategoryId);
            SeedCategory("Tops", womenClothingCategoryId);
            SeedCategory("Trousers", womenClothingCategoryId);
            SeedCategory("Skirts", womenClothingCategoryId);
            SeedCategory("Coats", womenClothingCategoryId);

            var womenShoesCategoryId = _context.Categories
                .FirstOrDefault(x => x.Name == "Shoes" && x.HeadCategoryId == womenCategoryId).Id;

            SeedCategory("Heels", womenShoesCategoryId);
            SeedCategory("Boots", womenShoesCategoryId);
            SeedCategory("Sandals", womenShoesCategoryId);
            SeedCategory("Trainers", womenShoesCategoryId);

            var womenAccessoriesCategoryId = _context.Categories
                .FirstOrDefault(x => x.Name == "Accessories" && x.HeadCategoryId == womenCategoryId).Id;

            SeedCategory("Bags", womenAccessoriesCategoryId);
            SeedCategory("Jewellery", womenAccessoriesCategoryId);
            SeedCategory("Hats", womenAccessoriesCategoryId);
            SeedCategory("Sunglasses", womenAccessoriesCategoryId);
        }

        private void SeedMenSubCategories()
        {
            var menCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Men").Id;

            SeedCategory("Clothing", menCategoryId);
            SeedCategory("Shoes", menCategoryId);
            SeedCategory("Accessories", menCategoryId);
            _context.SaveChanges();

            var menClothingCategoryId = _context.Categories
                .FirstOrDefault(x => x.Name == "Clothing" && x.HeadCategoryId == menCategoryId).Id;

            SeedCategory("Suits", menClothingCategoryId);
            SeedCategory("Shirts", menClothingCategoryId);
            SeedCategory("Trousers", menClothingCategoryId);
            SeedCategory("T-Shirts", menClothingCategoryId);
            SeedCategory("Coats", menClothingCategoryId);

            var menShoesCategoryId = _context.Categories
                .FirstOrDefault(x => x.Name == "Shoes" && x.HeadCategoryId == menCategoryId).Id;

            SeedCategory("Oxfords", menShoesCategoryId);
            SeedCategory("Boots", menShoesCategoryId);
            SeedCategory("Sandals", menShoesCategoryId);
            SeedCategory("Trainers", menShoesCategoryId);

            var menAccessoriesCategoryId = _context.Categories
                .FirstOrDefault(x => x.Name == "Accessories" && x.HeadCategoryId == menCategoryId).Id;

            SeedCategory("Bags", menAccessoriesCategoryId);
            SeedCategory("Watches", menAccessoriesCategoryId);
            SeedCategory("Hats", menAccessoriesCategoryId);
            SeedCategory("Sunglasses", menAccessoriesCategoryId);
        }

        private void SeedKidsSubCategories()
        {
            var kidsCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Kids").Id;

            SeedCategory("Clothing", kidsCategoryId);
            SeedCategory("Shoes", kidsCategoryId);
            SeedCategory("Accessories", kidsCategoryId);
            _context.SaveChanges();

            var kidsClothingCategoryId = _context.Categories
                .FirstOrDefault(x => x.Name == "Clothing" && x.HeadCategoryId == kidsCategoryId).Id;

            SeedCategory("Dresses", kidsClothingCategoryId);
            SeedCategory("Jumpers", kidsClothingCategoryId);
            SeedCategory("Tops", kidsClothingCategoryId);
            SeedCategory("Trousers", kidsClothingCategoryId);

            var kidsShoesCategoryId = _context.Categories
                .FirstOrDefault(x => x.Name == "Shoes" && x.HeadCategoryId == kidsCategoryId).Id;

            SeedCategory("Trainers", kidsShoesCategoryId);
            SeedCategory("Boots", kidsShoesCategoryId);
            SeedCategory("Sandals", kidsShoesCategoryId);
            SeedCategory("Slippers", kidsShoesCategoryId);

            var kidsAccessoriesCategoryId = _context.Categories
                .FirstOrDefault(x => x.Name == "Accessories" && x.HeadCategoryId == kidsCategoryId).Id;

            SeedCategory("Bags", kidsAccessoriesCategoryId);
            SeedCategory("Scarves", kidsAccessoriesCategoryId);
            SeedCategory("Hats", kidsAccessoriesCategoryId);
        }
    }
}