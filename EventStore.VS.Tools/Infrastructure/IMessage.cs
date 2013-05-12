namespace EventStore.VS.Tools.Infrastructure
{
    public interface IMessage
    {
    }

    public interface ICommand : IMessage {}
    public interface IEvent : IMessage{}
}
