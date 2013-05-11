using System;
using System.Collections.Concurrent;

namespace EventStore.VS.Tools.Infrastructure
{
    public sealed class TypeBaseDispatcher<TBase> : IPublish<TBase>
    {
        private readonly ConcurrentDictionary<Type, Multiplexer<TBase>> _subscribers
            = new ConcurrentDictionary<Type, Multiplexer<TBase>>(); 

        public void Publish(TBase message)
        {
            Multiplexer<TBase> multiplexer;
            if (_subscribers.TryGetValue(message.GetType(), out multiplexer))
                multiplexer.Consume(message);
        }

        public void Subscribe<T>(IConsume<T> consumer) where T : TBase
        {
            var downcaster = new DowncastingConsumer<TBase, T>(consumer);
            _subscribers.AddOrUpdate(typeof (T),
                                     _ => new Multiplexer<TBase>(downcaster),
                                     (_, m) => { m.Attach(downcaster); return m; });
        }
    }
}
