using Shoppi.Data.Models;
using Shoppi.Logic.Factories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface IProductServices
    {
        Task CreateAsync(Product product);

        Task<List<Product>> GetAllAsync();

        Task<ICollection<Product>> GetAsync(ProductSpecificationFilters filters);

        Task<List<Product>> GetByCategoryIdAsync(int categoryId);

        Task<Product> GetByIdAsync(int id);

        Task EditAsync(Product product);

        Task DeleteAsync(int id);
    }
}