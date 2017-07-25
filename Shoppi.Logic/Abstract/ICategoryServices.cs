using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface ICategoryServices
    {
        Task CreateAsync(Category category);

        Task<List<Category>> GetAllAsync();

        Task<Category> GetByIdAsync(int id);

        Task<List<Category>> GetSubCategoriesAsync(int id);

        Task EditAsync(Category category);

        Task DeleteAsync(int id);
    }
}