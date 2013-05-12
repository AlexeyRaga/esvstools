using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace EventStore.VS.Tools.Infrastructure
{
    public sealed class TopicBaseDispatcher<TBase> : IPublish<TBase> where TBase : IMessage
    {
        private readonly TypeTopics _typeTopics = new TypeTopics();
        private readonly ConcurrentDictionary<string, Multiplexer<TBase>> _subscribers
            = new ConcurrentDictionary<string, Multiplexer<TBase>>(); 

        public void Publish(TBase message)
        {
            var topics = _typeTopics.GetForType(message.GetType());
            foreach (var topic in topics)
            {
                PublishToTopic(topic, message);
            }
        }

        private void PublishToTopic(string topic, TBase message)
        {
            Multiplexer<TBase> multiplexer;
            if (_subscribers.TryGetValue(topic, out multiplexer))
                multiplexer.Consume(message);
        }

        public void Subscribe<T>(IConsume<T> consumer) where T : TBase
        {
            var downcaster = new DowncastingConsumer<TBase, T>(consumer);
            _subscribers.AddOrUpdate(typeof (T).FullName,
                                     _ => new Multiplexer<TBase>(downcaster),
                                     (_, m) => { m.Attach(downcaster); return m; });
        }
    }

    public sealed class TypeTopics
    {
        private readonly ConcurrentDictionary<Type, string[]> _topics = new ConcurrentDictionary<Type, string[]>();
 
        public IList<string> GetForType(Type type)
        {
            var topics = _topics.GetOrAdd(type, t => GetMessageTopics(t).ToArray());
            return topics.ToList();
        }

        private bool IgnoreType(Type type)
        {
            return !type.FullName.StartsWith("EventStore.Vs.Tools", StringComparison.OrdinalIgnoreCase);
        }

        private IEnumerable<string> GetMessageTopics(Type messageType)
        {
            var typeAssembly = messageType.Assembly;

            var currentType = messageType;
            while (currentType != typeof (object))
            {
                if (!IgnoreType(currentType))
                    yield return currentType.FullName;

                currentType = currentType.BaseType;
            }

            var interfaces = messageType.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!IgnoreType(@interface))
                    yield return @interface.FullName;
            }
        }
    }
}
