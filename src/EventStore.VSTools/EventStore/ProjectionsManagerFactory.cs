using EventStore.VSTools.CredentialsManager;
using EventStore.VSTools.Infrastructure;
using EventStore.VSTools.Views.Credentials;

namespace EventStore.VSTools.EventStore
{
    public interface IProjectionsManagerFactory
    {
        IProjectionsManager BuildProjectionsManager(string eventStoreAddress);
    }

    public sealed class ProjectionsManagerFactory : IProjectionsManagerFactory
    {
        public IProjectionsManager BuildProjectionsManager(string eventStoreAddress)
        {
            return new ProjectionsManager(eventStoreAddress,
                                          new CredentialPromptService(
                                              new CredentialsManager.CredentialsManager(new CredentialsEncryptor(),
                                                                                        new RegistryCredentialsStore(
                                                                                            Constants.Product.Name))),
                                          new SimpleHttpClient());

        }
    }
}
