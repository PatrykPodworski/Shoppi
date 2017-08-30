using Shoppi.Logic.Abstract;
using System.IO;
using System.Threading.Tasks;

namespace Shoppi.Logic.Implementation
{
    public class ImageServices : IImageServices
    {
        public async Task<byte[]> GetImage(string imagePath)
        {
            using (var stream = new FileStream(imagePath, FileMode.Open))
            {
                var image = new byte[stream.Length];
                await stream.ReadAsync(image, 0, (int)stream.Length);
                return image;
            }
        }
    }
}