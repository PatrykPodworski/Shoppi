﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using Shoppi.Logic.Exceptions;
using Shoppi.Logic.Implementation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppi.Tests.Logic
{
    [TestClass]
    public class UserServicesTests
    {
        private List<ShoppiUser> _users;
        private UserServices _userServices;
        private Mock<IUserRepository> _mockRepository;
        private Mock<IAddressServices> _mockAddressServices;
        private List<Address> _addresses;

        public UserServicesTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            SetUpMockRepository();

            _mockAddressServices = new Mock<IAddressServices>();
            SetUpMockAddressServices();

            _userServices = new UserServices(_mockRepository.Object, _mockAddressServices.Object);

            _users = new List<ShoppiUser>();
            _addresses = new List<Address>();
        }

        private void SetUpMockRepository()
        {
            _mockRepository.Setup(x => x.SetDefaultAddressIdAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns<string, int>((uId, aId) => Task.Run(() => _users.FirstOrDefault(u => u.Id == uId).DefaultAddressId = aId));

            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .Returns<string>(x => Task.Run(() => _users.FirstOrDefault(u => u.Id == x)));
        }

        private void SetUpMockAddressServices()
        {
            _mockAddressServices.Setup(x => x.DoesAddressBelongsToUserAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns<string, int>((uId, aId) => Task.Run(() => _addresses.FirstOrDefault(x => x.Id == aId).UserId == uId));
        }

        [TestMethod]
        [ExpectedException(typeof(AddressUnauthorizedAccessException))]
        public async Task UserServices_SetDefaultAddressWithAddressThatDoesNotBelongToUser_ThrowsException()
        {
            // Arrange
            var userId = "UserId";
            var addressId = 3;

            var address = new Address() { UserId = "AnotherUserId", Id = addressId };
            _addresses.Add(address);

            // Act
            await _userServices.SetDefaultAddressAsync(userId, address.Id);
        }

        [TestMethod]
        public async Task UserServices_SetDefaultAddressWhenAddressWithProperUserIdIsGiven_SetsUserDefaultAddressId()
        {
            // Arrange
            var userId = "UserId";
            var addressId = 4;

            var address = new Address() { UserId = userId, Id = addressId };
            _addresses.Add(address);

            var user = new ShoppiUser() { Id = userId };
            _users.Add(user);

            // Act
            await _userServices.SetDefaultAddressAsync(userId, addressId);

            // Assert
            Assert.IsTrue(_users[0].DefaultAddressId == addressId);
        }

        [TestMethod]
        public async Task UserServices_GetByIdAsync_ReturnsUserWithGivenId()
        {
            // Arrange
            var userId = "UserId";
            var user = new ShoppiUser() { Id = userId };
            _users.Add(user);

            // Act
            var result = await _userServices.GetByIdAsync(userId);

            // Assert
            Assert.AreEqual(user, result);
        }

        [TestMethod]
        public async Task UserServices_GetByIdAsyncWhenThereIsNotUserWithGivenId_ReturnsNull()
        {
            // Arrange
            var userId = "UserId";
            var user = new ShoppiUser() { Id = "AnotherUserId" };
            _users.Add(user);

            // Act
            var result = await _userServices.GetByIdAsync(userId);

            // Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public async Task UserServices_GetUsersDefaultAddressIdAsync_WhenUserDoesNotHaveDefaultAddress_ReturnsNull()
        {
            // Arrange
            var userId = "UserId";
            var user = new ShoppiUser { Id = userId };
            _users.Add(user);

            // Act
            var result = await _userServices.GetUsersDefaultAddressIdAsync(userId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UserServices_GetUsersDefaultAddressIdAsync_WhenUserHaveDefaultAddress_ReturnsItsId()
        {
            // Arrange
            var userId = "UserId";
            var defaultAddressId = 13;
            var user = new ShoppiUser { Id = userId, DefaultAddressId = defaultAddressId };
            _users.Add(user);

            // Act
            var result = await _userServices.GetUsersDefaultAddressIdAsync(userId);

            // Assert
            Assert.IsTrue(result == defaultAddressId);
        }
    }
}