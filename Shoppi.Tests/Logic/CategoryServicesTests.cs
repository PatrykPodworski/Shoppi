using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Exceptions;
using Shoppi.Logic.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shoppi.Tests
{
    [TestClass]
    public class CategoryServicesTests
    {
        private List<Category> _categories;
        private CategoryServices _categoryServices;
        private Mock<ICategoryRepository> _mockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _categories = new List<Category>();

            var mockRepository = SetUpMockRepository();
            _categoryServices = new CategoryServices(mockRepository.Object);
        }

        private Mock<ICategoryRepository> SetUpMockRepository()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            SetUpGetAllMethod();
            SetUpCreateMethod();

            return _mockRepository;
        }

        private void SetUpGetAllMethod()
        {
            _mockRepository.Setup(m => m.GetAllAsync())
                .Returns(Task.Run(() => _categories));
        }

        private void SetUpCreateMethod()
        {
            _mockRepository.Setup(m => m.Create(It.IsAny<Category>()))
                .Callback<Category>(x => _categories.Add(x));
        }

        [TestMethod]
        public async Task CategoryServices_GetAll_ReturnsAllCategoriesFromRepository()
        {
            // Arrange
            var numberOfCategories = 5;
            CreateCategoriesInRepository(numberOfCategories);

            // Act
            var result = await _categoryServices.GetAllAsync();

            // Assert
            Assert.IsTrue(result.Count == numberOfCategories);
            _mockRepository.Verify(m => m.GetAllAsync(), Times.Once);
        }

        private void CreateCategoriesInRepository(int numberOfCategories)
        {
            for (var i = 0; i < numberOfCategories; i++)
            {
                _categories.Add(new Category("Category") { Id = i });
            }
        }

        [TestMethod]
        public async Task CategoryServices_GetAllWithEmptyRepository_ReturnsEmptyList()
        {
            // Act
            var result = await _categoryServices.GetAllAsync();

            // Assert
            Assert.IsTrue(result.Count == 0);
            _mockRepository.Verify(m => m.GetAllAsync(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(CategoryValidationException))]
        public async Task CategoryServices_CreateWithNoName_ThrowsException()
        {
            // Arrange
            var category = new Category(null);

            // Act
            await _categoryServices.CreateAsync(category);
        }

        [TestMethod]
        [ExpectedException(typeof(CategoryValidationException))]
        public async Task CategoryServices_CreateWithWhitespaceName_ThrowsException()
        {
            // Arrange
            var category = new Category("   ");

            // Act
            await _categoryServices.CreateAsync(category);
        }

        [TestMethod]
        public async Task CategoryServices_CreateWithValidName_CreatesCategory()
        {
            // Arrange
            var category = new Category("Category");

            // Act
            await _categoryServices.CreateAsync(category);

            // Assert
            _mockRepository.Verify(m => m.Create(It.IsAny<Category>()), Times.Once);
            Assert.IsTrue(IsCorrectlyCreatedAsOnlyCategoryInRepository(category));
        }

        private bool IsCorrectlyCreatedAsOnlyCategoryInRepository(Category category)
        {
            if (_categories.Count != 1)
            {
                return false;
            }

            if (category != _categories[0])
            {
                return false;
            }

            return true;
        }
    }
}