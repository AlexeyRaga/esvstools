namespace EventStore.VSTools.Infrastructure
{
    public interface IOutputErrorMessages
    {
        void WriteError(string message, params object[] parameters);
    }
}
