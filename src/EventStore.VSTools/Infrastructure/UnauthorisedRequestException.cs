using System;
using System.Runtime.Serialization;

namespace EventStore.VSTools.Infrastructure
{
    [Serializable]
    public class UnauthorisedRequestException : Exception
    {
        public UnauthorisedRequestException()
        {
        }

        public UnauthorisedRequestException(string message) : base(message)
        {
        }

        public UnauthorisedRequestException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UnauthorisedRequestException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
