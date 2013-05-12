namespace EventStore.VS.Tools.Infrastructure
{
    public interface IConsume<in T> 
    {
        void Consume(T message);
    }
}
