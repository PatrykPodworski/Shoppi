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

        public async Task Create(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new InvalidProductNameException("Product name can't be null or whitespaces.");
            }

            if (product.Quantity < 0)
            {
                throw new NegativeProductQuantityException("Product quantity can't be negative.");
            }

            _productRepository.Create(product);
            await _productRepository.SaveAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }
    }
}