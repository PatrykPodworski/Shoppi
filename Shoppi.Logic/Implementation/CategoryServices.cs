using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Logic.Implementation
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _repository;

        public CategoryServices(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Category category)
        {
            ValidateCategory(category);
            _repository.Create(category);
            await _repository.SaveAsync();
        }

        private void ValidateCategory(Category category)
        {
            if (IsInvalidCategoryName(category.Name))
            {
                throw new CategoryValidationException("Invalid category name.");
            }
        }

        private bool IsInvalidCategoryName(string name)
        {
            return string.IsNullOrWhiteSpace(name);
        }

        public async Task EditAsync(Category category)
        {
            ValidateCategory(category);
            await _repository.EditAsync(category);
            await _repository.SaveAsync();
        }
    }
}