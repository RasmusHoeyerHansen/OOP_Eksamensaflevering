using System;
using System.Runtime.Serialization;

namespace Controller
{
    [Serializable]
    public class ProductAlreadyActivatedException : Exception
    {
        public ProductAlreadyActivatedException()
        {
        }

        public ProductAlreadyActivatedException(string message) : base(message)
        {
        }

        public ProductAlreadyActivatedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductAlreadyActivatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}