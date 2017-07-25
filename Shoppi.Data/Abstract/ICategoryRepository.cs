using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Data.Abstract
{
    public interface ICategoryRepository
    {
        void Create(Category category);

        Task<List<Category>> GetAllAsync();

        Task<Category> GetByIdAsync(int id);

        Task<List<Category>> GetSubCategoriesAsync(int id);

        Task EditAsync(Category category);

        void Delete(int id);

        Task SaveAsync();
    }
}