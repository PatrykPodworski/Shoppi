using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface IBrandServices
    {
        Task<Brand> GetByIdAsync(int id);

        Task<List<Brand>> GetAllAsync();

        Task CreateAsync(Brand brand);
    }
}