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

        public ProductServices(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task CreateAsync(Product product)
        {
            ValidateProduct(product);
            _productRepository.Create(product);
            await _productRepository.SaveAsync();
        }

        private void ValidateProduct(Product product)
        {
            if (!IsValidProductName(product.Name))
            {
                throw new ProductValidationException("Invalid product name.");
            }

            if (!IsValidProductQuantity(product.Quantity))
            {
                throw new ProductValidationException("Invalid product quantity.");
            }

            if (!IsUniqueProductName(product.Name))
            {
                throw new ProductValidationException("There already is a product with a given name.");
            }
        }

        private bool IsUniqueProductName(string name)
        {
            var productFromDb = _productRepository.GetByName(name);
            return productFromDb == null;
        }

        private bool IsValidProductName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        private bool IsValidProductQuantity(int quantity)
        {
            return quantity >= 0;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }
    }
}