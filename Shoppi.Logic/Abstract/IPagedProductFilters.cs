namespace Shoppi.Logic.Abstract
{
    public interface IPagedProductFilters : IProductFilters
    {
        int Page { get; set; }
        int ProductsPerPage { get; set; }
    }
}