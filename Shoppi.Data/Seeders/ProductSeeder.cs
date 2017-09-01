using Shoppi.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Shoppi.Data.Seeders
{
    public class ProductSeeder
    {
        private ShoppiDbContext _context;

        private List<int> _brandIds;

        private Random _random;

        public ProductSeeder(ShoppiDbContext context)
        {
            _context = context;

            _random = new Random();

            _brandIds = _context.Brands.Where(x => true).Select(x => x.Id).ToList();
        }

        public void Seed()
        {
            SeedWomenProducts();
        }

        private void SeedWomenProducts()
        {
            SeedDresses();
            SeedTops();
            SeedTrousers();
            SeedSkirts();
            SeedCoats();

            SeedHeels();
            SeedBoots();
            SeedSandals();
            SeedTrainers();

            SeedBags();
            SeedJewellery();
            SeedHats();
            SeedSunglasses();
        }

        private void SeedDresses()
        {
            var dressesCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Dresses" && x.HeadCategory.HeadCategory.Name == "Women").Id;
            SeedProduct(new Product { Name = "MAMIE - Cocktail dress", CategoryId = dressesCategoryId, Price = 84.99m });
            SeedProduct(new Product { Name = "MARIAM - Summer dress", CategoryId = dressesCategoryId, Price = 79.99m });
            SeedProduct(new Product { Name = "Jumper dress - windsor wine", CategoryId = dressesCategoryId, Price = 54.99m });
        }

        private void SeedProduct(Product product)
        {
            product.BrandId = PickRandomBrandId();
            product.Types = GenerateTypes();
            _context.Products.AddOrUpdate(x => new { x.Name, x.CategoryId }, product);
        }

        private List<ProductType> GenerateTypes()
        {
            return new List<ProductType>
            {
                new ProductType {Name="XS", Quantity = GetRandomQuantity()},
                new ProductType {Name="S", Quantity = GetRandomQuantity()},
                new ProductType {Name="M", Quantity = GetRandomQuantity()},
                new ProductType {Name="L", Quantity = GetRandomQuantity()},
                new ProductType {Name="XL", Quantity = GetRandomQuantity()},
                new ProductType {Name="XXL", Quantity = GetRandomQuantity()},
            };
        }

        private int GetRandomQuantity()
        {
            return _random.Next(1, 1000);
        }

        private int PickRandomBrandId()
        {
            return _brandIds[_random.Next(_brandIds.Count)];
        }

        private void SeedTops()
        {
            var topsCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Tops" && x.HeadCategory.HeadCategory.Name == "Women").Id;
            SeedProduct(new Product { Name = "TRACY - Blouse - golden rod", CategoryId = topsCategoryId, Price = 74.99m });
            SeedProduct(new Product { Name = "VMAYLA COLD SHOULDER - eggnog", CategoryId = topsCategoryId, Price = 29.99m });
            SeedProduct(new Product { Name = "GINGHAM RUFFLE - Vest", CategoryId = topsCategoryId, Price = 19.99m });
            SeedProduct(new Product { Name = "Vest - light indigo", CategoryId = topsCategoryId, Price = 34.99m });
        }

        private void SeedTrousers()
        {
            var trousersCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Trousers" && x.HeadCategory.HeadCategory.Name == "Women").Id;
            SeedProduct(new Product { Name = "VMSEVEN - Trousers - black", CategoryId = trousersCategoryId, Price = 31.99m });
            SeedProduct(new Product { Name = "JOLIE - Trousers - cherry", CategoryId = trousersCategoryId, Price = 89.99m });
            SeedProduct(new Product { Name = "AVERY WINDOWPANE - grey", CategoryId = trousersCategoryId, Price = 104.99m });
        }

        private void SeedSkirts()
        {
            var skirtsCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Skirts" && x.HeadCategory.HeadCategory.Name == "Women").Id;
            SeedProduct(new Product { Name = "Pencil skirt - black", CategoryId = skirtsCategoryId, Price = 19.99m });
            SeedProduct(new Product { Name = "HANKY HEM - A-line skirt - mid grey", CategoryId = skirtsCategoryId, Price = 21.99m });
            SeedProduct(new Product { Name = "PEPPY - A-line skirt - black", CategoryId = skirtsCategoryId, Price = 139.99m });
        }

        private void SeedCoats()
        {
            var coatsCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Coats" && x.HeadCategory.HeadCategory.Name == "Women").Id;
            SeedProduct(new Product { Name = "VICAMA - Short coat - black", CategoryId = coatsCategoryId, Price = 119.99m });
            SeedProduct(new Product { Name = "Winter coat - black", CategoryId = coatsCategoryId, Price = 114.99m });
            SeedProduct(new Product { Name = "LION - Parka - moss green melange", CategoryId = coatsCategoryId, Price = 229.99m });
        }

        private void SeedHeels()
        {
            var heelsCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Heels" && x.HeadCategory.HeadCategory.Name == "Women").Id;
            SeedProduct(new Product { Name = "Classic heels - stone", CategoryId = heelsCategoryId, Price = 54.99m });
            SeedProduct(new Product { Name = "AUDREY - Classic heels - black", CategoryId = heelsCategoryId, Price = 174.99m });
        }

        private void SeedBoots()
        {
            var bootsCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Boots" && x.HeadCategory.HeadCategory.Name == "Women").Id;
            SeedProduct(new Product { Name = "ANGELICA - Boots - black", CategoryId = bootsCategoryId, Price = 374.99m });
            SeedProduct(new Product { Name = "MOLLY - Wellies - navy", CategoryId = bootsCategoryId, Price = 46.99m });
        }

        private void SeedSandals()
        {
            var sandalsCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Sandals" && x.HeadCategory.HeadCategory.Name == "Women").Id;
            SeedProduct(new Product { Name = "ERICA - Sandals - black", CategoryId = sandalsCategoryId, Price = 264.99m });
            SeedProduct(new Product { Name = "SIENNA - Sandals - gold", CategoryId = sandalsCategoryId, Price = 34.99m });
        }

        private void SeedTrainers()
        {
            var trainersCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Trainers" && x.HeadCategory.HeadCategory.Name == "Women").Id;
            SeedProduct(new Product { Name = "OLD SKOOL - Skate shoes - black", CategoryId = trainersCategoryId, Price = 64.99m });
            SeedProduct(new Product { Name = "CARNABY EVO - High-top trainers - white", CategoryId = trainersCategoryId, Price = 104.99m });
        }

        private void SeedBags()
        {
            var bagsCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Bags" && x.HeadCategory.HeadCategory.Name == "Women").Id;
            SeedProduct(new Product { Name = "Across body bag - metallic rose gold", CategoryId = bagsCategoryId, Price = 15.99m });
            SeedProduct(new Product { Name = "AVA - Handbag - yellow", CategoryId = bagsCategoryId, Price = 159.99m });
        }

        private void SeedJewellery()
        {
            var jewelleryCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Jewellery" && x.HeadCategory.HeadCategory.Name == "Women").Id;
            SeedProduct(new Product { Name = "PAW - Necklace - rose gold-coloured", CategoryId = jewelleryCategoryId, Price = 134.99m });
            SeedProduct(new Product { Name = "ELIN - Ring - silver-coloured", CategoryId = jewelleryCategoryId, Price = 54.99m });
        }

        private void SeedHats()
        {
            var hatsCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Hats" && x.HeadCategory.HeadCategory.Name == "Women").Id;
            SeedProduct(new Product { Name = "Hat - light grey heather", CategoryId = hatsCategoryId, Price = 29.99m });
            SeedProduct(new Product { Name = "TRUE ORIGINATORS 940 - Cap - multicolor", CategoryId = hatsCategoryId, Price = 21.99m });
        }

        private void SeedSunglasses()
        {
            var sunglassesCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Sunglasses" && x.HeadCategory.HeadCategory.Name == "Women").Id;
            SeedProduct(new Product { Name = "ERIKA - Sunglasses - schwarz", CategoryId = sunglassesCategoryId, Price = 67.99m });
            SeedProduct(new Product { Name = "NEW WAYFARER - Sunglasses - schwarz", CategoryId = sunglassesCategoryId, Price = 114.99m });
        }
    }
}