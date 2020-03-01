using System;
using System.Runtime.Serialization;

namespace ATM.MQ.Application.Exceptions
{
    public class InvalidMessageDataException : Exception
    {
        public InvalidMessageDataException()
        {
        }

        public InvalidMessageDataException(string message) : base(message)
        {
        }

        public InvalidMessageDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidMessageDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
