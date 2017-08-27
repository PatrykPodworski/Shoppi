using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            SetUpDeleteLineMethod();
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

        private void SetUpDeleteLineMethod()
        {
            _mockRepository.Setup(x => x.DeleteLine(It.IsAny<int>()))
                .Callback<int>(x => _cart.Lines.RemoveAll(y => y.Product.Id == x));
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
        public async Task CartServices_Add_WhenProductIsInCart_IncrementQuantity()
        {
            // Arrange
            var id = 55;
            var product = new Product() { Id = id };
            var quantity = 4;
            _cart.Lines.Add(new CartLine { Product = product, Quantity = quantity });

            // Act
            await _services.AddAsync(id);

            // Assert
            Assert.IsTrue(_cart.Lines.Count == 1);
            Assert.IsTrue(_cart.Lines[0].Quantity == (quantity + 1));
        }

        [TestMethod]
        public void CartServices_Delete_RemovesProductFromCart()
        {
            // Arrange
            var id = 14;
            var product = new Product() { Id = id };
            var quantity = 2;
            _cart.Lines.Add(new CartLine { Product = product, Quantity = quantity });

            // Act
            _services.Delete(id);

            // Assert
            Assert.IsTrue(_cart.Lines.Count == 0);
        }

        [TestMethod]
        public void CartServices_IncrementProductQuantity_IncrementsProductQuantity()
        {
            // Arrange
            var id = 82;
            var product = new Product() { Id = id };
            var quantity = 3;
            _cart.Lines.Add(new CartLine { Product = product, Quantity = quantity });

            // Act
            _services.IncrementProductQuantity(id);

            // Assert
            Assert.IsTrue(_cart.Lines[0].Quantity == quantity + 1);
        }

        [TestMethod]
        public void CartServices_IncrementProductQuantity_ReturnsNewQuantity()
        {
            // Arrange
            var id = 56;
            var product = new Product() { Id = id };
            var quantity = 7;
            _cart.Lines.Add(new CartLine { Product = product, Quantity = quantity });

            // Act
            var result = _services.IncrementProductQuantity(id);

            // Assert
            Assert.IsTrue(result == quantity + 1);
        }

        [TestMethod]
        public void CartServices_DecrementProductQuantity_WhenQuantityIsGreaterThanOne_DecrementsProductQuantity()
        {
            // Arrange
            var id = 72;
            var product = new Product() { Id = id };
            var quantity = 2;
            _cart.Lines.Add(new CartLine { Product = product, Quantity = quantity });

            // Act
            _services.DecrementProductQuantity(id);

            // Assert
            Assert.IsTrue(_cart.Lines[0].Quantity == quantity - 1);
        }

        [TestMethod]
        public void CartServices_DecrementProductQuantity_WhenQuantityIsEqualOne_DoesNotChangeQuantity()
        {
            // Arrange
            var id = 72;
            var product = new Product() { Id = id };
            var quantity = 1;
            _cart.Lines.Add(new CartLine { Product = product, Quantity = quantity });

            // Act
            _services.DecrementProductQuantity(id);

            // Assert
            Assert.IsTrue(_cart.Lines[0].Quantity == quantity);
        }

        [TestMethod]
        public void CartServices_DecrementProductQuantity_ReturnsNewQuantity()
        {
            // Arrange
            var id = 72;
            var product = new Product() { Id = id };
            var quantity = 4;
            _cart.Lines.Add(new CartLine { Product = product, Quantity = quantity });

            // Act
            var result = _services.DecrementProductQuantity(id);

            // Assert
            Assert.AreEqual(quantity - 1, result);
        }

        [TestMethod]
        public void CartServices_GetNumberOfProducts_WhenCartIsEmpty_ReturnsZero()
        {
            // Act
            var result = _services.GetNumberOfProducts();

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CartServices_GetNumberOfProducts_WhenThereIsSingleProduct_ReturnsOne()
        {
            // Arrange
            _cart.Lines.Add(new CartLine { Product = new Product(), Quantity = 1 });

            // Act
            var result = _services.GetNumberOfProducts();

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void CartServices_GetNumberOfProducts_WhereThereAreManyProductsWithQuantityOfOne_ReturnsSumOfProducts()
        {
            // Arrange
            var numberOfProducts = 13;
            for (int i = 0; i < numberOfProducts; i++)
            {
                _cart.Lines.Add(new CartLine { Product = new Product(), Quantity = 1 });
            }

            // Act
            var result = _services.GetNumberOfProducts();

            // Assert
            Assert.AreEqual(numberOfProducts, result);
        }

        [TestMethod]
        public void CartServices_GetNumberOfProducts_WhereThereAreProductsWithDiffrentQuantity_ReturnsSumOfQuantities()
        {
            // Arrange
            var numberOfProducts = 5;
            var sum = 0;
            for (int i = 0; i < numberOfProducts; i++)
            {
                _cart.Lines.Add(new CartLine { Product = new Product(), Quantity = i });
                sum += i;
            }

            // Act
            var result = _services.GetNumberOfProducts();

            // Assert
            Assert.AreEqual(sum, result);
        }
    }
}