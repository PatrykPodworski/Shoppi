using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Logic.Implementation
{
    public class BrandServices : IBrandServices
    {
        private IBrandRepository _repository;

        public BrandServices(IBrandRepository repository)
        {
            _repository = repository;
        }

        public async Task<Brand> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            return result;
        }

        public async Task CreateAsync(Brand brand)
        {
            ValidateBrand(brand);
            _repository.Create(brand);
            await _repository.SaveAsync();
        }

        private void ValidateBrand(Brand brand)
        {
            if (IsInvalidName(brand.Name))
            {
                throw new BrandValidationException("Invalid brand name.");
            }
        }

        private bool IsInvalidName(string name)
        {
            return string.IsNullOrWhiteSpace(name);
        }
    }
}