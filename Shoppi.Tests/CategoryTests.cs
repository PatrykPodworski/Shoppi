using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shoppi.Data.Models;
using System;

namespace Shoppi.Tests
{
    [TestClass]
    public class CategoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Category_EmptyName_ThrowsArgumentException()
        {
            var category = new Category("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Category_WhiteSpaceName_ThrowsArgumentException()
        {
            var category = new Category(" ");
        }

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