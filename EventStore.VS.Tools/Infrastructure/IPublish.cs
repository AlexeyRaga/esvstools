namespace EventStore.VS.Tools.Infrastructure
{
    public interface IPublish<in T>
    {
        void Publish(T message);
    }
}
