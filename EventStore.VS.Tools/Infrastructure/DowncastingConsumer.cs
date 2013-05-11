namespace EventStore.VS.Tools.Infrastructure
{
    public class DowncastingConsumer<TBase, TExpected> : IConsume<TBase> where TExpected : TBase
    {
        private readonly IConsume<TExpected> _innerConsumer;

        public DowncastingConsumer(IConsume<TExpected> innerConsumer)
        {
            _innerConsumer = innerConsumer;
        }

        public void Consume(TBase instance)
        {
            if (instance is TExpected)
                _innerConsumer.Consume((TExpected)instance);
        }
    }
}
