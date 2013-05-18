using System.Collections.Generic;
using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools.Tests
{
    public sealed class FakePublisher : IPublish<IMessage>
    {
        public readonly List<IMessage> PublishedMessages = new List<IMessage>(); 
        public void Publish(IMessage message)
        {
            PublishedMessages.Add(message);
        }
    }
}
