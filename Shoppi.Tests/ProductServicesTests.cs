﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Exceptions;
using Shoppi.Logic.Implementation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppi.Tests
{
    [TestClass]
    public class ProductServicesTests
    {
        private Mock<IProductRepository> _mockRepository;
        private ProductServices _productServices;
        private List<Product> _products;

        [TestInitialize]
        public void TestInitialize()
        {
            _products = new List<Product>();
            _mockRepository = new Mock<IProductRepository>();
            _mockRepository.Setup(x => x.Create(It.IsAny<Product>()))
                            .Callback<Product>(x => _products.Add(x));
            _mockRepository.Setup(m => m.GetByName(It.IsAny<string>()))
                            .Returns<string>(x => _products.FirstOrDefault(y => y.Name == x));
            _productServices = new ProductServices(_mockRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_CreateWithNoName_ThrowsException()
        {
            // Arrange
            var product = new Product(null, 0);

            // Act
            await _productServices.CreateAsync(product);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_CreateWithWhitespacesName_ThrowsException()
        {
            // Arrange
            var product = new Product("      ", 0);

            // Act
            await _productServices.CreateAsync(product);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_CreateWithNegativeAmount_ThrowsException()
        {
            // Arrange
            var product = new Product("Product", 0, -100);

            // Act
            await _productServices.CreateAsync(product);
        }

        [TestMethod]
        public async Task ProductServices_CreateWithValidData_Passes()
        {
            // Arrange
            var productName = "Product";
            var quantity = 100;

            var product = new Product(productName, 0, quantity);

            // Act
            await _productServices.CreateAsync(product);

            // Assert
            _mockRepository.Verify(m => m.Create(It.IsAny<Product>()), Times.Once());
        }

        [TestMethod]
        public async Task ProductServices_CreateWithZeroQuantity_Passes()
        {
            // Arrange
            var productName = "Product";
            var quantity = 0;

            var product = new Product(productName, 0, quantity);

            // Act
            await _productServices.CreateAsync(product);

            // Assert
            _mockRepository.Verify(m => m.Create(It.IsAny<Product>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_CreateWithUnuniqueName_ThrowsException()
        {
            // Arrange
            var productName = "Product";
            var quantity = 123;

            var product = new Product(productName, 0, quantity);
            _products.Add(new Product(productName, 1, 12));

            // Act
            await _productServices.CreateAsync(product);
        }
    }
}