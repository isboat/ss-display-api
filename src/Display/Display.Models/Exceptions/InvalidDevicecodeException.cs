using System.Runtime.Serialization;

namespace Display.Models.Exceptions
{
    public class InvalidDevicecodeException : Exception
    {
        public InvalidDevicecodeException()
        {
        }

        public InvalidDevicecodeException(string? message) : base(message)
        {
        }

        public InvalidDevicecodeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidDevicecodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
