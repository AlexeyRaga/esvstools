using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools.EventStore
{
    public abstract class ErrorEvent : IEvent
    {
        public string Title { get; private set; }
        public string Message { get; private set; }

        protected ErrorEvent(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }

    public sealed class EventStoreConnectionError : ErrorEvent
    {
        public EventStoreConnectionError(string title, string message) : base(title, message)
        {
        }
    }
}
