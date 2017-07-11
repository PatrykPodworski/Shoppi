using System;

namespace Shoppi.Logic.Exceptions
{
    public class NegativeProductQuantityException : Exception
    {
        public NegativeProductQuantityException(string message) : base(message)
        {
        }
    }
}