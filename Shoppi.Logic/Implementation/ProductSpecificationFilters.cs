using Shoppi.Logic.Abstract;

namespace Shoppi.Logic.Implementation
{
    public class PagedProductSpecificationFilters : IPagedProductFilters
    {
        public int Page { get; set; }
        public int ProductsPerPage { get; set; }
    }
}