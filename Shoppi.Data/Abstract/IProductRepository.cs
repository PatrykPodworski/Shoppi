using Shoppi.Data.Models;
using System;
using System.Collections.Generic;

namespace Shoppi.Data.Abstract
{
    public interface IProductRepository
    {
        Product GetById(int id);

        IEnumerable<Product> Get(Predicate<Product> predicate);
    }
}