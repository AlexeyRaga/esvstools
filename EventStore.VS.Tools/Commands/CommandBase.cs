using System;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace EventStore.VS.Tools.Commands
{
    public abstract class CommandBase : IVsCommand
    {
        private readonly EventStorePackage _package;
        protected IVsUIShell Shell { get; private set; }

        public abstract uint CmdId { get; }
        public abstract void Execute(HierarchyNode node);

        protected CommandBase(EventStorePackage package)
        {
            if (package == null) throw new ArgumentNullException("package");
            _package = package;

            Shell = (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));
        }

        protected void ClearOutput()
        {
            Output.Pane.Clear();
        }

        protected void WriteOutput(string message, params object[] parameters)
        {
            Output.Pane.OutputStringThreadSafe(String.Format(message, parameters));
            Output.Pane.Activate();
        }

        protected void WriteOutputLine(string message, params object[] parameters)
        {
            message = message + Environment.NewLine;
            WriteOutput(message, parameters);
        }

        protected void ShowErrorDialog(string message)
        {
            var clsid = Guid.Empty;
            int result;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(Shell.ShowMessageBox(
                       0,
                       ref clsid,
                       "Error",
                       message,
                       string.Empty,
                       0,
                       OLEMSGBUTTON.OLEMSGBUTTON_OK,
                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                       OLEMSGICON.OLEMSGICON_CRITICAL,
                       0,        // false
                       out result));
        }
    }
}
