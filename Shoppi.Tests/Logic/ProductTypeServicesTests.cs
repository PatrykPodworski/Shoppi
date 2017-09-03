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
    public class ProductTypeServicesTests
    {
        private Mock<ITypeRepository> _mockRepository;
        private TypeServices _typeServices;
        private List<ProductType> _types;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<ITypeRepository>();
            SetupMockRepository();

            _typeServices = new TypeServices(_mockRepository.Object);
            _types = new List<ProductType>();
        }

        private void SetupMockRepository()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns<int>(x => Task.Run(() => _types.FirstOrDefault(t => t.Id == x)));
        }

        [TestMethod]
        public async Task ProductTypeServices_GetById_ReturnsTypeWithGivenIdAsync()
        {
            // Arrange
            var id = 32;
            var type = new ProductType { Id = id };
            _types.Add(type);

            // Act
            var result = await _typeServices.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(type, result);
        }
    }
}