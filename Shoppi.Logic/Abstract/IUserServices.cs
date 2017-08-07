using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface IUserServices
    {
        Task SetDefaultAddressAsync(string userId, int addressId);
    }
}