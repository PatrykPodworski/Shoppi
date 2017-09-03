using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using System.Threading.Tasks;

namespace Shoppi.Logic.Implementation
{
    public class TypeServices : ITypeServices
    {
        private ITypeRepository _repository;

        public TypeServices(ITypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductType> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}