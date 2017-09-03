using Shoppi.Data.Models;
using System.Threading.Tasks;

namespace Shoppi.Data.Abstract
{
    public interface ITypeRepository
    {
        Task<ProductType> GetByIdAsync(int id);
    }
}