using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface IImageServices
    {
        Task<byte[]> GetImage(string imagePath);
    }
}