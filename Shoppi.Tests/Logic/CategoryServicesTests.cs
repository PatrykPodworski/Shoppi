using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Exceptions;
using Shoppi.Logic.Implementation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppi.Tests.Logic
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
            SetUpGetByIdMethod();
            SetUpGetSubCategoriesMethod();
            SetUpCreateMethod();
            SetUpEditMethod();
            SetUpDeleteMethod();

            return _mockRepository;
        }

        private void SetUpGetAllMethod()
        {
            _mockRepository.Setup(m => m.GetAllAsync())
                .Returns(Task.Run(() => _categories));
        }

        private void SetUpGetByIdMethod()
        {
            _mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>()))
                .Returns<int>(x => Task.Run(() => _categories.FirstOrDefault(c => c.Id == x)));
        }

        private void SetUpGetSubCategoriesMethod()
        {
            _mockRepository.Setup(m => m.GetSubCategoriesAsync(It.IsAny<int>()))
                .Returns<int>(x => Task.Run(() => _categories.Where(c => c.HeadCategoryId == x).ToList()));
        }

        private void SetUpCreateMethod()
        {
            _mockRepository.Setup(m => m.Create(It.IsAny<Category>()))
                .Callback<Category>(x => _categories.Add(x));
        }

        private void SetUpEditMethod()
        {
            _mockRepository.Setup(m => m.EditAsync(It.IsAny<Category>()))
                .Returns<Category>(x => Task.Run(() =>
                {
                    var categoryFromRepository = _categories.FirstOrDefault(y => y.Id == x.Id);

                    if (categoryFromRepository == null)
                    {
                        return;
                    }

                    categoryFromRepository.Name = x.Name;
                    categoryFromRepository.HeadCategoryId = x.HeadCategoryId;
                }));
        }

        private void SetUpDeleteMethod()
        {
            _mockRepository.Setup(m => m.Delete(It.IsAny<int>()))
                .Callback<int>(x => _categories.RemoveAll(c => c.Id == x));
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

        [TestMethod]
        public async Task CategoryServices_Edit_ChangesValueOfCategoryInRepository()
        {
            // Arrange
            var id = 1;
            var existingCategory = new Category("Existing Category") { Id = id, HeadCategoryId = 2 };
            _categories.Add(existingCategory);

            var editCategory = new Category("Edited Category") { Id = id, HeadCategoryId = null };

            // Act
            await _categoryServices.EditAsync(editCategory);

            // Assert
            Assert.IsTrue(IsCorrectlyEdited(existingCategory, editCategory));
        }

        private bool IsCorrectlyEdited(Category existingCategory, Category editValues)
        {
            if (existingCategory.Id != editValues.Id)
            {
                return false;
            }

            if (existingCategory.Name != editValues.Name)
            {
                return false;
            }

            if (existingCategory.HeadCategoryId != editValues.HeadCategoryId)
            {
                return false;
            }

            return true;
        }

        [TestMethod]
        public async Task CategoryServices_EditNotExistingCategory_NothingHappens()
        {
            // Arrange
            var id = 1;
            var idOfNotExistingCategory = 2;
            var categoryName = "Category";
            var headCategoryId = 5;
            var categoryInRepo = new Category(categoryName) { Id = id, HeadCategoryId = headCategoryId };
            _categories.Add(categoryInRepo);

            var editCategory = new Category("New name") { Id = idOfNotExistingCategory, HeadCategoryId = 11 };

            // Act
            await _categoryServices.EditAsync(editCategory);

            // Assert
            Assert.IsTrue(_categories.Count == 1);
            Assert.IsTrue(categoryInRepo.Name == categoryName);
            Assert.IsTrue(categoryInRepo.Id == id);
            Assert.IsTrue(categoryInRepo.HeadCategoryId == headCategoryId);
        }

        [TestMethod]
        [ExpectedException(typeof(CategoryValidationException))]
        public async Task CategoryServices_EditToNoName_ThrowsException()
        {
            // Arrange
            var id = 1;
            var existingCategory = new Category("Existing Category") { Id = id, HeadCategoryId = 2 };
            _categories.Add(existingCategory);

            // Act
            await _categoryServices.EditAsync(new Category(null) { Id = id });
        }

        [TestMethod]
        [ExpectedException(typeof(CategoryValidationException))]
        public async Task CategoryServices_EditToWhiteSpaceName_ThrowsException()
        {
            // Arrange
            var id = 1;
            var existingCategory = new Category("Existing Category") { Id = id, HeadCategoryId = 2 };
            _categories.Add(existingCategory);

            // Act
            await _categoryServices.EditAsync(new Category("  ") { Id = id });
        }

        [TestMethod]
        public async Task CategoryServices_GetById_ReturnsCategoryWithProperIdFromRepository()
        {
            // Arrange
            var id = 1;
            var category = new Category("Category") { Id = id };
            _categories.Add(category);

            // Act
            var result = await _categoryServices.GetByIdAsync(id);

            // Assert
            Assert.IsTrue(result.Id == id);
        }

        [TestMethod]
        public async Task CategoryServices_GetByIdWithNoProperCategoryInRepository_ReturnsNull()
        {
            // Act
            var result = await _categoryServices.GetByIdAsync(1);

            // Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public async Task CategoryServices_Delete_DeletesOnlyCategoryWithGivenIdFromRepository()
        {
            // Arrange
            var id = 13;
            var numberOfCategories = 5;
            var categoryInRepository = new Category("Name") { Id = id };

            CreateCategoriesInMockRepository(numberOfCategories);
            _categories.Add(categoryInRepository);

            // Act
            await _categoryServices.DeleteAsync(id);

            // Assert
            Assert.IsTrue(_categories.Count == numberOfCategories);
        }

        private void CreateCategoriesInMockRepository(int numberOfCategories)
        {
            for (int i = 0; i < numberOfCategories; i++)
            {
                _categories.Add(new Category("Category" + i) { Id = i });
            }
        }

        [TestMethod]
        public async Task CategoryServices_DeleteWithNoCategoryWithGivenId_NothingHappens()
        {
            // Arrange
            var id = 13;
            var numberOfCategories = 5;

            CreateCategoriesInMockRepository(numberOfCategories);

            // Act
            await _categoryServices.DeleteAsync(id);

            // Assert
            Assert.IsTrue(_categories.Count == numberOfCategories);
        }

        [TestMethod]
        public async Task CategoryServices_GetAllSubCategories_ReturnsListOfSubCategories()
        {
            // Arrange
            var id = 13;
            var numberOfSubCategories = 3;
            var category = new Category("HeadCategory") { Id = id };
            CreateCategoriesInMockRepository(5);
            _categories.Add(category);
            CreateSubCategoriesInMockRepository(numberOfSubCategories, id);

            // Act
            var result = await _categoryServices.GetSubCategoriesAsync(id);

            // Assert
            Assert.IsTrue(result.Count == numberOfSubCategories);
        }

        private void CreateSubCategoriesInMockRepository(int numberOfSubCategories, int headCategoryId)
        {
            for (int i = 0; i < numberOfSubCategories; i++)
            {
                _categories.Add(new Category("Category" + i) { Id = i, HeadCategoryId = headCategoryId });
            }
        }

        [TestMethod]
        public async Task CategoryServices_GetAllSubCategoriesWithNoSubCategories_ReturnsEmptyList()
        {
            // Arrange
            var id = 13;
            CreateCategoriesInMockRepository(5);

            // Act
            var result = await _categoryServices.GetSubCategoriesAsync(id);

            // Assert
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public async Task CategoryServices_GetAllFinalCategories_ReturnsAllCategoriesThatDoesNotHaveSubCategories()
        {
            // Arrange
            var finalCategory = new Category();
            var finalCategory2 = new Category();
            var finalCategory3 = new Category();

            var subCategory = new Category { SubCategories = new List<Category> { finalCategory3 } };
            var headCategory = new Category { SubCategories = new List<Category> { finalCategory2, subCategory } };

            _categories.AddRange(new List<Category> { headCategory, subCategory, finalCategory, finalCategory2, finalCategory3 });

            // Act
            var result = await _categoryServices.GetAllFinalCategoriesAsync();

            // Assert
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public async Task CategoryServices_IsFinalCategory_WhenCategoryDoesNotHaveSubCategories_ReturnsTrue()
        {
            // Arrange
            var id = 93;
            var category = new Category { Id = id };
            _categories.Add(category);

            // Act
            var result = await _categoryServices.IsFinalCategoryAsync(id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CategoryServices_IsFinalCategory_WhenCategoryHaveSubCategories_ReturnsTrue()
        {
            // Arrange
            var id = 93;
            var category = new Category { Id = id, SubCategories = new List<Category> { new Category() } };
            _categories.Add(category);

            // Act
            var result = await _categoryServices.IsFinalCategoryAsync(id);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CategoryServices_IsFinalCategory_WhenIdIsEqualToZero_ReturnsFalse()
        {
            // Arrange
            var id = 0;

            // Act
            var result = await _categoryServices.IsFinalCategoryAsync(id);

            // Assert
            Assert.IsFalse(result);
        }
    }
}