using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Project;
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

        public void Execute(HierarchyNode node)
        {
            var projectNode = (ProjectionsProjectNode) node;

            var projectionNodes = new List<ProjectionFileNode>();
            projectNode.FindNodesOfType(projectionNodes);

            ShowNames(projectionNodes);
        }

        private void ShowNames(IEnumerable<ProjectionFileNode> files)
        {
            var names = String.Join(", ", files.Select(x => x.FileName));
            var clsid = Guid.Empty;
            int result;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(_shell.ShowMessageBox(
                       0,
                       ref clsid,
                       "EventStoreTools",
                       names,
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
