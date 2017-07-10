using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shoppi.Data.Models;

namespace Shoppi.Tests
{
    [TestClass]
    public class CategoryTests
    {
        [TestMethod]
        public void Category_ValidName_SetsName()
        {
            //Arrange
            var categoryName = "CategoryName";

            //Act
            var category = new Category(categoryName);

            //Assert
            Assert.AreEqual(category.Name, categoryName);
        }
    }
}