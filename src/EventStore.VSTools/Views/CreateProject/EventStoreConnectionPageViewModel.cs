using System;
using EventStore.VSTools.EventStore;

namespace EventStore.VSTools.Views.CreateProject
{
    public sealed class EventStoreConnectionPageViewModel : PageViewModel
    {
        private readonly WizardState _state;
        private readonly IProjectionsManagerFactory _projectionsManagerFactory;

        public System.Windows.Input.ICommand TestConnectionCommand { get; private set; }

        public EventStoreConnectionPageViewModel(WizardState state, IProjectionsManagerFactory projectionsManagerFactory)
        {
            _state = state;
            _projectionsManagerFactory = projectionsManagerFactory;
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
            var projectionsManager = _projectionsManagerFactory.BuildProjectionsManager(_state.EventStoreConnection);

            try
            {
                await projectionsManager.TestConnectionAsync();
                CanGoNext = true;
            }
            catch (EventStoreConnectionException ex)
            {
                Output.Pane.OutputStringThreadSafe(String.Format("ERROR: " + ex.Message));
            }

        }
    }
}
