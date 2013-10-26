using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools.Tests
{
    public sealed class FakeCredentialsService : IProvideCredentials
    {
        private VSTools.Credentials _credentialsToReturn;

        public FakeCredentialsService(VSTools.Credentials credentialsToReturn)
        {
            _credentialsToReturn = credentialsToReturn;
        }

        public FakeCredentialsService()
            : this(new VSTools.Credentials("someUser", "somePassword")) { }

        public void SetCredentials(VSTools.Credentials credentials)
        {
            _credentialsToReturn = credentials;
        }

        public VSTools.Credentials GetFor(string resource, bool forceAskUser)
        {
            return _credentialsToReturn;
        }
    }
}
