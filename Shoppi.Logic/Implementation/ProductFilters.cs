using Shoppi.Logic.Abstract;

namespace Shoppi.Logic.Implementation
{
    public class ProductFilters : IProductFilters
    {
        public int Page { get; set; }
        public int ProductsPerPage { get; set; }
        public string OrderBy { get; set; }
    }
}