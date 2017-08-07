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
            SetupGetByUserIdMethod();
            SetubGetByIdMethod();
            SetupDeleteMethod();
        }

        private void SetupCreateMethod()
        {
            _mockRepository.Setup(x => x.Create(It.IsAny<Address>()))
                .Callback<Address>(x => _addresses.Add(x));
        }

        private void SetupGetByUserIdMethod()
        {
            _mockRepository.Setup(x => x.GetByUserIdAsync(It.IsAny<string>()))
                .Returns<string>(x => Task.Run(() => _addresses.Where(y => y.UserId == x).ToList()));
        }

        private void SetubGetByIdMethod()
        {
            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns<int>(x => Task.Run(() => _addresses.FirstOrDefault(a => a.Id == x)));
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

        [TestMethod]
        public async Task AddressServices_GetById_ReturnsAddressWithGivenId()
        {
            // Arrange
            var id = 11;
            var numberOfAddresses = 5;
            var address = new Address() { Id = id };
            CreateAddressesInMockRepository(numberOfAddresses);
            _addresses.Add(address);

            // Act
            var result = await _services.GetByIdAsync(id);

            // Assert
            Assert.IsTrue(address.Equals(result));
        }

        [TestMethod]
        public async Task AddressServices_GetByIdWithNoMatch_ReturnsNull()
        {
            // Arrange
            var id = 11;
            var numberOfAddresses = 5;
            CreateAddressesInMockRepository(numberOfAddresses);

            // Act
            var result = await _services.GetByIdAsync(id);

            // Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressUnauthorizedAccessException))]
        public async Task AddressServices_DeleteUserAddressWithWrongUserId_ThrowsException()
        {
            // Arrange
            var id = 13;
            var userId = "UserId";
            var address = new Address() { Id = id, UserId = "AnotherUserId" };
            _addresses.Add(address);

            // Act
            await _services.DeleteUserAddressAsync(userId, id);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressUnauthorizedAccessException))]
        public async Task AddressServices_DeleteUserAddressWithNotExisingAddress_ThrowsException()
        {
            // Arrange
            var id = 13;

            // Act
            await _services.DeleteUserAddressAsync("userId", id);
        }

        [TestMethod]
        public async Task AddressServices_DeleteUserAddressWithCorrectData_DeletesAddressFromRepository()
        {
            // Arrange
            var id = 13;
            var numberOfAddresses = 7;
            var userId = "UserId";
            var address = new Address() { Id = id, UserId = userId };
            CreateAddressesInMockRepository(numberOfAddresses);
            _addresses.Add(address);

            // Act
            await _services.DeleteUserAddressAsync(userId, id);

            // Assert
            Assert.IsTrue(_addresses.Count == numberOfAddresses);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressUnauthorizedAccessException))]
        public async Task AddressServices_GetUserAddressByIdWithWrongUserId_ThrowsException()
        {
            // Arrange
            var id = 13;
            var userId = "UserId";
            var address = new Address() { Id = id, UserId = "AnotherUserId" };
            _addresses.Add(address);

            // Act
            var result = await _services.GetUserAddressByIdAsync(userId, id);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressUnauthorizedAccessException))]
        public async Task AddressServices_GetUserAddressByIdWithNotExistingAddress_ThrowsException()
        {
            // Arrange
            var id = 13;
            var userId = "UserId";

            var result = await _services.GetUserAddressByIdAsync(userId, id);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressUnauthorizedAccessException))]
        public async Task AddressServices_EditUserAddressWithWrongUserId_ThrowsException()
        {
            // Arrange
            var id = 13;
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.Id = id;
            address.UserId = "AnotherUserId";
            _addresses.Add(address);

            // Act
            await _services.EditUserAddressAsync(userId, address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressUnauthorizedAccessException))]
        public async Task AddressServices_EditUserAddressWithNotExistingAddress_ThrowsException()
        {
            // Arrange
            var id = 13;
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.Id = id;
            address.UserId = userId;

            // Act
            await _services.EditUserAddressAsync(userId, address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_EditUserAddressWithoutName_ThrowsException()
        {
            // Arrange
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.Name = null;
            address.UserId = userId;
            _addresses.Add(address);

            // Act
            await _services.EditUserAddressAsync(userId, address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_EditUserAddressWithWhitespaceName_ThrowsException()
        {
            // Arrange
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.Name = "  ";
            address.UserId = userId;
            _addresses.Add(address);

            // Act
            await _services.EditUserAddressAsync(userId, address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_EditUserAddressWithoutAddressLine_ThrowsException()
        {
            // Arrange
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.AddressLine = null;
            address.UserId = userId;
            _addresses.Add(address);

            // Act
            await _services.EditUserAddressAsync(userId, address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_EditUserAddressWithWhitespaceAddressLine_ThrowsException()
        {
            // Arrange
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.AddressLine = "  ";
            address.UserId = userId;
            _addresses.Add(address);

            // Act
            await _services.EditUserAddressAsync(userId, address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_EditUserAddressWithoutCity_ThrowsException()
        {
            // Arrange
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.City = null;
            address.UserId = userId;
            _addresses.Add(address);

            // Act
            await _services.EditUserAddressAsync(userId, address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_EditUserAddressWithWhitespaceCity_ThrowsException()
        {
            // Arrange
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.City = "  ";
            address.UserId = userId;
            _addresses.Add(address);

            // Act
            await _services.EditUserAddressAsync(userId, address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_EditUserAddressWithoutZipCode_ThrowsException()
        {
            // Arrange
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.ZipCode = null;
            address.UserId = userId;
            _addresses.Add(address);

            // Act
            await _services.EditUserAddressAsync(userId, address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_EditUserAddressWithWhitespaceZipCode_ThrowsException()
        {
            // Arrange
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.ZipCode = "  ";
            address.UserId = userId;
            _addresses.Add(address);

            // Act
            await _services.EditUserAddressAsync(userId, address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_EditUserAddressWithoutCountry_ThrowsException()
        {
            // Arrange
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.Country = null;
            address.UserId = userId;
            _addresses.Add(address);

            // Act
            await _services.EditUserAddressAsync(userId, address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_EditUserAddressWithWhitespaceCountry_ThrowsException()
        {
            // Arrange
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.Country = "  ";
            address.UserId = userId;
            _addresses.Add(address);

            // Act
            await _services.EditUserAddressAsync(userId, address);
        }

        [TestMethod]
        [ExpectedException(typeof(AddressValidationException))]
        public async Task AddressServices_EditUserAddressWithoutUserId_ThrowsException()
        {
            // Arrange
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.UserId = null;
            _addresses.Add(address);

            // Act
            await _services.EditUserAddressAsync(userId, address);
        }

        [TestMethod]
        public async Task AddressServices_EditUserAddressWithValidAddress_ChangesAddressValues()
        {
            // Arrange
            var userId = "UserId";
            var address = GenerateValidAddress();
            address.UserId = userId;
            _addresses.Add(address);

            // Act
            ChangeAddressValues(address);
            await _services.EditUserAddressAsync(userId, address);

            // Assert
            Assert.IsTrue(IsAddressEdited(_addresses[0]));
        }

        private void ChangeAddressValues(Address address)
        {
            address.Name += "Changed";
            address.AddressLine += "Changed";
            address.City += "Changed";
            address.ZipCode += "Changed";
            address.Country += "Changed";
        }

        private bool IsAddressEdited(Address address)
        {
            var sampleAddress = GenerateValidAddress();
            ChangeAddressValues(sampleAddress);

            if (sampleAddress.Name != address.Name)
            {
                return false;
            }

            if (sampleAddress.AddressLine != address.AddressLine)
            {
                return false;
            }
            if (sampleAddress.City != address.City)
            {
                return false;
            }
            if (sampleAddress.ZipCode != address.ZipCode)
            {
                return false;
            }
            if (sampleAddress.Country != address.Country)
            {
                return false;
            }

            return true;
        }

        [TestMethod]
        public async Task AddressServices_DoesAddressBelongsToUserWhenCorrectUserIdIsGiven_ReturnsTrue()
        {
            // Arrange
            var userId = "UserId";
            var addressId = 5;
            var address = GenerateValidAddress();
            address.UserId = userId;
            address.Id = addressId;
            _addresses.Add(address);

            // Act
            var result = await _services.DoesAddressBelongsToUserAsync(userId, addressId);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task AddressServices_DoesAddressBelongsToUserWhenWrongUserIdIsGiven_ReturnsFalse()
        {
            // Arrange
            var userId = "UserId";
            var addressId = 5;
            var address = GenerateValidAddress();
            address.UserId = "AnotherUserId";
            address.Id = addressId;
            _addresses.Add(address);

            // Act
            var result = await _services.DoesAddressBelongsToUserAsync(userId, addressId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}