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
        private ITypeServices _typeServices;

        private int _minimalQuantity = 1;

        public CartServices(ICartRepository repository, ITypeServices typeServices)
        {
            _repository = repository;
            _typeServices = typeServices;
        }

        public Cart GetCart()
        {
            return _repository.GetCart();
        }

        public async Task AddAsync(int typeId)
        {
            var cartLine = _repository.GetCartLine(typeId);

            if (cartLine != null)
            {
                _repository.IncrementCartLineQuantity(typeId);
            }
            else
            {
                var type = await _typeServices.GetByIdAsync(typeId);
                _repository.AddLine(type);
            }
        }

        public void Delete(int typeId)
        {
            _repository.DeleteLine(typeId);
        }

        public int DecrementCartLineQuantity(int typeId)
        {
            var cartLine = _repository.GetCartLine(typeId);

            if (cartLine == null)
            {
                return 0;
            }

            if (cartLine.Quantity == _minimalQuantity)
            {
                return _minimalQuantity;
            }

            _repository.DecrementCartLineQuantity(typeId);

            return cartLine.Quantity;
        }

        public int IncrementCartLineQuantity(int typeId)
        {
            var cartLine = _repository.GetCartLine(typeId);

            if (cartLine == null)
            {
                return 0;
            }

            _repository.IncrementCartLineQuantity(typeId);

            return cartLine.Quantity;
        }

        public int GetNumberOfProducts()
        {
            return _repository.GetCart().Lines
                .Sum(x => x.Quantity);
        }
    }
}