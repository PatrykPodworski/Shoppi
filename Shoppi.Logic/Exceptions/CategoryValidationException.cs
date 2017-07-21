using System;

namespace Shoppi.Logic.Exceptions
{
    public class CategoryValidationException : Exception
    {
        public CategoryValidationException(string message) : base(message)
        {
        }
    }
}