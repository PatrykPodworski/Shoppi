using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Data.Specifications;
using Shoppi.Logic.Abstract;
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
        private Mock<ICategoryServices> _mockCategoryServices;
        private Mock<IProductSpecificationFactory> _mockFactory;
        private ProductServices _productServices;
        private List<Product> _products;

        [TestInitialize]
        public void TestInitialize()
        {
            _products = new List<Product>();

            SetUpMockRepository();
            SetUpMockCategoryServices();
            SetUpMockBuilder();
            _productServices = new ProductServices(_mockRepository.Object, _mockCategoryServices.Object, _mockFactory.Object);
        }

        private void SetUpMockRepository()
        {
            _mockRepository = new Mock<IProductRepository>();
            SetUpCreateMethod();
            SetUpGetByNameMethod();
            SetUpGetByCategoryIdMethod();
            SetUpGetAllMethod();
            SetUpGetByIdMethod();
            SetUpEditMethod();
            SetUpDeleteMethod();
            SetUpGetMethod();
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
                    productFromRepository.BrandId = x.BrandId;
                }));
        }

        private void SetUpDeleteMethod()
        {
            _mockRepository.Setup(m => m.Delete(It.IsAny<int>()))
                .Callback<int>(x => _products.RemoveAll(y => y.Id == x));
        }

        private void SetUpGetNumberOfProductsSatisfyingMethod(int numberOfProducts)
        {
            _mockRepository.Setup(m => m.GetNumberOfProductsSatisfying(It.IsAny<Specification<Product>>()))
                .Returns(Task.Run(() => numberOfProducts));
        }

        private void SetUpGetMethod()
        {
            _mockRepository.Setup(x => x.GetAsync(It.IsAny<Specification<Product>>()))
                .Returns(Task.FromResult<ICollection<Product>>(_products));
        }

        private void SetUpMockCategoryServices()
        {
            _mockCategoryServices = new Mock<ICategoryServices>();
            SetUpIsFinalCategoryMethod(true);
        }

        private void SetUpMockBuilder()
        {
            _mockFactory = new Mock<IProductSpecificationFactory>();
        }

        private void SetUpIsFinalCategoryMethod(bool returns)
        {
            _mockCategoryServices.Setup(x => x.IsFinalCategoryAsync(It.IsAny<int>()))
                .Returns(Task.Run(() => returns));
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

        private void EditProductValues(Product product)
        {
            product.Name = "New" + product.Name;
            product.Price = product.Price * 2 + 1m;
            product.CategoryId = product.CategoryId * 2 + 1;
            product.BrandId = product.BrandId * 2 + 1;
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

            if (editedProduct.BrandId != editValues.BrandId)
            {
                return false;
            }

            return true;
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Edit_WhenNameIsSetToNull_ThrowsException()
        {
            // Arrange
            var product = GenerateValidProduct();
            product.Name = null;

            // Act
            await _productServices.EditAsync(product);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Edit_WhenNameIsSetToWhitespace_ThrowsException()
        {
            // Arrange
            var product = GenerateValidProduct();
            product.Name = " ";

            // Act
            await _productServices.EditAsync(product);
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

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Create_WhenCategoryIdIsNotFinal_ThrowsException()
        {
            // Arrange
            SetUpIsFinalCategoryMethod(false);
            var product = GenerateValidProduct();

            // Act
            await _productServices.CreateAsync(product);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductValidationException))]
        public async Task ProductServices_Edit_WhenCategoryIdIsNotFinal_ThrowsException()
        {
            // Arrange
            var product = GenerateValidProduct();
            _products.Add(product);

            var newProduct = GenerateValidProduct();
            SetUpIsFinalCategoryMethod(false);

            // Act
            await _productServices.EditAsync(newProduct);
        }

        [TestMethod]
        public async Task ProductServices_GetNumberOfPages_WhenNumberOfProductsIsDivisibleByProductPerPage_ReturnsNumberOfPages()
        {
            // Arrange
            var numberOfProducts = 27;
            var productsPerPage = 9;
            var pages = 3;
            SetUpGetNumberOfProductsSatisfyingMethod(numberOfProducts);

            var filters = new ProductFilters { ProductsPerPage = productsPerPage };

            // Act
            var result = await _productServices.GetNumberOfPages(filters);

            // Assert
            Assert.AreEqual(pages, result);
        }

        [TestMethod]
        public async Task ProductServices_GetNumberOfPages_WhenNumberOfProductsIsNoTDivisibleByProductPerPage_ReturnsNumberOfPages()
        {
            // Arrange
            var numberOfProducts = 28;
            var productsPerPage = 9;
            var pages = 4;
            SetUpGetNumberOfProductsSatisfyingMethod(numberOfProducts);

            var filters = new ProductFilters { ProductsPerPage = productsPerPage };

            // Act
            var result = await _productServices.GetNumberOfPages(filters);

            // Assert
            Assert.AreEqual(pages, result);
        }

        [TestMethod]
        public async Task ProductServices_GetWithFilters_ReturnsProductsSatifyingSpecification()
        {
            // Arrange
            var numberOfProducts = 13;
            for (int i = 0; i < numberOfProducts; i++)
            {
                _products.Add(new Product());
            }
            var filters = new ProductFilters();

            // Act
            var result = await _productServices.GetAsync(filters);

            // Assert
            Assert.AreEqual(numberOfProducts, result.Count);
        }
    }
}