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
        public void SessionCartRepository_GetCart_WhenThereAlreadyIsCartInSession_ReturnsCart()
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
        public void SessionCartRepository_AddLine_AddsNewLineToCart()
        {
            // Arrange
            var cart = new Cart();
            var productId = 8;
            var product = new Product() { Id = productId };
            HttpContext.Current.Session["Cart"] = cart;

            // Act
            _repository.AddLine(product);

            // Assert
            Assert.IsTrue(cart.Lines.Count == 1);
            Assert.IsTrue(cart.Lines[0].Product.Id == productId);
        }
    }
}