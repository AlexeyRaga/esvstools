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
    public sealed class DeployCommand : CommandBase
    {
        public override uint CmdId { get { return PkgCmdIDList.cmdidDeployToEventStore; } }

        public DeployCommand(EventStorePackage package) : base(package)
        {
        }

        public override void Execute(HierarchyNode node)
        {
            var projectNode = (ProjectionsProjectNode) node;

            var projectionNodes = new List<ProjectionFileNode>();
            projectNode.FindNodesOfType(projectionNodes);

            ShowNames(projectionNodes);
        }

        private void ShowNames(IEnumerable<ProjectionFileNode> files)
        {
            var names = String.Join(", ", files.Select(x => x.FileName));
            WriteOutput(names);
        }
    }
}
