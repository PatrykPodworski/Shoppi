using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Logic.Implementation
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryServices _categoryServices;

        public ProductServices(IProductRepository productRepository, ICategoryServices categroryServices)
        {
            _productRepository = productRepository;
            _categoryServices = categroryServices;
        }

        public async Task CreateAsync(Product product)
        {
            await ValidateProduct(product);
            _productRepository.Create(product);
            await _productRepository.SaveAsync();
        }

        private async Task ValidateProduct(Product product)
        {
            if (IsInvalidProductName(product.Name))
            {
                throw new ProductValidationException("Invalid product name: " + product.Name);
            }

            if (IsInvalidPrice(product.Price))
            {
                throw new ProductValidationException("Invalid product price: " + product.Price);
            }

            if (await IsInvalidCategory(product.CategoryId))
            {
                throw new ProductValidationException("Invalid category.");
            }
        }

        private bool IsInvalidProductName(string name)
        {
            return string.IsNullOrWhiteSpace(name);
        }

        private async Task<bool> IsInvalidCategory(int categoryId)
        {
            return !(await _categoryServices.IsFinalCategoryAsync(categoryId));
        }

        private bool IsInvalidPrice(decimal price)
        {
            return price <= 0m;
        }

        public Task<List<Product>> GetAllAsync()
        {
            return _productRepository.GetAllAsync();
        }

        public Task<List<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return _productRepository.GetByCategoryIdAsync(categoryId);
        }

        public Task<Product> GetByIdAsync(int id)
        {
            return _productRepository.GetByIdAsync(id);
        }

        public async Task EditAsync(Product product)
        {
            await ValidateProduct(product);
            await _productRepository.EditAsync(product);
            await _productRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _productRepository.Delete(id);
            await _productRepository.SaveAsync();
        }
    }
}