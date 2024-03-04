using System.Runtime.Serialization;

namespace Display.Shared.Exceptions
{
    public class DeviceNotFoundException : Exception
    {
        public DeviceNotFoundException()
        {
        }

        public DeviceNotFoundException(string? message) : base(message)
        {
        }

        public DeviceNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DeviceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
