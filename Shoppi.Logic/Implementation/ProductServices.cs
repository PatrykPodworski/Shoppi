﻿using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
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

        public Task<List<Product>> GetAllAsync()
        {
            return _productRepository.GetAllAsync();
        }
    }
}