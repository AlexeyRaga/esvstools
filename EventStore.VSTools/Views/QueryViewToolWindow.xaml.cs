using System.Windows.Controls;

namespace EventStore.VSTools.Views
{
    /// <summary>
    /// Interaction logic for QueryView.xaml
    /// </summary>
    public partial class QueryViewToolWindow : UserControl
    {
        public QueryViewToolWindow(QueryToolWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            Unloaded += (sender, args) => viewModel.OnWindowClosed();
            Loaded += (sender, args) => viewModel.OnWindowOpened();
        }

    }
}
