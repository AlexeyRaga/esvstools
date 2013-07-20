using System;
using System.Windows.Input;

namespace EventStore.VSTools.Views
{
    public sealed class DelegateCommand : ICommand
    {
        private readonly Func<object, bool> _canRun;
        private readonly Action<object> _action;

        public DelegateCommand(Func<object, bool> canRun, Action<object> action)
        {
            _canRun = canRun;
            _action = action;
        }

        public DelegateCommand(Action<object> action)
        {
            _canRun = _ => true;
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return (_canRun == null) || _canRun(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (_action != null) _action(parameter);
        }

        public void UpdateStatus()
        {
            var evt = CanExecuteChanged;
            if (evt != null) evt(this, EventArgs.Empty);
        }
    }
}
