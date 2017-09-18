using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shoppi.Data.Models;
using Shoppi.Data.Specifications;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Implementation;

namespace Shoppi.Tests.Logic
{
    [TestClass]
    public class ProductSpecificationFactoryTests
    {
        [TestMethod]
        public void ProductSpecificationFactory_ReturnsSpecificationOfProducts()
        {
            // Arrange
            var filters = new Mock<IProductFilters>();
            var builder = new ProductSpecificationFactory();

            // Act
            var result = builder.GetResult(filters.Object);

            // Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Specification<Product>));
        }
    }
}