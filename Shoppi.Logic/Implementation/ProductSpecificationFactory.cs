using Shoppi.Data.Models;
using Shoppi.Data.Specifications;
using Shoppi.Logic.Abstract;

namespace Shoppi.Logic.Implementation
{
    public class ProductSpecificationFactory : IProductSpecificationFactory
    {
        public Specification<Product> GetResult(IProductFilters filters)
        {
            var productsToSkip = (filters.Page - 1) * filters.ProductsPerPage;

            return new Specification<Product>(x => true)
                .OrderBy(x => x.Name)
                .Skip(productsToSkip)
                .Take(filters.ProductsPerPage);
        }
    }
}