namespace EventStore.VSTools.CredentialsManager
{
    public interface ICredentialsManager
    {
        void Put(string resource, Credentials credentials);
        Credentials Get(string resource);
    }
}
