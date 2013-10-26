﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using EventStore.VSTools.EventStore;
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

        public Action<bool> OnClose;

        public PageViewModel ActivePage
        {
            get { return _activePage; }
            set
            {
                if (_activePage == value) return;
                if (_activePage != null) _activePage.Deactivate();
                _activePage = value;
                if (_activePage != null) _activePage.Activate();

                UpdateCommandsStatus();
                OnPropertyChanged("ActivePage");
            }
        }

        public CreateProjectViewModel(Func<string, IProjectionsManager> projectionsManagerFactory)
        {
            State = new WizardState {EventStoreConnection = "localhost:2113"};
            AddPage(new StartPageViewModel(State));
            AddPage(new EventStoreConnectionPageViewModel(State, projectionsManagerFactory));
            AddPage(new ImportProjectionsPageViewModel(State, projectionsManagerFactory));
            AddPage(new FinishPageViewModel(State));

            NextCommand     = BuildCommand(_ => ActivePage != _pages.Last() && ActivePage.CanGoNext, _ => OnNext());
            PrevCommand     = BuildCommand(_ => ActivePage != _pages.First() && ActivePage.CanGoBack, _ => OnPrev());
            FinishCommand   = BuildCommand(_ => ActivePage == _pages.Last() && ActivePage.CanGoNext, _ => OnFinish());
            CancelCommand   = BuildCommand(_ => true, _ => OnCancel());

            ActivePage = _pages.First();
        }

        private void AddPage(PageViewModel page)
        {
            page.PageStateChanded += (sender, args) => UpdateCommandsStatus();
            _pages.Add(page);
        }

        private DelegateCommand BuildCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            var command = new DelegateCommand(canExecute, execute);
            _knownCommands.Add(command);
            return command;
        }

        private void UpdateCommandsStatus()
        {
            _knownCommands.ForEach(x => x.UpdateStatus());
        }

        private void OnCancel()
        {
            OnClose(false);
        }

        private void OnFinish()
        {
            OnClose(true);
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
