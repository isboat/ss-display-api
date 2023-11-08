using System.Runtime.Serialization;

namespace Display.Shared.Exceptions
{
    public class InvalidDeviceIdException : SystemException
    {
        public InvalidDeviceIdException()
        {
        }

        public InvalidDeviceIdException(string? message) : base(message)
        {
        }

        public InvalidDeviceIdException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidDeviceIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
