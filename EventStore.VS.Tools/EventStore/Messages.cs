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

        protected CommandToEventStore(string eventStoreAddress, string name, string content)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            EventStoreAddress = eventStoreAddress;
            Name = name;
            Content = content;
        }
    }

    public sealed class DeployProjection : CommandToEventStore
    {
        public DeployProjection(string eventStoreAddress, string name, string content) : base(eventStoreAddress, name, content) { }
    }

    public sealed class RunProjection : CommandToEventStore
    {
        public RunProjection(string eventStoreAddress, string name, string content)
            : base(eventStoreAddress, name, content) { }
    }

    public sealed class ProjectionExecuted : IEvent
    {
        public string Name { get; private set; }
        public string Result { get; private set; }

        public ProjectionExecuted(string name, string result)
        {
            Name = name;
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
