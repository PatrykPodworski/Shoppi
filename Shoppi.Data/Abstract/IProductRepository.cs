using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Data.Abstract
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);

        Product GetByName(string name);

        Task<List<Product>> GetAllAsync();

        void Create(Product product);

        Task<int> SaveAsync();
    }
}