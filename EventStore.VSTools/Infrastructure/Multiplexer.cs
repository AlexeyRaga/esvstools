using System;
using System.Collections.Generic;

namespace EventStore.VSTools.Infrastructure
{
    public sealed class Multiplexer<T> : IConsume<T>
    {
        private readonly List<IConsume<T>> _consumers = new List<IConsume<T>>();

        public Multiplexer() { }

        public Multiplexer(params IConsume<T>[] consumers)
        {
            if (consumers == null) throw new ArgumentNullException("consumers");
            _consumers.AddRange(consumers);
        }

        public void Attach(IConsume<T> consumer)
        {
            _consumers.Add(consumer);
        }

        public void Consume(T message)
        {
            _consumers.ForEach(x => x.Consume(message));
        }
    }

}
