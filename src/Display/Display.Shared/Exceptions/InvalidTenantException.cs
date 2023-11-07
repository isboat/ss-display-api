using System.Runtime.Serialization;

namespace Display.Shared.Exceptions
{
    public class InvalidTenantException : SystemException
    {
        public InvalidTenantException()
        {
        }

        public InvalidTenantException(string? message) : base(message)
        {
        }

        public InvalidTenantException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidTenantException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
