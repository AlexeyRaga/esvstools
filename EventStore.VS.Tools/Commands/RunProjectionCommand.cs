using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.VS.Tools.Commands
{
    public sealed class RunProjectionCommand : IVsCommand
    {
        private readonly IVsUIShell _shell;
        public uint CmdId { get { return PkgCmdIDList.cmdidRunProjection; } }

        public RunProjectionCommand(IVsUIShell shell)
        {
            _shell = shell;
        }

        public void Execute(HierarchyNode node)
        {
            var fileNode = (ProjectionFileNode) node;
            var clsid = Guid.Empty;
            int result;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(_shell.ShowMessageBox(
                       0,
                       ref clsid,
                       "EventStoreTools",
                       string.Format(CultureInfo.CurrentCulture, "Executing {0}", fileNode.FileName),
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
