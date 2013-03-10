using System.IO;
using EventStore.ClientAPI;
using EventStore.VS.Tools.EventStoreServices;
using Microsoft.VisualStudio;
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

            ClearOutput();

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                ShowErrorDialog("EventStore connection is not specified");
                return;
            }

            var endpoint = EventStoreConnectionFactory.GetEventStoreEndPoint(connectionString);
            var projectionManager = new ProjectionsManager(new EventStoreLogger(), endpoint);

            var query = File.ReadAllText(fileNode.Url);

            WriteOutput("Deploying {0}...", fileNode.FileName);

            try
            {
                projectionManager.CreateContinuous(Path.GetFileNameWithoutExtension(fileNode.FileName), query);
            }
            catch (Exception ex)
            {
                WriteOutputLine("\tFAILED!");
                WriteOutputLine("\t{0}", ex);
                throw;
            }
            //projectionManager.CreateOneTime(query);


            WriteOutputLine("\tSuccessful.");
        }
    }
}
