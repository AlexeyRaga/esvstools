namespace EventStore.VSTools.Infrastructure
{
    public interface IMessage
    {
    }

    public interface ICommand : IMessage {}
    public interface IEvent : IMessage{}
}
