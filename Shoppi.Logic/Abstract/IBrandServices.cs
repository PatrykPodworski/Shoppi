using Shoppi.Data.Models;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface IBrandServices
    {
        Task<Brand> GetByIdAsync(int id);
    }
}