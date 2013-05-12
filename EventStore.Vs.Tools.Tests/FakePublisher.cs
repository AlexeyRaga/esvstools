using System.Collections.Generic;
using EventStore.VS.Tools.Infrastructure;

namespace EventStore.Vs.Tools.Tests
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
