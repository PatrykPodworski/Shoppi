using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Exceptions;
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

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_CreateWithoutName_ThrowsException()
        {
            // Arrange
            var address = GenerateValidAddress();
            address.Name = null;

            // Act
            await _services.CreateAsync(address);
        }

        private Address GenerateValidAddress()
        {
            return new Address()
            {
                Name = "Name",
                AddressLine = "Adress line",
                City = "City",
                UserId = "UserId",
                Country = "Country",
                ZipCode = "Zip Code"
            };
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_CreateWithWhitespaceName_ThrowsException()
        {
            // Arrange
            var address = GenerateValidAddress();
            address.Name = "  ";

            // Act
            await _services.CreateAsync(address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_CreateWithoutAddressLine_ThrowsException()
        {
            // Arrange
            var address = GenerateValidAddress();
            address.AddressLine = null;

            // Act
            await _services.CreateAsync(address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_CreateWithWhitespaceAddressLine_ThrowsException()
        {
            // Arrange
            var address = GenerateValidAddress();
            address.AddressLine = "  ";

            // Act
            await _services.CreateAsync(address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_CreateWithoutCity_ThrowsException()
        {
            // Arrange
            var address = GenerateValidAddress();
            address.City = null;

            // Act
            await _services.CreateAsync(address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_CreateWithWhitespaceCity_ThrowsException()
        {
            // Arrange
            var address = GenerateValidAddress();
            address.City = "  ";

            // Act
            await _services.CreateAsync(address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_CreateWithoutCountry_ThrowsException()
        {
            // Arrange
            var address = GenerateValidAddress();
            address.Country = null;

            // Act
            await _services.CreateAsync(address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_CreateWithWhitespaceCountry_ThrowsException()
        {
            // Arrange
            var address = GenerateValidAddress();
            address.Country = "  ";

            // Act
            await _services.CreateAsync(address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_CreateWithoutZipCode_ThrowsException()
        {
            // Arrange
            var address = GenerateValidAddress();
            address.ZipCode = null;

            // Act
            await _services.CreateAsync(address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_CreateWithWhitespaceZipCode_ThrowsException()
        {
            // Arrange
            var address = GenerateValidAddress();
            address.ZipCode = "  ";

            // Act
            await _services.CreateAsync(address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_CreateWithoutUserId_ThrowsException()
        {
            // Arrange
            var address = GenerateValidAddress();
            address.UserId = null;

            // Act
            await _services.CreateAsync(address);
        }
    }
}