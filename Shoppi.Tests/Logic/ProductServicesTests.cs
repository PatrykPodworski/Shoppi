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

            var mockRepository = SetUpMockRepository();
            _productServices = new ProductServices(mockRepository.Object);
        }

        private Mock<IProductRepository> SetUpMockRepository()
        {
            _mockRepository = new Mock<IProductRepository>();
            SetUpCreateMethod();
            SetUpGetByNameMethod();
            SetUpGetByCategoryIdMethod();
            SetUpGetAllMethod();
            SetUpGetByIdMethod();
            SetUpEditMethod();
            SetUpDeleteMethod();

            return _mockRepository;
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

        private void SetUpGetByCategoryIdMethod()
        {
            _mockRepository.Setup(m => m.GetByCategoryIdAsync(It.IsAny<int>()))
                .Returns<int>(x => Task.Run(() => _products.Where(y => y.CategoryId == x).ToList()));
        }

        private void SetUpGetAllMethod()
        {
            _mockRepository.Setup(m => m.GetAllAsync()).Returns(Task.Run(() => _products));
        }

        private void SetUpGetByIdMethod()
        {
            _mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>()))
                .Returns<int>(x => Task.Run(() => _products.FirstOrDefault(p => p.Id == x)));
        }

        private void SetUpEditMethod()
        {
            _mockRepository.Setup(m => m.EditAsync(It.IsAny<Product>()))
                .Returns<Product>(x => Task.Run(() =>
                {
                    var productFromRepository = _products.FirstOrDefault(y => y.Id == x.Id);

                    if (productFromRepository == null)
                    {
                        return;
                    }

                    productFromRepository.Name = x.Name;
                    productFromRepository.CategoryId = x.CategoryId;
                    productFromRepository.Quantity = x.Quantity;
                }));
        }

        private void SetUpDeleteMethod()
        {
            _mockRepository.Setup(m => m.Delete(It.IsAny<int>()))
                .Callback<int>(x => _products.RemoveAll(y => y.Id == x));
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Create_WhenNameIsNull_ThrowsException()
        {
            // Arrange
            var product = new Product(null, 0);

            // Act
            await _productServices.CreateAsync(product);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Create_WhenNameIsWhitespace_ThrowsException()
        {
            // Arrange
            var product = new Product("      ", 0);

            // Act
            await _productServices.CreateAsync(product);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Create_WhenQuantityIsNegative_ThrowsException()
        {
            // Arrange
            var product = new Product("Product", 0, -100);

            // Act
            await _productServices.CreateAsync(product);
        }

        [TestMethod]
        public async Task ProductServices_Create_WhenValidDataIsGiven_AddsProductToRepository()
        {
            // Arrange
            var productName = "Product";
            var quantity = 100;

            var product = new Product(productName, 0, quantity);

            // Act
            await _productServices.CreateAsync(product);

            // Assert
            _mockRepository.Verify(m => m.Create(It.IsAny<Product>()), Times.Once());
            Assert.IsTrue(IsCorrectlyCreatedAsOnlyProductInRepository(product));
        }

        private bool IsCorrectlyCreatedAsOnlyProductInRepository(Product product)
        {
            if (_products.Count != 1)
            {
                return false;
            }
            if (_products[0] != product)
            {
                return false;
            }

            return true;
        }

        [TestMethod]
        public async Task ProductServices_Create_WhenQuantityIsZero_AddsProductToRepository()
        {
            // Arrange
            var productName = "Product";
            var quantity = 0;

            var product = new Product(productName, 0, quantity);

            // Act
            await _productServices.CreateAsync(product);

            // Assert
            _mockRepository.Verify(m => m.Create(It.IsAny<Product>()), Times.Once());
            Assert.IsTrue(IsCorrectlyCreatedAsOnlyProductInRepository(product));
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Create_WhenNameIsNotUnique_ThrowsException()
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
        public async Task ProductServices_Delete_WhenValidIdIsGiven_DeletesProductFromRepository()
        {
            // Arrange
            var id = 34;
            var numberOfProducts = 5;
            CreateProductsInMockRepository(numberOfProducts);
            _products.Add(new Product("ProductName", 0) { Id = 34 });

            // Act
            await _productServices.DeleteAsync(id);

            // Assert
            _mockRepository.Verify(m => m.Delete(It.IsAny<int>()), Times.Once);
            Assert.IsTrue(_products.Count == 5);
        }

        private void CreateProductsInMockRepository(int numberOfProducts, int categoryId = 0)
        {
            for (var i = 0; i < numberOfProducts; i++)
            {
                _products.Add(new Product("Product" + i, categoryId));
            }
        }

        [TestMethod]
        public async Task ProductServices_Delete_WhenProductWithGivenIdDoesNotExist_DoesNothing()
        {
            // Arrange
            var numberOfProducts = 10;
            var id = 33;
            CreateProductsInMockRepository(numberOfProducts);

            // Act
            await _productServices.DeleteAsync(id);

            // Assert
            _mockRepository.Verify(m => m.Delete(It.IsAny<int>()), Times.Once);
            Assert.IsTrue(_products.Count == numberOfProducts);
        }

        [TestMethod]
        public async Task ProductServices_Edit_WhenProductWithGivenIdDoesNotExists_DoesNothing()
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
            _mockRepository.Verify(m => m.EditAsync(It.IsAny<Product>()), Times.Once);
            Assert.IsTrue(existingProduct.Name == productName);
            Assert.IsTrue(existingProduct.CategoryId == categoryId);
            Assert.IsTrue(existingProduct.Quantity == quantity);
        }

        [TestMethod]
        public async Task ProductServices_Edit_WhenValidProductIsGiven_ChangesProductInRepository()
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
            _mockRepository.Verify(m => m.EditAsync(It.IsAny<Product>()), Times.Once);
            Assert.IsTrue(IsEdited(existingProduct, newProduct));
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Edit_WhenNameIsSetToNull_ThrowsException()
        {
            // Arrange
            var newProduct = new Product(null, 0, 11) { Id = 1 };

            // Act
            await _productServices.EditAsync(newProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Edit_WhenNameIsSetToWhitespace_ThrowsException()
        {
            // Arrange
            var newProduct = new Product("  ", 0, 11) { Id = 1 };

            // Act
            await _productServices.EditAsync(newProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Edit_WhenQuantityIsNegative_ThrowsException()
        {
            // Arrange
            var newProduct = new Product("Product", 0, -13) { Id = 1 };

            // Act
            await _productServices.EditAsync(newProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Edit_WhenNewNameIsNotUnique_ThrowsException()
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
        public async Task ProductServices_Edit_WhenNameIsNotChanged_EditsProductInRepository()
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

        [TestMethod]
        public async Task ProductServices_GetByCategoryId_ReturnsAllProductsWithGivenCategoryId()
        {
            // Arrange
            var categoryId = 1;
            var numberOfProductsWithGivenCategoryId = 5;
            CreateProductsInMockRepository(numberOfProductsWithGivenCategoryId, categoryId);
            CreateProductsInMockRepository(3, 3);
            CreateProductsInMockRepository(3, 4);

            // Act
            var products = await _productServices.GetByCategoryIdAsync(categoryId);

            // Assert
            Assert.IsTrue(products.Count == numberOfProductsWithGivenCategoryId);
            _mockRepository.Verify(m => m.GetByCategoryIdAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task ProductServices_GetByCategoryId_WhenThereAreNotProductsWithGivenCategoryId_ReturnsEmptyList()
        {
            // Arrange
            var categoryId = 1;

            CreateProductsInMockRepository(3, 3);
            CreateProductsInMockRepository(3, 4);

            // Act
            var products = await _productServices.GetByCategoryIdAsync(categoryId);

            // Assert
            Assert.IsTrue(products.Count == 0);
            _mockRepository.Verify(m => m.GetByCategoryIdAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task ProductServices_GetAll_ReturnsAllProductsFromRepository()
        {
            // Arrange
            var numberOfProducts = 5;
            CreateProductsInMockRepository(numberOfProducts);

            // Act
            var result = await _productServices.GetAllAsync();

            // Assert
            Assert.AreEqual(numberOfProducts, result.Count);
            Assert.AreEqual(_products, result);
        }

        [TestMethod]
        public async Task ProductServices_GetAll_WhenRepositoryIsEmpty_ReturnsEmptyList()
        {
            // Act
            var result = await _productServices.GetAllAsync();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task ProductServices_GetById_ReturnsProductWithMatchingId()
        {
            // Arrange
            var numberOfProducts = 5;
            var productName = "Product";
            var categoryId = 1;
            var quantity = 333;
            var id = 10;

            CreateProductsInMockRepository(numberOfProducts);
            _products.Add(new Product(productName, categoryId, quantity) { Id = id });

            // Act
            var result = await _productServices.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(productName, result.Name);
            Assert.AreEqual(categoryId, result.CategoryId);
            Assert.AreEqual(quantity, result.Quantity);
        }
    }
}