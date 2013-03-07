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
    public sealed class RunProjectionCommand : CommandBase
    {
        public override uint CmdId { get { return PkgCmdIDList.cmdidRunProjection; } }

        public RunProjectionCommand(EventStorePackage package)
            : base(package)
        {
        }

        public override void Execute(HierarchyNode node)
        {
            var fileNode = (ProjectionFileNode) node;
            var connectionString = node.ProjectMgr.CurrentConfig.GetPropertyValue("ESConnectionString");

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                ShowErrorDialog("EventStore connection is not specified");
                return;
            }

            WriteOutput("Would deploy {0} into {1}", fileNode.FileName, connectionString);
        }

    }
}
