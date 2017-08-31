using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Implementation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppi.Tests.Logic
{
    [TestClass]
    public class BrandServicesTests
    {
        private BrandServices _brandServices;
        private Mock<IBrandRepository> _mockRepository;
        private List<Brand> _brands;

        [TestInitialize]
        public void TestInitialize()
        {
            _brands = new List<Brand>();
            _mockRepository = new Mock<IBrandRepository>();
            SetupMockRepository();

            _brandServices = new BrandServices(_mockRepository.Object);
        }

        private void SetupMockRepository()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns<int>(x => Task.Run(() => _brands.FirstOrDefault(y => y.Id == x)));
        }

        [TestMethod]
        public async Task BrandServices_GetById_WhenThereIsNoBrandWithGivenId_ReturnsNull()
        {
            // Arrange
            var id = 13;

            // Act
            var result = await _brandServices.GetByIdAsync(id);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task BrandServices_GetById_ReturnsBrandWithGivenId()
        {
            // Arrange
            var id = 13;
            var brand = new Brand { Id = id };
            _brands.Add(brand);

            // Act
            var result = await _brandServices.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(brand, result);
        }
    }
}