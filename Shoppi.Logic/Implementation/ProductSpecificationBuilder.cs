using Shoppi.Data.Models;
using Shoppi.Data.Specifications;
using Shoppi.Logic.Abstract;

namespace Shoppi.Logic.Implementation
{
    public class ProductSpecificationBuilder : ISpecificationBuilder<Product>
    {
        protected readonly IProductFilters _filters;

        public ProductSpecificationBuilder(IProductFilters filters)
        {
            _filters = filters;
        }

        public virtual Specification<Product> GetResult()
        {
            return new Specification<Product>(x => true);
        }
    }
}