using System;

namespace Shoppi.Logic.Exceptions
{
    public class AddressValidationException : Exception
    {
        public AddressValidationException(string message) : base(message)
        {
        }
    }
}