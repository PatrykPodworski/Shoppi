using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Data.Abstract
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
    }
}