namespace EventStore.VSTools.Infrastructure
{
    public interface IProvideCredentials
    {
        Credentials GetFor(string resource, bool forceAskUser);
    }
}
