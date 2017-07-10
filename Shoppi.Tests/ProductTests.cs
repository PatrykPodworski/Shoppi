using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shoppi.Data.Models;

namespace Shoppi.Tests
{
    [TestClass]
    public class ProductTests
    {
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