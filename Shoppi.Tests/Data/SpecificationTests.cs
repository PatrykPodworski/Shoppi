using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shoppi.Data.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace Shoppi.Tests.Data
{
    [TestClass]
    public class SpecificationTests
    {
        [TestMethod]
        public void Specification_ReturnsAllItemsMatchingThePredicate()
        {
            // Arrange
            var spec = new Specification<int>(x => x >= 5);
            var collection = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            // Act
            var result = spec.ItemsSatisfyingSpecification(collection.AsQueryable());

            // Assert
            Assert.AreEqual(5, result.Count());
        }

        [TestMethod]
        public void Specification_OrderBy_ReturnItemsInCorrectOrder()
        {
            // Arrange
            var spec = new Specification<int>(x => true).OrderBy(x => x);
            var collection = new List<int> { 5, 4, 3, 6, 0, 9, 1, 2, 8, 7 };
            var expected = collection.OrderBy(x => x);

            // Act
            var result = spec.ItemsSatisfyingSpecification(collection.AsQueryable()).ToList();

            // Assert
            Assert.IsTrue(result.SequenceEqual(expected));
        }

        [TestMethod]
        public void Specification_OrderByDescending_ReturnItemsInCorrectOrder()
        {
            // Arrange
            var spec = new Specification<int>(x => true).OrderByDescending(x => x);
            var collection = new List<int> { 5, 4, 3, 6, 0, 9, 1, 2, 8, 7 };
            var expected = collection.OrderByDescending(x => x);

            // Act
            var result = spec.ItemsSatisfyingSpecification(collection.AsQueryable()).ToList();

            // Assert
            Assert.IsTrue(result.SequenceEqual(expected));
        }

        [TestMethod]
        public void Specification_Skip_ReturnItemsWithoutSkippedOnes()
        {
            // Arrange
            var spec = new Specification<int>(x => true).Skip(4);
            var collection = new List<int> { 5, 4, 3, 6, 0, 9, 1, 2, 8, 7 };
            var expected = collection.Skip(4);

            // Act
            var result = spec.ItemsSatisfyingSpecification(collection.AsQueryable()).ToList();

            // Assert
            Assert.IsTrue(result.SequenceEqual(expected));
        }

        [TestMethod]
        public void Specification_Take_ReturnXFirstItems()
        {
            // Arrange
            var spec = new Specification<int>(x => true).Take(6);
            var collection = new List<int> { 5, 4, 3, 6, 0, 9, 1, 2, 8, 7 };
            var expected = collection.Take(6);

            // Act
            var result = spec.ItemsSatisfyingSpecification(collection.AsQueryable()).ToList();

            // Assert
            Assert.IsTrue(result.SequenceEqual(expected));
        }
    }
}