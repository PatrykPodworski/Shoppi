using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using Shoppi.Logic.Abstract;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppi.Logic.Implementation
{
    public class CartServices : ICartServices
    {
        private ICartRepository _repository;
        private IProductServices _productServices;

        public CartServices(ICartRepository repository, IProductServices productServices)
        {
            _repository = repository;
            _productServices = productServices;
        }

        public Cart GetCart()
        {
            return _repository.GetCart();
        }

        public async Task AddAsync(int productId)
        {
            var cart = _repository.GetCart();

            var productLine = cart.Lines.FirstOrDefault(x => x.Product.Id == productId);

            if (productLine == null)
            {
                var product = await _productServices.GetByIdAsync(productId);
                _repository.AddLine(product);
            }
            else
            {
                productLine.Quantity++;
            }
        }

        public void Delete(int productId)
        {
            var cart = _repository.GetCart();
            _repository.DeleteLine(productId);
        }

        public int DecrementProductQuantity(int productId)
        {
            var cart = _repository.GetCart();
            var quantity = cart.Lines.FirstOrDefault(x => x.Product.Id == productId).Quantity;
            if (quantity > 1)
            {
                quantity = --cart.Lines.FirstOrDefault(x => x.Product.Id == productId).Quantity;
            }

            return quantity;
        }

        public int IncrementProductQuantity(int productId)
        {
            var cart = _repository.GetCart();
            return ++cart.Lines.FirstOrDefault(x => x.Product.Id == productId).Quantity;
        }
    }
}