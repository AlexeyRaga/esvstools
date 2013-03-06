using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;

namespace EventStore.VS.Tools.Commands
{
    public sealed class DeployCommand : IVsCommand
    {
        private readonly IVsUIShell _shell;
        public uint CmdId { get { return PkgCmdIDList.cmdidDeployToEventStore; } }

        public DeployCommand(IVsUIShell shell)
        {
            _shell = shell;
        }

        public void Execute(object sender, EventArgs eventArgs)
        {
            var clsid = Guid.Empty;
            int result;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(_shell.ShowMessageBox(
                       0,
                       ref clsid,
                       "EventStoreTools",
                       string.Empty,
                       0,
                       OLEMSGBUTTON.OLEMSGBUTTON_OK,
                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                       OLEMSGICON.OLEMSGICON_INFO,
                       0,        // false
                       out result));
        }
    }
}
