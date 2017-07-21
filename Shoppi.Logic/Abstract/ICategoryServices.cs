using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface ICategoryServices
    {
        Task<List<Category>> GetAllAsync();

        void Create(Category category);
    }
}