using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Display.Shared.Exceptions
{
    public class AccessForbiddenException : Exception
    {
        public AccessForbiddenException()
        {
        }

        public AccessForbiddenException(string? message) : base(message)
        {
        }

        public AccessForbiddenException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AccessForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
