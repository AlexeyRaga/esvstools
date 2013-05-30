namespace EventStore.VSTools.Views.CreateProject
{
    public sealed class FinishPageViewModel : PageViewModel
    {
        public WizardState State { get; set; }
        public override string Title { get { return Resources.Wizard_FinishPageTitle; } }

        public FinishPageViewModel(WizardState state)
        {
            State = state;
        }
    }
}
