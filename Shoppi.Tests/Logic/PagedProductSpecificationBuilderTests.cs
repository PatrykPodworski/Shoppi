using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shoppi.Data.Models;
using Shoppi.Data.Specifications;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Implementation;

namespace Shoppi.Tests.Logic
{
    [TestClass]
    public class PagedProductSpecificationBuilderTests
    {
        [TestMethod]
        public void PagedProductSpecificationBuilder_ReturnsSpecificationOfProducts()
        {
            // Arrange
            var filters = new Mock<IPagedProductFilters>();
            var builder = new PagedProductSpecificationBuilder(filters.Object);

            // Act
            var result = builder.GetResult();

            // Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Specification<Product>));
        }
    }
}