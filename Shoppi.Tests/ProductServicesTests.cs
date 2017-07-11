using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Exceptions;
using Shoppi.Logic.Implementation;
using System.Threading.Tasks;

namespace Shoppi.Tests
{
    [TestClass]
    public class ProductServicesTests
    {
        private Mock<IProductRepository> _mockRepository;
        private ProductServices _productServices;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockRepository.Setup(x => x.Create(It.IsAny<Product>()));
            _productServices = new ProductServices(_mockRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidProductNameException))]
        public async Task ProductServices_CreateWithNoName_ThrowsException()
        {
            // Arrange
            var product = new Product(null, new Category("Category"));

            // Act
            await _productServices.Create(product);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidProductNameException))]
        public async Task ProductServices_CreateWithWhitespacesName_ThrowsException()
        {
            // Arrange
            var product = new Product("      ", new Category("Category"));

            // Act
            await _productServices.Create(product);
        }

        [TestMethod]
        [ExpectedException(typeof(NegativeProductQuantityException))]
        public async Task ProductServices_CreateWithNegativeAmount_ThrowsException()
        {
            // Arrange
            var product = new Product("Product", new Category("Category"), -100);

            // Act
            await _productServices.Create(product);
        }

        [TestMethod]
        public async Task ProductServices_CreateWithValidData_Passes()
        {
            // Arrange
            var productName = "Product";
            var category = new Category("Category");
            var quantity = 100;

            var product = new Product(productName, category, quantity);

            // Act
            await _productServices.Create(product);

            // Assert
            _mockRepository.Verify(m => m.Create(It.IsAny<Product>()), Times.Once());
        }

        [TestMethod]
        public async Task ProductServices_CreateWithZeroQuantity_Passes()
        {
            // Arrange
            var productName = "Product";
            var category = new Category("Category");
            var quantity = 0;

            var product = new Product(productName, category, quantity);

            // Act
            await _productServices.Create(product);

            // Assert
            _mockRepository.Verify(m => m.Create(It.IsAny<Product>()), Times.Once());
        }
    }
}