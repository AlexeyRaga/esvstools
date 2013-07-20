using System;
using EventStore.VSTools.CredentialsManager;
using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools.Views.Credentials
{
    public sealed class CredentialPromptService : IProvideCredentials
    {
        private readonly ICredentialsManager _credentialsManager;

        public CredentialPromptService(ICredentialsManager credentialsManager)
        {
            _credentialsManager = credentialsManager;
        }

        public VSTools.Credentials GetFor(string resource, bool forceAskUser)
        {
            var credentials = _credentialsManager.Get(resource);
            if (credentials.IsEmpty()) 
                return AskForCredentials(resource, String.Empty, String.Empty);

            if (forceAskUser)
                return AskForCredentials(resource, credentials.Username, credentials.Password);

            return credentials;
        }

        private VSTools.Credentials AskForCredentials(string resourceName, string username, string password)
        {
            var viewModel = new CredentialsPromptViewModel(resourceName, username, password);
            var viewWindow = new CredentialsPrompt();
            viewWindow.AttachModel(viewModel);

            var gotCredentials = viewWindow.ShowDialog().GetValueOrDefault();
            return !gotCredentials ? null : new VSTools.Credentials(viewModel.Username, viewModel.Password);
        }
    }
}
