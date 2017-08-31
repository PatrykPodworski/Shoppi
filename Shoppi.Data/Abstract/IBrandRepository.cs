using Shoppi.Data.Models;
using System.Threading.Tasks;

namespace Shoppi.Data.Abstract
{
    public interface IBrandRepository
    {
        Task<Brand> GetByIdAsync(int id);
    }
}