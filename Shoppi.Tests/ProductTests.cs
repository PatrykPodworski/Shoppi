using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shoppi.Data.Models;
using System;

namespace Shoppi.Tests
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Product_EmptyName_ThrowsArgumentException()
        {
            var product = new Product("", new Category("Category"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Product_WhiteSpaceName_ThrowsArgumentException()
        {
            var product = new Product(" ", new Category("Category"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Product_NullCategory_ThrowsArgumentNullException()
        {
            var product = new Product("ProductName", null);
        }

        [TestMethod]
        public void Product_ValidName_SetsName()
        {
            //Arrange
            var productName = "ProductName";

            //Act
            var product = new Product(productName, new Category("Category"));

            //Assert
            Assert.AreEqual(product.Name, productName);
        }
    }
}