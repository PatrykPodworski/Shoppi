using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppi.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ShoppiDbContext _context;

        public CategoryRepository(ShoppiDbContext context)
        {
            _context = context;
        }

        public void Create(Category category)
        {
            _context.Categories.Add(category);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Category>> GetSubCategoriesAsync(int id)
        {
            return await _context.Categories.Where(c => c.HeadCategoryId == id).ToListAsync();
        }

        public async Task EditAsync(Category category)
        {
            var categoryToEdit = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            _context.Entry(categoryToEdit).State = EntityState.Modified;
            EditCategoryValues(categoryToEdit, category);
        }

        private void EditCategoryValues(Category categoryToEdit, Category editValues)
        {
            categoryToEdit.Name = editValues.Name;
            categoryToEdit.HeadCategoryId = editValues.HeadCategoryId;
        }

        public void Delete(int id)
        {
            var category = new Category() { Id = id };
            _context.Categories.Attach(category);
            _context.Categories.Remove(category);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}