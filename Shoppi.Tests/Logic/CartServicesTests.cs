using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Implementation;

namespace Shoppi.Tests.Logic
{
    [TestClass]
    public class CartServicesTests
    {
        private CartServices _services;
        private Mock<ICartRepository> _mockRepository;
        private Cart _cart;

        [TestInitialize]
        public void TestInitialize()
        {
            _cart = new Cart();
            _mockRepository = new Mock<ICartRepository>();
            SetUpMockRepository();
            _services = new CartServices(_mockRepository.Object);
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
        public void CartServices_Add_WhenProductIsNotInTheCartYet_AddsNewCartLine()
        {
            // Arrange
            var product = new Product() { Id = 1 };

            // Act
            _services.Add(product);

            // Assert
            Assert.IsTrue(_cart.Lines.Count == 1);
        }

        [TestMethod]
        public void CartSerVices_Add_WhenProductIsInCart_IncrementQuantity()
        {
            // Arrange
            var product = new Product() { Id = 1 };
            var previousQuantity = 4;
            _cart.Lines.Add(new CartLine() { Product = product, Quantity = previousQuantity });

            // Act
            _services.Add(product);

            // Assert
            Assert.IsTrue(_cart.Lines.Count == 1);
            Assert.IsTrue(_cart.Lines[0].Quantity == (previousQuantity + 1));
        }
    }
}