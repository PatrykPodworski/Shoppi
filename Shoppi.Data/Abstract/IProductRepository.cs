using Shoppi.Data.Models;
using Shoppi.Data.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Data.Abstract
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);

        Task<Product> GetByNameAsync(string name);

        Task<List<Product>> GetAllAsync();

        Task<ICollection<Product>> GetAsync(Specification<Product> specification);

        Task<List<Product>> GetByCategoryIdAsync(int categoryId);

        void Create(Product product);

        Task EditAsync(Product product);

        void Delete(int idv);

        Task SaveAsync();
    }
}