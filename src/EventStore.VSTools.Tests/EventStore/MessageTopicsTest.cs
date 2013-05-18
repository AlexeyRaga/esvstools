using EventStore.VSTools.Infrastructure;
using NUnit.Framework;

namespace EventStore.VSTools.Tests.EventStore
{
    [TestFixture]
    public sealed class MessageTopicsTest
    {
        [Test]
        public void Should_traverse_types_tree()
        {
            var topics = new TypeTopics();

            var baseTopics = topics.GetForType(typeof (BaseMessage));
            Assert.AreEqual(2, baseTopics.Count);

            baseTopics = topics.GetForType(typeof (ConcreteMessage));
            Assert.AreEqual(3, baseTopics.Count);

            baseTopics = topics.GetForType(typeof (MessageWithInterface));
            Assert.AreEqual(4, baseTopics.Count);

        }

        [Test]
        public void Should_get_topics_for_command()
        {
            var topics = new TypeTopics();

            var ts = topics.GetForType(typeof (BaseCommand));
            Assert.AreEqual(3, ts.Count);

            ts = topics.GetForType(typeof (ConcreteCommand));
            Assert.AreEqual(4, ts.Count);

        }

        public interface ISomeInterface {}
        public class BaseMessage : IMessage {}
        public class ConcreteMessage : BaseMessage {}
        public class MessageWithInterface : BaseMessage, ISomeInterface {}

        public abstract class BaseCommand : ICommand {}
        public class ConcreteCommand : BaseCommand {}

    }
}
