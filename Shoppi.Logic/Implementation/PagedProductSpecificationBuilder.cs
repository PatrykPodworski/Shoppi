using Shoppi.Data.Models;
using Shoppi.Data.Specifications;
using Shoppi.Logic.Abstract;

namespace Shoppi.Logic.Implementation
{
    public class PagedProductSpecificationBuilder : ProductSpecificationBuilder
    {
        private new IPagedProductFilters _filters;

        public PagedProductSpecificationBuilder(IPagedProductFilters filters) : base(filters)
        {
        }

        public override Specification<Product> GetResult()
        {
            var productsToSkip = (_filters.Page - 1) * _filters.ProductsPerPage;

            return base.GetResult()
                .Skip(productsToSkip)
                .Take(_filters.ProductsPerPage);
        }
    }
}