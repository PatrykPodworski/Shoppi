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
        private Mock<ITypeServices> _mockTypeServices;
        private Cart _cart;
        private List<ProductType> _types;

        [TestInitialize]
        public void TestInitialize()
        {
            _cart = new Cart();
            _types = new List<ProductType>();

            _mockRepository = new Mock<ICartRepository>();
            SetUpMockRepository();

            _mockTypeServices = new Mock<ITypeServices>();
            SetUpMockTypeServices();

            _services = new CartServices(_mockRepository.Object, _mockTypeServices.Object);
        }

        private void SetUpMockRepository()
        {
            SetUpGetCartMethod();
            SetUpAddLineMethod();
            SetUpGetCartLineMethod();
            SetUpIncrementCartLineQuantityMethod();
            SetUpDecrementCartLineQuantityMethod();
            SetUpDeleteLineMethod();
        }

        private void SetUpGetCartMethod()
        {
            _mockRepository.Setup(x => x.GetCart())
                .Returns(_cart);
        }

        private void SetUpAddLineMethod()
        {
            _mockRepository.Setup(x => x.AddLine(It.IsAny<ProductType>()))
                .Callback<ProductType>(x => _cart.Lines.Add(new CartLine { Type = x }));
        }

        private void SetUpGetCartLineMethod()
        {
            _mockRepository.Setup(x => x.GetCartLine(It.IsAny<int>()))
                .Returns<int>(x => _cart.Lines.FirstOrDefault(y => y.Type.Id == x));
        }

        private void SetUpIncrementCartLineQuantityMethod()
        {
            _mockRepository.Setup(x => x.IncrementCartLineQuantity(It.IsAny<int>()))
                .Callback<int>(x => _cart.Lines.FirstOrDefault(y => y.Type.Id == x).Quantity++);
        }

        private void SetUpDecrementCartLineQuantityMethod()
        {
            _mockRepository.Setup(x => x.DecrementCartLineQuantity(It.IsAny<int>()))
                .Callback<int>(x => _cart.Lines.FirstOrDefault(y => y.Type.Id == x).Quantity--);
        }

        private void SetUpDeleteLineMethod()
        {
            _mockRepository.Setup(x => x.DeleteLine(It.IsAny<int>()))
                .Callback<int>(x => _cart.Lines.RemoveAll(y => y.Type.Id == x));
        }

        private void SetUpMockTypeServices()
        {
            SetUpGetByIdMethod();
        }

        private void SetUpGetByIdMethod()
        {
            _mockTypeServices.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns<int>(x => Task.Run(() => _types.FirstOrDefault(t => t.Id == x)));
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
        public async Task CartServices_Add_WhenTypeIsNotInTheCart_AddsNewCartLineWithProperType()
        {
            // Arrange
            var typeId = 32;
            var type = new ProductType { Id = typeId };
            _types.Add(type);

            // Act
            await _services.AddAsync(typeId);

            // Assert
            Assert.AreEqual(1, _cart.Lines.Count);
            Assert.AreEqual(type, _cart.Lines[0].Type);
        }

        [TestMethod]
        public async Task CartServices_Add_WhenTypeIsInTheCart_IncrementsQuantityOfCartLine()
        {
            // Arrange
            var typeId = 32;
            var type = new ProductType { Id = typeId };
            _types.Add(type);

            var quantity = 36;
            _cart.Lines.Add(new CartLine { Type = type, Quantity = quantity });

            // Act
            await _services.AddAsync(typeId);

            // Assert
            Assert.AreEqual(1, _cart.Lines.Count);
            Assert.AreEqual(36 + 1, _cart.Lines[0].Quantity);
        }

        [TestMethod]
        public void CartServices_Delete_RemovesTypeFromCart()
        {
            // Arrange
            var id = 14;
            var type = new ProductType { Id = id };
            _cart.Lines.Add(new CartLine { Type = type });

            // Act
            _services.Delete(id);

            // Assert
            Assert.AreEqual(0, _cart.Lines.Count);
        }

        [TestMethod]
        public void CartServices_IncrementProductQuantity_IncrementsProductQuantity()
        {
            // Arrange
            var id = 82;
            var type = new ProductType { Id = id };
            var quantity = 3;
            _cart.Lines.Add(new CartLine { Type = type, Quantity = quantity });

            // Act
            _services.IncrementCartLineQuantity(id);

            // Assert
            Assert.AreEqual(quantity + 1, _cart.Lines[0].Quantity);
        }

        [TestMethod]
        public void CartServices_IncrementProductQuantity_ReturnsNewQuantity()
        {
            // Arrange
            var id = 12;
            var type = new ProductType { Id = id };
            var quantity = 3;
            _cart.Lines.Add(new CartLine { Type = type, Quantity = quantity });

            // Act
            var result = _services.IncrementCartLineQuantity(id);

            // Assert
            Assert.AreEqual(quantity + 1, result);
        }

        [TestMethod]
        public void CartServices_IncrementProductQuantity_WhenThereIsNoCartLineWithGivenId_ReturnsZero()
        {
            // Arrange
            var id = 12;

            // Act
            var result = _services.IncrementCartLineQuantity(id);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CartServices_DecrementProductQuantity_WhenQuantityIsGreaterThanOne_DecrementsProductQuantity()
        {
            // Arrange
            var id = 12;
            var type = new ProductType { Id = id };
            var quantity = 3;
            _cart.Lines.Add(new CartLine { Type = type, Quantity = quantity });

            // Act
            _services.DecrementCartLineQuantity(id);

            // Assert
            Assert.AreEqual(quantity - 1, _cart.Lines[0].Quantity);
        }

        [TestMethod]
        public void CartServices_DecrementProductQuantity_WhenThereIsNoCartLineWithGivenId_ReturnsZero()
        {
            // Arrange
            var id = 12;

            // Act
            var result = _services.DecrementCartLineQuantity(id);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CartServices_DecrementProductQuantity_WhenQuantityIsEqualOne_DoesNotChangeQuantity()
        {
            // Arrange
            var id = 72;
            var quantity = 1;
            var type = new ProductType { Id = id };
            _cart.Lines.Add(new CartLine { Type = type, Quantity = quantity });

            // Act
            _services.DecrementCartLineQuantity(id);

            // Assert
            Assert.AreEqual(quantity, _cart.Lines[0].Quantity);
        }

        [TestMethod]
        public void CartServices_DecrementProductQuantity_ReturnsNewQuantity()
        {
            // Arrange
            var id = 32;
            var quantity = 13;
            var type = new ProductType { Id = id };
            _cart.Lines.Add(new CartLine { Type = type, Quantity = quantity });

            // Act
            var result = _services.DecrementCartLineQuantity(id);

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
            _cart.Lines.Add(new CartLine { Type = new ProductType(), Quantity = 1 });

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
                _cart.Lines.Add(new CartLine { Type = new ProductType(), Quantity = 1 });
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
                _cart.Lines.Add(new CartLine { Type = new ProductType(), Quantity = i });
                sum += i;
            }

            // Act
            var result = _services.GetNumberOfProducts();

            // Assert
            Assert.AreEqual(sum, result);
        }
    }
}