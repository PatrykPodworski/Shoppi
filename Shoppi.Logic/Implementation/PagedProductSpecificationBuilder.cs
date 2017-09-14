using Shoppi.Data.Models;
using Shoppi.Data.Specifications;
using Shoppi.Logic.Implementation;

namespace Shoppi.Logic.Factories
{
    public class PagedProductSpecificationBuilder : ISpecificationBuilder<Product>
    {
        private readonly ProductSpecificationFilters _filters;

        public PagedProductSpecificationBuilder(ProductSpecificationFilters filters)
        {
            _filters = filters;
        }

        public Specification<Product> GetResult()
        {
            var productsToSkip = (_filters.Page - 1) * _filters.ProductsPerPage;

            return new Specification<Product>(x => true)
                .OrderBy(x => x.Name)
                .Skip(productsToSkip)
                .Take(_filters.ProductsPerPage);
        }
    }

    public interface ISpecificationBuilder<T>
    {
        Specification<T> GetResult();
    }
}