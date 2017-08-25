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

        public int Remove(int productId)
        {
            var cart = _repository.GetCart();
            var newQuantity = --cart.Lines.FirstOrDefault(x => x.Product.Id == productId).Quantity;
            if (newQuantity == 0)
            {
                _repository.DeleteLine(productId);
            }
            return newQuantity;
        }
    }
}