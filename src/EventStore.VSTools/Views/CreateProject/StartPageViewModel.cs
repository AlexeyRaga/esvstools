namespace EventStore.VSTools.Views.CreateProject
{
    public sealed class StartPageViewModel : PageViewModel
    {
        private readonly WizardState _state;

        public StartPageViewModel(WizardState state)
        {
            _state = state;
        }

        public override string Title { get { return Resources.Wizard_StartPageTitle; } }

        public string WelcomeNote { get { return Resources.Wizard_StartPageContent; } }
    }
}
