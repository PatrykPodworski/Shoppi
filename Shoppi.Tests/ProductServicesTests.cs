using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            SetUpMockRepository();
            _productServices = new ProductServices(_mockRepository.Object);
        }

        private void SetUpMockRepository()
        {
            _mockRepository = new Mock<IProductRepository>();
            SetUpCreateMethod();
            SetUpGetByNameMethod();
            SetUpDeleteMethod();
            SetUpEditMethod();
        }

        private void SetUpCreateMethod()
        {
            _mockRepository.Setup(x => x.Create(It.IsAny<Product>()))
                .Callback<Product>(x => _products.Add(x));
        }

        private void SetUpGetByNameMethod()
        {
            _mockRepository.Setup(m => m.GetByName(It.IsAny<string>()))
                .Returns<string>(x => _products.FirstOrDefault(y => y.Name == x));
        }

        private void SetUpDeleteMethod()
        {
            _mockRepository.Setup(m => m.Delete(It.IsAny<int>()))
                .Callback<int>(x => _products.RemoveAll(y => y.Id == x));
        }

        private void SetUpEditMethod()
        {
            _mockRepository.Setup(m => m.Edit(It.IsAny<Product>()))
                .Callback<Product>(x =>
                {
                    var productFromRepository = _products.FirstOrDefault(y => y.Id == x.Id);

                    if (productFromRepository == null)
                    {
                        return;
                    }

                    productFromRepository.Name = x.Name;
                    productFromRepository.CategoryId = x.CategoryId;
                    productFromRepository.Quantity = x.Quantity;
                });
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
            _products.Add(new Product(productName, 1, 12) { Id = 1 });

            // Act
            await _productServices.CreateAsync(product);
        }

        [TestMethod]
        public async Task ProductServices_Delete_DeletesFromRepository()
        {
            // Arrange
            var id = 1;
            _products.Add(new Product("ProductName", 0) { Id = id });
            _products.Add(new Product("ProductName2", 0) { Id = 2 });

            // Act
            await _productServices.DeleteAsync(id);

            // Assert
            _mockRepository.Verify(m => m.Delete(It.IsAny<int>()), Times.Once);
            Assert.IsTrue(_products.Count == 1);
        }

        [TestMethod]
        public async Task ProductServices_DeleteNotExistingProduct_NothingHappens()
        {
            // Arrange
            var id = 1;
            _products.Add(new Product("ProductName", 0) { Id = 2 });
            _products.Add(new Product("ProductName2", 0) { Id = 3 });

            // Act
            await _productServices.DeleteAsync(id);

            // Assert
            _mockRepository.Verify(m => m.Delete(It.IsAny<int>()), Times.Once);
            Assert.IsTrue(_products.Count == 2);
        }

        [TestMethod]
        public async Task ProductServices_EditNotExistingProduct_NothingHappens()
        {
            // Arrange
            var productName = "Product";
            var categoryId = 0;
            var quantity = 100;
            var existingProduct = new Product(productName, categoryId, quantity) { Id = 1 };
            _products.Add(existingProduct);

            var newProduct = new Product("NewProduct", 2, 10) { Id = 2 };

            // Act
            await _productServices.EditAsync(newProduct);

            // Assert
            _mockRepository.Verify(m => m.Edit(It.IsAny<Product>()), Times.Once);
            Assert.IsTrue(existingProduct.Name == productName);
            Assert.IsTrue(existingProduct.CategoryId == categoryId);
            Assert.IsTrue(existingProduct.Quantity == quantity);
        }

        [TestMethod]
        public async Task ProductServices_EditExistingProduct_ChangesProductInRepository()
        {
            // Arrange
            var productName = "NewProduct";
            var categoryId = 0;
            var quantity = 100;
            var id = 1;
            var existingProduct = new Product("Product", 2, 10) { Id = id };
            _products.Add(existingProduct);

            var newProduct = new Product(productName, categoryId, quantity) { Id = id };

            // Act
            await _productServices.EditAsync(newProduct);

            // Assert
            _mockRepository.Verify(m => m.Edit(It.IsAny<Product>()), Times.Once);
            Assert.IsTrue(IsEdited(existingProduct, newProduct));
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_EditToNoName_ThrowsException()
        {
            // Arrange
            var newProduct = new Product(null, 0, 11) { Id = 1 };

            // Act
            await _productServices.EditAsync(newProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_EditToWhiteSpaceName_ThrowsException()
        {
            // Arrange
            var newProduct = new Product("  ", 0, 11) { Id = 1 };

            // Act
            await _productServices.EditAsync(newProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_EditToInvalidQuantity_ThrowsException()
        {
            // Arrange
            var newProduct = new Product("Product", 0, -13) { Id = 1 };

            // Act
            await _productServices.EditAsync(newProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_EditToExistingName_ThrowsException()
        {
            // Arrange
            var productName = "Product";
            var existingProduct = new Product(productName, 2, 10) { Id = 2 };
            _products.Add(existingProduct);

            var newProduct = new Product(productName, 1, 13) { Id = 1 };

            // Act
            await _productServices.EditAsync(newProduct);
        }

        [TestMethod]
        public async Task ProductServices_EditWithUnchangedName_Passes()
        {
            // Arrange
            var productName = "Product";
            var id = 1;
            var existingProduct = new Product(productName, 2, 10) { Id = id };
            _products.Add(existingProduct);

            var newProduct = new Product(productName, 1, 13) { Id = id };

            // Act
            await _productServices.EditAsync(newProduct);

            // Assert
            Assert.IsTrue(IsEdited(existingProduct, newProduct));
        }

        private bool IsEdited(Product editedProduct, Product editValues)
        {
            if (editedProduct.Name != editValues.Name)
            {
                return false;
            }
            if (editedProduct.CategoryId != editValues.CategoryId)
            {
                return false;
            }
            if (editedProduct.Quantity != editValues.Quantity)
            {
                return false;
            }

            return true;
        }
    }
}