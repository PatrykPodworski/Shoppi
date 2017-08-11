﻿using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppi.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShoppiDbContext _context;

        public ProductRepository(ShoppiDbContext context)
        {
            _context = context;
        }

        public void Create(Product product)
        {
            _context.Products.Add(product);
        }

        public async Task EditAsync(Product product)
        {
            var productToEdit = await _context.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
            _context.Entry(productToEdit).State = EntityState.Modified;
            EditProductValues(productToEdit, product);
        }

        private void EditProductValues(Product toEdit, Product editValues)
        {
            toEdit.Name = editValues.Name;
            toEdit.Quantity = editValues.Quantity;
            toEdit.CategoryId = editValues.CategoryId;
        }

        public void Delete(int id)
        {
            var product = new Product() { Id = id };
            _context.Products.Attach(product);
            _context.Products.Remove(product);
        }

        public Task<List<Product>> GetAllAsync()
        {
            return _context.Products.ToListAsync();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            return _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<List<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return _context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
        }
    }
}