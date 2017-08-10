﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Implementation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppi.Tests.Logic
{
    [TestClass]
    public class CartServicesTests
    {
        private CartServices _services;
        private Mock<ICartRepository> _mockRepository;
        private Mock<IProductServices> _mockProductServices;
        private Cart _cart;
        private List<Product> _products;

        [TestInitialize]
        public void TestInitialize()
        {
            _cart = new Cart();
            _products = new List<Product>();

            _mockRepository = new Mock<ICartRepository>();
            SetUpMockRepository();

            _mockProductServices = new Mock<IProductServices>();
            SetUpMockProductServices();

            _services = new CartServices(_mockRepository.Object, _mockProductServices.Object);
        }

        private void SetUpMockRepository()
        {
            SetUpGetCartMethod();
            SetUpAddLineMethod();
        }

        private void SetUpGetCartMethod()
        {
            _mockRepository.Setup(x => x.GetCart()).Returns(_cart);
        }

        private void SetUpAddLineMethod()
        {
            _mockRepository.Setup(x => x.AddLine(It.IsAny<Product>()))
                           .Callback<Product>(x => _cart.Lines.Add(new CartLine() { Product = x, Quantity = 1 }));
        }

        private void SetUpMockProductServices()
        {
            _mockProductServices.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns<int>(x => Task.Run(() => _products.FirstOrDefault(p => p.Id == x)));
        }

        [TestMethod]
        public void CartServices_GetCart_ReturnsCart()
        {
            // Act
            var result = _services.GetCart();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Cart));
        }

        [TestMethod]
        public async Task CartServices_Add_WhenProductIsNotInTheCartYet_AddsNewCartLine()
        {
            // Arrange
            var id = 42;
            var product = new Product() { Id = id };
            _products.Add(product);

            // Act
            await _services.AddAsync(id);

            // Assert
            Assert.IsTrue(_cart.Lines.Count == 1);
        }

        [TestMethod]
        public async Task CartSerVices_Add_WhenProductIsInCart_IncrementQuantity()
        {
            // Arrange
            var id = 55;
            var product = new Product() { Id = id };
            var previousQuantity = 4;
            _cart.Lines.Add(new CartLine() { Product = product, Quantity = previousQuantity });

            // Act
            await _services.AddAsync(id);

            // Assert
            Assert.IsTrue(_cart.Lines.Count == 1);
            Assert.IsTrue(_cart.Lines[0].Quantity == (previousQuantity + 1));
        }
    }
}