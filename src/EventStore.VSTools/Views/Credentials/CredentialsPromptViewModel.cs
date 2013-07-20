using System;
using System.Windows.Input;

namespace EventStore.VSTools.Views.Credentials
{
    public sealed class CredentialsPromptViewModel
    {
        public string ResourceName { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public ICommand CancelCommand { get; private set; }
        public ICommand OkCommand { get; private set; }

        public Action<bool> OnClose;

        public CredentialsPromptViewModel()
        {
            CancelCommand = new DelegateCommand(_ => OnCancel());
            OkCommand = new DelegateCommand(_ => OnOk());
        }

        public CredentialsPromptViewModel(string resourceName, string username, string password)
            : this()
        {
            ResourceName = resourceName;
            Username = username;
            Password = password;
        }

        public void OnOk()
        {
            OnClose(true);
        }

        public void OnCancel()
        {
            OnClose(false);
        }
    }
}
