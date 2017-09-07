using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Data.Abstract
{
    public interface IBrandRepository
    {
        Task<Brand> GetByIdAsync(int id);

        Task<List<Brand>> GetAllAsync();

        void Create(Brand brand);

        Task SaveAsync();
    }
}