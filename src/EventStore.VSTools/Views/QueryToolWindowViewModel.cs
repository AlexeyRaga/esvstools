using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EventStore.VSTools.Views
{
    public sealed class QueryToolWindowViewModel
    {
        public ObservableCollection<QueryViewModel> Queries { get; private set; }

        public ICommand CloseTabCommand { get; private set; }

        public QueryToolWindowViewModel()
        {
            Queries = new ObservableCollection<QueryViewModel>();
            CloseTabCommand = new DelegateCommand(x => CloseTab((QueryViewModel)x));
        }

        private void CloseTab(QueryViewModel tabModel)
        {
            tabModel.Close();
            Queries.Remove(tabModel);
        }

        public void OnWindowClosed()
        {
            foreach (var model in Queries)
                model.Stop();

        }

        public void OnWindowOpened()
        {

        }

        internal void ShowQueryResult(string name, string queryUri, string queryResult)
        {
            var viewModel = new QueryViewModel(name, queryUri, queryResult);
            Queries.Add(viewModel);
            viewModel.IsSelected = true;
            viewModel.Start();
        }
    }
}
