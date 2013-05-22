namespace EventStore.VSTools.Views.CreateProject
{
    public sealed class EventStoreConnectionPageViewModel : PageViewModel
    {
        private readonly WizardState _state;

        public EventStoreConnectionPageViewModel(WizardState state)
        {
            _state = state;
        }

        public string ConnectionString
        {
            get { return _state.EventStoreConnection; }
            set { _state.EventStoreConnection = value; }
        }

        public override string Title
        {
            get { return Resources.Wizard_EventStoreConnection; }
        }
    }
}
