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
        private Mock<IAddressRepository> _mockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IAddressRepository>();
            SetupMockMethods();

            _services = new AddressServices(_mockRepository.Object);
            _addresses = new List<Address>();
        }

        private void SetupMockMethods()
        {
            SetupCreateMethod();
            SetupGetAllMethod();
            SetupDeleteMethod();
        }

        private void SetupCreateMethod()
        {
            _mockRepository.Setup(x => x.Create(It.IsAny<Address>()))
                .Callback<Address>(x => _addresses.Add(x));
        }

        private void SetupGetAllMethod()
        {
            _mockRepository.Setup(x => x.GetByUserIdAsync(It.IsAny<string>()))
                .Returns<string>(x => Task.Run(() => _addresses.Where(y => y.UserId == x).ToList()));
        }

        private void SetupDeleteMethod()
        {
            _mockRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .Callback<int>(x => _addresses.RemoveAll(a => a.Id == x));
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

        [TestMethod]
        public async Task AddressServices_CreateWithValidAddress_AddsAddressToRepository()
        {
            // Arrange
            var address = GenerateValidAddress();

            // Act
            await _services.CreateAsync(address);

            // Assert
            Assert.IsTrue(_addresses.Count == 1);
        }

        [TestMethod]
        public async Task AddressServices_Delete_DeletesAddressWithGivenId()
        {
            // Arrange
            var id = 11;
            var numberOfAddresses = 5;
            CreateAddressesInMockRepository(numberOfAddresses);
            _addresses.Add(new Address() { Id = id });

            // Act
            await _services.DeleteAsync(id);

            // Assert
            Assert.IsTrue(_addresses.Count == numberOfAddresses);
        }

        private void CreateAddressesInMockRepository(int numberOfAddresses)
        {
            _addresses.AddRange(
                Enumerable.Range(0, numberOfAddresses)
                .Select(x => new Address() { Id = x }));
        }

        [TestMethod]
        public async Task AddressServices_DeleteWithoutMatch_NothingHappens()
        {
            // Arrange
            var id = 11;
            var numberOfAddresses = 5;
            CreateAddressesInMockRepository(numberOfAddresses);

            // Act
            await _services.DeleteAsync(id);

            // Assert
            Assert.IsTrue(_addresses.Count == numberOfAddresses);
        }
    }
}