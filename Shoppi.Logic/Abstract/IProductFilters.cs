namespace Shoppi.Logic.Abstract
{
    public interface IProductFilters
    {
        int Page { get; set; }
        int ProductsPerPage { get; set; }
        string OrderBy { get; set; }
    }
}