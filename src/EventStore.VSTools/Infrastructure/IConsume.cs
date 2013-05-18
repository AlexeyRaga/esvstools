namespace EventStore.VSTools.Infrastructure
{
    public interface IConsume<in T> 
    {
        void Consume(T message);
    }
}
