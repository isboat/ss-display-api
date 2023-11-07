using System.Runtime.Serialization;

namespace Display.Shared.Exceptions
{
    public class AccessExpiredException : Exception
    {
        public AccessExpiredException()
        {
        }

        public AccessExpiredException(string? message) : base(message)
        {
        }

        public AccessExpiredException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AccessExpiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
