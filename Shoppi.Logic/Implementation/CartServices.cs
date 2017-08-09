using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using System.Linq;

namespace Shoppi.Logic.Implementation
{
    public class CartServices : ICartServices
    {
        private ICartRepository _repository;

        public CartServices(ICartRepository repository)
        {
            _repository = repository;
        }

        public Cart GetCart()
        {
            return _repository.GetCart();
        }

        public void Add(Product product)
        {
            var cart = _repository.GetCart();
            var productLine = cart.Lines.FirstOrDefault(x => x.Product.Id == product.Id);
            if (productLine == null)
            {
                _repository.AddLine(product);
            }
            else
            {
                productLine.Quantity++;
            }
        }
    }
}