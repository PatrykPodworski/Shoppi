using Shoppi.Data.Models;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface ITypeServices
    {
        Task<ProductType> GetByIdAsync(int id);
    }
}