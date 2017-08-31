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
            _mockRepository.Setup(m => m.GetByNameAsync(It.IsAny<string>()))
                .Returns<string>(x => Task.Run(() => _products.FirstOrDefault(y => y.Name == x)));
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
                    productFromRepository.Price = x.Price;
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
            var product = GenerateValidProduct();
            product.Name = null;

            // Act
            await _productServices.CreateAsync(product);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Create_WhenNameIsWhitespace_ThrowsException()
        {
            // Arrange
            var product = GenerateValidProduct();
            product.Name = "  ";

            // Act
            await _productServices.CreateAsync(product);
        }

        [TestMethod]
        public async Task ProductServices_Create_WhenValidDataIsGiven_AddsProductToRepository()
        {
            // Arrange
            var product = GenerateValidProduct();

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

        private Product GenerateValidProduct()
        {
            return new Product()
            {
                Name = "ProductName",
                Price = 0.01m,
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Create_WhenNameIsNotUnique_ThrowsException()
        {
            // Arrange
            var productName = "Product";

            var product = new Product { Name = productName };
            _products.Add(new Product { Name = productName, Id = 1 });

            // Act
            await _productServices.CreateAsync(product);
        }

        [TestMethod]
        public async Task ProductServices_Delete_WhenValidIdIsGiven_DeletesProductFromRepository()
        {
            // Arrange
            var id = 34;
            var product = new Product { Id = id };
            _products.Add(product);

            // Act
            await _productServices.DeleteAsync(id);

            // Assert
            _mockRepository.Verify(m => m.Delete(It.IsAny<int>()), Times.Once);
            Assert.IsTrue(_products.Count == 0);
        }

        [TestMethod]
        public async Task ProductServices_Delete_WhenProductWithGivenIdDoesNotExist_DoesNothing()
        {
            // Arrange
            var id = 33;
            _products.Add(new Product { Id = id - 2 });

            // Act
            await _productServices.DeleteAsync(id);

            // Assert
            _mockRepository.Verify(m => m.Delete(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(_products.Count, 1);
        }

        [TestMethod]
        public async Task ProductServices_Edit_WhenProductWithGivenIdDoesNotExists_DoesNothing()
        {
            // Arrange
            var existingProduct = GenerateValidProduct();
            existingProduct.Id = 1;
            _products.Add(existingProduct);

            var newProduct = GenerateValidProduct();
            EditProductValues(newProduct);
            newProduct.Id = 2;

            // Act
            await _productServices.EditAsync(newProduct);

            // Assert
            _mockRepository.Verify(m => m.EditAsync(It.IsAny<Product>()), Times.Once);
            Assert.IsTrue(!IsEdited(existingProduct, newProduct));
        }

        [TestMethod]
        public async Task ProductServices_Edit_WhenValidProductIsGiven_ChangesProductInRepository()
        {
            // Arrange
            var existingProduct = GenerateValidProduct();
            _products.Add(existingProduct);

            var newProduct = GenerateValidProduct();
            EditProductValues(newProduct);

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
            var newProduct = new Product { Id = 1 };

            // Act
            await _productServices.EditAsync(newProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Edit_WhenNameIsSetToWhitespace_ThrowsException()
        {
            // Arrange
            var newProduct = new Product { Name = "  ", Id = 1 };

            // Act
            await _productServices.EditAsync(newProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Edit_WhenNewNameIsNotUnique_ThrowsException()
        {
            // Arrange
            var productName = "Product";
            var existingProduct = new Product { Name = productName, Id = 2 };
            _products.Add(existingProduct);

            var newProduct = new Product { Name = productName, Id = 1 };

            // Act
            await _productServices.EditAsync(newProduct);
        }

        [TestMethod]
        public async Task ProductServices_Edit_WhenNameIsNotChanged_EditsProductInRepository()
        {
            // Arrange
            var id = 1;
            var existingProduct = GenerateValidProduct();
            existingProduct.Id = id;
            _products.Add(existingProduct);

            var newProduct = GenerateValidProduct();
            newProduct.Id = id;
            EditProductValues(newProduct);
            newProduct.Name = existingProduct.Name;

            // Act
            await _productServices.EditAsync(newProduct);

            // Assert
            Assert.IsTrue(IsEdited(existingProduct, newProduct));
        }

        private void EditProductValues(Product product)
        {
            product.Name = "New" + product.Name;
            product.Price = product.Price * 2 + 1m;
            product.CategoryId = product.CategoryId * 2 + 1;
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

            if (editedProduct.Price != editValues.Price)
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
            for (int i = 0; i < numberOfProductsWithGivenCategoryId; i++)
            {
                _products.Add(new Product { CategoryId = categoryId });
            }

            // Act
            var products = await _productServices.GetByCategoryIdAsync(categoryId);

            // Assert
            _mockRepository.Verify(m => m.GetByCategoryIdAsync(It.IsAny<int>()), Times.Once);
            Assert.IsTrue(products.Count == numberOfProductsWithGivenCategoryId);
        }

        [TestMethod]
        public async Task ProductServices_GetByCategoryId_WhenThereAreNotProductsWithGivenCategoryId_ReturnsEmptyList()
        {
            // Arrange
            var categoryId = 1;

            _products.Add(new Product { CategoryId = categoryId * 2 + 1 });

            // Act
            var result = await _productServices.GetByCategoryIdAsync(categoryId);

            // Assert
            _mockRepository.Verify(m => m.GetByCategoryIdAsync(It.IsAny<int>()), Times.Once);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public async Task ProductServices_GetAll_ReturnsAllProductsFromRepository()
        {
            // Arrange
            var numberOfProducts = 5;
            for (int i = 0; i < numberOfProducts; i++)
            {
                _products.Add(new Product());
            }

            // Act
            var result = await _productServices.GetAllAsync();

            // Assert
            Assert.AreEqual(numberOfProducts, result.Count);
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
            var id = 10;
            var product = new Product { Id = id };
            _products.Add(product);

            // Act
            var result = await _productServices.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(product, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Create_WhenPriceIsNegative_ThrowsException()
        {
            // Arrange
            var price = -13.66m;
            var product = GenerateValidProduct();
            product.Price = price;

            // Act
            await _productServices.CreateAsync(product);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Create_WhenPriceIsZero_ThrowsException()
        {
            // Arrange
            var price = 0m;
            var product = GenerateValidProduct();
            product.Price = price;

            // Act
            await _productServices.CreateAsync(product);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Edit_WhenPriceIsNegative_ThrowsException()
        {
            // Arrange
            var price = -10m;
            var product = GenerateValidProduct();
            _products.Add(product);

            var newProduct = GenerateValidProduct();
            newProduct.Price = price;

            // Act
            await _productServices.EditAsync(newProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Edit_WhenPriceIsZero_ThrowsException()
        {
            // Arrange
            var price = 0m;
            var product = GenerateValidProduct();
            _products.Add(product);

            var newProduct = GenerateValidProduct();
            newProduct.Price = price;

            // Act
            await _productServices.EditAsync(newProduct);
        }
    }
}