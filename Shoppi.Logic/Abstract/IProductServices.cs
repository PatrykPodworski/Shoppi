using Shoppi.Data.Models;
using System.Collections.Generic;

namespace Shoppi.Logic.Abstract
{
    public interface IProductServices
    {
        IEnumerable<Product> GetAll();
    }
}