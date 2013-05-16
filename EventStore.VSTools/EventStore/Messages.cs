using System;
using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools
{
    public abstract class CommandToEventStore : IMessage
    {
        public string Name { get; private set; }

        public string EventStoreAddress { get; private set; }

        protected CommandToEventStore(string eventStoreAddress, string name)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            EventStoreAddress = eventStoreAddress;
            Name = name;
        }
    }

    public sealed class DeployProjection : CommandToEventStore
    {
        public DeployProjection(string eventStoreAddress, string name, string content)
            : base(eventStoreAddress, name)
        {
            Content = content;
            Enable = true;
            EnableCheckpoint = true;
        }

        public string Content { get; private set; }
        public bool EnableEmit { get; set; }
        public bool Enable { get; set; }
        public bool EnableCheckpoint { get; set; }
    }

    public sealed class RunProjection : CommandToEventStore
    {
        public RunProjection(string eventStoreAddress, string name, string content)
            : base(eventStoreAddress, name)
        {
            Content = content;
        }

        public string Content { get; private set; }
    }

    public sealed class ProjectionExecuted : IEvent
    {
        public string Name { get; private set; }
        public string Uri { get; private set; }
        public string Result { get; private set; }

        public ProjectionExecuted(string name, string uri, string result)
        {
            Name = name;
            Uri = uri;
            Result = result;
        }
    }

    public abstract class ProjectionDeploymentEvent : IEvent
    {
        public string Name { get; private set; }

        protected ProjectionDeploymentEvent(string name) { Name = name; }
    }

    public sealed class ProjectionCreated : ProjectionDeploymentEvent
    {
        public ProjectionCreated(string name) : base(name) { }
    }

    public sealed class ProjectionUpdated : ProjectionDeploymentEvent
    {
        public ProjectionUpdated(string name) : base(name) { }
    }

    public sealed class ProjectionNotUpdatedBecauseNotChanged : ProjectionDeploymentEvent
    {
        public ProjectionNotUpdatedBecauseNotChanged(string name) : base(name) { }
    }
}
