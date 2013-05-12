using System.Net;
using EventStore.VS.Tools.Infrastructure;
using System;

namespace EventStore.VS.Tools
{
    public abstract class CommandToEventStore : IMessage
    {
        public string Name { get; private set; }
        public string Content { get; private set; }

        public string EventStoreAddress { get; private set; }

        protected CommandToEventStore(string eventStoreEndPoint, string name, string content)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            EventStoreAddress = eventStoreEndPoint;
            Name = name;
            Content = content;
        }
    }

    public sealed class DeployProjection : CommandToEventStore
    {
        public DeployProjection(string eventStoreEndPoint, string name, string content) : base(eventStoreEndPoint, name, content) { }
    }
}
