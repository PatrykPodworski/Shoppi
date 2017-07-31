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
    public class AddressServicesTests
    {
        private List<Address> _addresses;
        private AddressServices _services;
        private Mock<IAddressRepository> _MockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new Mock<IAddressRepository>();
            SetupMockMethods();

            _services = new AddressServices(_MockRepository.Object);
            _addresses = new List<Address>();
        }

        private void SetupMockMethods()
        {
            SetupGetAllMethod();
        }

        private void SetupGetAllMethod()
        {
            _MockRepository.Setup(x => x.GetByUserIdAsync(It.IsAny<string>()))
                .Returns<string>(x => Task.Run(() => _addresses.Where(y => y.UserId == x).ToList()));
        }

        [TestMethod]
        public async Task AddressServices_GetByUserId_ReturnsAllAddressesWithGivenUserId()
        {
            // Arrange
            int numberOfAddresses = 5;
            string userId = "userId";
            AddAddressesWithUserIdToRepository(numberOfAddresses, userId);
            AddAddressesWithUserIdToRepository(4, "otherUserId");

            // Act
            var result = await _services.GetByUserIdAsync(userId);

            // Assert
            Assert.IsTrue(result.Count == numberOfAddresses);
        }

        private void AddAddressesWithUserIdToRepository(int numberOfAddresses, string userId)
        {
            _addresses.AddRange(
                Enumerable.Range(0, numberOfAddresses)
                .Select(x => new Address() { UserId = userId, AddressLine = x.ToString() }));
        }

        [TestMethod]
        public async Task AddressServices_GetByUserIdWithNoMatches_ReturnsEmptyList()
        {
            // Arrange
            string userId = "userId";
            AddAddressesWithUserIdToRepository(4, "otherUserId");

            // Act
            var result = await _services.GetByUserIdAsync(userId);

            // Assert
            Assert.IsTrue(result.Count == 0);
        }
    }
}