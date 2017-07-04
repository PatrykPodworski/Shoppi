using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface IProductServices
    {
        Task<List<Product>> GetAllAsync();
    }
}