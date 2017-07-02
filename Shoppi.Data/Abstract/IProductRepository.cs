﻿using Shoppi.Data.Models;
using System.Collections.Generic;

namespace Shoppi.Data.Abstract
{
    public interface IProductRepository
    {
        Product GetById(int id);

        IEnumerable<Product> GetAll();
    }
}