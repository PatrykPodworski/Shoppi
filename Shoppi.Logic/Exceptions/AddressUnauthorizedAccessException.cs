using System;

namespace Shoppi.Logic.Exceptions
{
    public class AddressUnauthorizedAccessException : Exception
    {
        public AddressUnauthorizedAccessException(string message) : base(message)
        {
        }
    }
}