using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shoppi.Data.Models;
using Shoppi.Data.Repositories;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace Shoppi.Tests.Repositories
{
    [TestClass]
    public class SessionCartRepositoryTests
    {
        private SessionCartRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new SessionCartRepository();
            CreateHttpContext();
        }

        private void CreateHttpContext()
        {
            var httpRequest = new HttpRequest("", "http://tempurl.com", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);
            var sessionContainer = CreateSessionContainer();
            SessionStateUtility.AddHttpSessionStateToContext(httpContext, sessionContainer);
            HttpContext.Current = httpContext;
        }

        private HttpSessionStateContainer CreateSessionContainer()
        {
            return new HttpSessionStateContainer(
                 "id",
                 new SessionStateItemCollection(),
                 new HttpStaticObjectsCollection(),
                 5,
                 true,
                 HttpCookieMode.AutoDetect,
                 SessionStateMode.InProc,
                 false);
        }

        [TestMethod]
        public void SessionCartRepository_GetCart_WhenThereIsNoCartInSession_ReturnsNewCart()
        {
            // Act
            var result = _repository.GetCart();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Cart));
        }

        [TestMethod]
        public void SessionCartRepository_GetCart_WhenThereIsACartInSession_ReturnsCart()
        {
            // Arrange
            var cart = new Cart();
            HttpContext.Current.Session["Cart"] = cart;

            // Act
            var result = _repository.GetCart();

            // Arrange
            Assert.AreSame(cart, result);
        }

        [TestMethod]
        public void SessionCartRepository_AddLine_AddsNewLineToCartWithProperType()
        {
            // Arrange
            var cart = new Cart();

            var typeId = 55;
            var type = new ProductType { Id = typeId };

            HttpContext.Current.Session["Cart"] = cart;

            // Act
            _repository.AddLine(type);

            // Assert
            Assert.AreEqual(type, cart.Lines[0].Type);
        }

        [TestMethod]
        public void SessionCartRepository_AddLine_AddsNewLineWithQuantityEqualToOne()
        {
            // Arrange
            var cart = new Cart();

            var typeId = 55;
            var type = new ProductType { Id = typeId };

            HttpContext.Current.Session["Cart"] = cart;

            // Act
            _repository.AddLine(type);

            // Assert
            Assert.AreEqual(1, cart.Lines[0].Quantity);
        }

        [TestMethod]
        public void SessionCartRepository_GetCartLine_ReturnsCartLineWithGivenType()
        {
            // Arrange
            var cart = new Cart();

            var typeId = 33;
            var type = new ProductType { Id = typeId };

            cart.Lines.Add(new CartLine { Type = type });
            HttpContext.Current.Session["Cart"] = cart;

            // Act
            var result = _repository.GetCartLine(typeId);

            // Assert
            Assert.AreEqual(cart.Lines[0], result);
        }

        [TestMethod]
        public void SessionCartRepository_DeleteLine_RemovesCartLineFromCart()
        {
            // Arrange

            var typeId = 93;
            var type = new ProductType { Id = typeId };

            var cart = new Cart();
            cart.Lines.Add(new CartLine { Type = type });

            HttpContext.Current.Session["Cart"] = cart;

            // Act
            _repository.DeleteLine(typeId);

            // Assert
            Assert.AreEqual(0, cart.Lines.Count);
        }

        [TestMethod]
        public void SessionCartRepository_IncrementCartLineQuantity_IncrementsCartLinQiantity()
        {
            // Arrange

            var typeId = 93;
            var type = new ProductType { Id = typeId };
            var quantity = 55;

            var cart = new Cart();
            cart.Lines.Add(new CartLine { Type = type, Quantity = quantity });

            HttpContext.Current.Session["Cart"] = cart;

            // Act
            _repository.IncrementCartLineQuantity(typeId);

            // Assert
            Assert.AreEqual(quantity + 1, cart.Lines[0].Quantity);
        }
    }
}