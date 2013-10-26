using System;
using EventStore.VSTools.CredentialsManager;
using EventStore.VSTools.Infrastructure;
using EventStore.VSTools.Views.Credentials;

namespace EventStore.VSTools.EventStore
{
    internal static class ConfigurationThatReplacesIoC
    {
        public static readonly Func<string, IProjectionsManager> BuildProjectionsManager =
            address => new ProjectionsManager(address, new CredentialPromptService(BuildCredentialsManager()), BuildHttpClient());


        public static ICredentialsManager BuildCredentialsManager()
        {
            return new CredentialsManager.CredentialsManager(new CredentialsEncryptor(), new RegistryCredentialsStore(Constants.Product.Name));
        }

        public static IHttpClient BuildHttpClient()
        {
            return new SimpleHttpClient();
        }
    }
}
