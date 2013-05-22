using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using EventStore.VSTools.Infrastructure;
using ICommand = System.Windows.Input.ICommand;

namespace EventStore.VSTools.Views.CreateProject
{
    public sealed class CreateProjectViewModel : INotifyPropertyChanged
    {
        public WizardState State { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IList<PageViewModel> _pages = new List<PageViewModel>();
        private PageViewModel _activePage;
        private readonly List<DelegateCommand> _knownCommands = new List<DelegateCommand>();

        public string Title { get { return Resources.ProjectionsProjectTitle; } }

        public ICommand NextCommand { get; private set; }
        public ICommand PrevCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand FinishCommand { get; private set; }

        public PageViewModel ActivePage
        {
            get { return _activePage; }
            set
            {
                if (_activePage == value) return;
                if (_activePage != null) _activePage.Deactivate();
                _activePage = value;
                if (_activePage != null) _activePage.Activate();

                _knownCommands.ForEach(x => x.UpdateStatus());
                OnPropertyChanged("ActivePage");
            }
        }

        public CreateProjectViewModel()
        {
            State = new WizardState {EventStoreConnection = "localhost:2113"};
            _pages.Add(new StartPageViewModel(State));
            _pages.Add(new EventStoreConnectionPageViewModel(State));
            _pages.Add(new ImportProjectionsPageViewModel(State, new SimpleHttpClient()));

            NextCommand     = BuildCommand(_ => ActivePage != _pages.Last(), _ => OnNext());
            PrevCommand     = BuildCommand(_ => ActivePage != _pages.First(), _ => OnPrev());
            FinishCommand   = BuildCommand(_ => ActivePage == _pages.Last(), _ => OnFinish());
            CancelCommand   = BuildCommand(_ => true, _ => OnCancel());

            ActivePage = _pages.First();
        }

        private DelegateCommand BuildCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            var command = new DelegateCommand(canExecute, execute);
            _knownCommands.Add(command);
            return command;
        }

        private void OnCancel()
        {
            
        }

        private void OnFinish()
        {
            
        }

        private void OnPrev()
        {
            var prevPage = _pages.Previous(ActivePage);
            ActivePage = prevPage;
        }

        private void OnNext()
        {
            var nextPage = _pages.Next(ActivePage);
            ActivePage = nextPage;
        }

        private void OnPropertyChanged(string propertyName)
        {
            var evt = PropertyChanged;
            if (evt != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
