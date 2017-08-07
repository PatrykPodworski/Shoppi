using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}