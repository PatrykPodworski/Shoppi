using Shoppi.Data.Abstract;
using Shoppi.Data.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Shoppi.Data.Repositories
{
    public class TypeRepository : ITypeRepository
    {
        private ShoppiDbContext _context;

        public TypeRepository(ShoppiDbContext context)
        {
            _context = context;
        }

        public async Task<ProductType> GetByIdAsync(int id)
        {
            return await _context.Types.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}