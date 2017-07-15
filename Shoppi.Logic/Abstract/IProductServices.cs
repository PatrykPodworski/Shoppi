using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface IProductServices
    {
        Task<List<Product>> GetAllAsync();

        Task CreateAsync(Product product);

        Task EditAsync(Product product);

        Task DeleteAsync(int id);
    }
}