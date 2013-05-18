namespace EventStore.VSTools.Infrastructure
{
    public interface IPublish<in T>
    {
        void Publish(T message);
    }
}
