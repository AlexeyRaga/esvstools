using System;
using System.Net;

namespace EventStore.VSTools.EventStore
{
    [Serializable]
    public class EventStoreConnectionException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public EventStoreConnectionException()
        {
        }

        public EventStoreConnectionException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
