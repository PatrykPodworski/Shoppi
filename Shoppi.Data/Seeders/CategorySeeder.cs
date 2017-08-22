using Shoppi.Data.Models;
using System.Linq;

namespace Shoppi.Data.Seeders
{
    public class CategorySeeder
    {
        private ShoppiDbContext _context;

        public CategorySeeder(ShoppiDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            SeedCategory("Women", null);
            SeedCategory("Men", null);
            SeedCategory("Kids", null);
            _context.SaveChanges();

            SeedWomenSubCategories();
            SeedMenSubCategories();
            SeedKidsSubCategories();

            _context.SaveChanges();
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