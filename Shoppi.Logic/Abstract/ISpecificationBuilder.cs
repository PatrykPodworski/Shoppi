using Shoppi.Data.Specifications;

namespace Shoppi.Logic.Abstract
{
    public interface ISpecificationBuilder<T>
    {
        Specification<T> GetResult();
    }
}