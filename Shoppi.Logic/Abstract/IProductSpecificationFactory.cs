using Shoppi.Data.Models;
using Shoppi.Data.Specifications;

namespace Shoppi.Logic.Abstract
{
    public interface IProductSpecificationFactory
    {
        Specification<Product> GetResult(IProductFilters filters);
    }
}