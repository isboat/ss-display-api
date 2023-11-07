using System.Runtime.Serialization;

namespace Display.Shared.Exceptions
{
    public class AuthorizationPendingException : SystemException
    {
        public AuthorizationPendingException()
        {
        }

        public AuthorizationPendingException(string? message) : base(message)
        {
        }

        public AuthorizationPendingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AuthorizationPendingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
