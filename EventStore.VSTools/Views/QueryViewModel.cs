using System;
using System.ComponentModel;
using System.Windows.Threading;
using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools.Views
{
    public sealed class QueryViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; private set; }
        public string QueryUri { get; private set; }

        private readonly DispatcherTimer _timer;

        private string _queryResult;
        private bool _isSelected;

        public string QueryResult
        {
            get { return _queryResult; }
            private set
            {
                if (_queryResult == value) return;
                _queryResult = value;
                OnPropertyChanged("QueryResult");
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged("IsSelected");

                if (_isSelected)
                    StartPeriodicUpdates();
                else
                    StopPeriodicUpdates();
            }
        }

        public QueryViewModel(string name, string queryUri, string queryResult)
        {
            Name = name;
            QueryUri = queryUri;
            QueryResult = queryResult;

            _timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
            _timer.Tick += (sender, args) => UpdateQuery();
        }

        public void Close()
        {
            _timer.Stop();
        }

        private async void UpdateQuery()
        {
            var client = new SimpleHttpClient();
            var result = await client.GetAsync(QueryUri + "/state");

            Dispatcher.CurrentDispatcher.Invoke(() => QueryResult = result.Content);
        }

        public void StopPeriodicUpdates()
        {
            _timer.Stop();
        }

        public void StartPeriodicUpdates()
        {
            _timer.Start();
        }

        private void OnPropertyChanged(string propertyName)
        {
            var evt = PropertyChanged;
            if (evt != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            Close();
        }
    }
}
