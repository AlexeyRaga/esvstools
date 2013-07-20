using System;
using EventStore.VSTools.EventStore;
using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools.Views.CreateProject
{
    public sealed class EventStoreConnectionPageViewModel : PageViewModel
    {
        private readonly WizardState _state;
        private readonly Func<string, IProjectionsManager> BuildProjectionsManager;

        public System.Windows.Input.ICommand TestConnectionCommand { get; private set; }

        public EventStoreConnectionPageViewModel(WizardState state, Func<string, IProjectionsManager> projectionsManagerFactory)
        {
            _state = state;
            BuildProjectionsManager = projectionsManagerFactory;
            CanGoNext = false;

           TestConnectionCommand  = new DelegateCommand(_ => TestConnection());
        }

        public string ConnectionString
        {
            get { return _state.EventStoreConnection; }
            set { _state.EventStoreConnection = value; }
        }

        public string Username
        {
            get { return _state.Username; }
            set { _state.Username = value; }
        }

        public string Password
        {
            get { return _state.Password; }
            set { _state.Password = value; }
        }

        public override string Title
        {
            get { return Resources.Wizard_EventStoreConnection; }
        }

        public void TestConnection()
        {
            RunConnectionTestAsync();
        }

        private async void RunConnectionTestAsync()
        {
            var projectionsManager = BuildProjectionsManager(_state.EventStoreConnection);

            try
            {
                var tryCredentials = new VSTools.Credentials(_state.Username, _state.Password);

                await projectionsManager.TestConnectionAsync(tryCredentials);
                CanGoNext = true;
            }
            catch (UnauthorisedRequestException ex)
            {
                Output.Pane.OutputStringThreadSafe(String.Format("ERROR: " + ex.Message));
            }
            catch (EventStoreConnectionException ex)
            {
                Output.Pane.OutputStringThreadSafe(String.Format("ERROR: " + ex.Message));
            }

        }
    }
}
