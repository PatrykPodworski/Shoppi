using System;

namespace Shoppi.Logic.Exceptions
{
    public class InvalidProductNameException : Exception
    {
        public InvalidProductNameException(string message) : base(message)
        {
        }
    }
}