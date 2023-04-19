using System.Runtime.Serialization;

namespace Display.Models.Exceptions
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

        protected AccessForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
