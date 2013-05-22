using System.Windows;

namespace EventStore.VSTools.Views.CreateProject
{
    /// <summary>
    /// Interaction logic for CreateProjectWizardView.xaml
    /// </summary>
    public partial class CreateProjectWizardView : Window
    {
        public CreateProjectWizardView(CreateProjectViewModel viewModel)
        {
            InitializeComponent();
            viewModel.OnClose = result =>
                {
                    DialogResult = result;
                    Close();
                };

            DataContext = viewModel;
        }
    }
}
