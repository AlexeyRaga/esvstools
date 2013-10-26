using System.Windows;

namespace EventStore.VSTools.Views.Credentials
{
    /// <summary>
    /// Interaction logic for CredentialsPrompt.xaml
    /// </summary>
    public partial class CredentialsPrompt : Window
    {
        public CredentialsPrompt()
        {
            InitializeComponent();
        }

        public void AttachModel(CredentialsPromptViewModel model)
        {
            this.DataContext = model;

            model.OnClose = result =>
            {
                DialogResult = result;
                Close();
            };
        }
    }
}
