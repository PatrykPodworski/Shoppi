using System;

namespace Shoppi.Logic.Exceptions
{
    public class BrandValidationException : Exception
    {
        public BrandValidationException(string message) : base(message)
        {
        }
    }
}