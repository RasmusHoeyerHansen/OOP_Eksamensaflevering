using System;
using System.Runtime.Serialization;

namespace Controller
{
    [Serializable]
    public class ProductAlreadyDeactivatedException : Exception
    {
        public ProductAlreadyDeactivatedException()
        {
        }

        public ProductAlreadyDeactivatedException(string message) : base(message)
        {
        }

        public ProductAlreadyDeactivatedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductAlreadyDeactivatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}