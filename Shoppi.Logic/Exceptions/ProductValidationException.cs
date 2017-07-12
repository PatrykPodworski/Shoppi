using System;

namespace Shoppi.Logic.Exceptions
{
    public class ProductValidationException : Exception
    {
        public ProductValidationException(string message) : base(message)
        {
        }
    }
}