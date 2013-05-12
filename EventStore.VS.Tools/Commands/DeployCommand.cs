using System.Collections.Generic;
using System.IO;
using System.Linq;
using EventStore.VS.Tools.EventStore;
using EventStore.VS.Tools.Infrastructure;
using Microsoft.VisualStudio.Project;

namespace EventStore.VS.Tools.Commands
{
    public sealed class DeployCommand : CommandBase
    {
        public override uint CmdId { get { return PkgCmdIDList.cmdidDeployToEventStore; } }

        public DeployCommand(IPublish<IMessage> publisher) 
            : base(publisher) { }

        public override void Execute(HierarchyNode node)
        {
            var projectNode = (ProjectionsProjectNode) node;
            var projectionNodes = new List<ProjectionFileNode>();

            var eventStoreAddress = EventStoreAddress.Get(node.ProjectMgr);

            projectNode.FindNodesOfType(projectionNodes);

            var commands = BuildDeployCommands(eventStoreAddress, projectionNodes);

            foreach (var command in commands)
            {
                Publisher.Publish(command);
            }
        }

        private IEnumerable<DeployProjection> BuildDeployCommands(string eventStoreAddress, IEnumerable<ProjectionFileNode> fileNodes)
        {
            return fileNodes.Select(x => new DeployProjection(eventStoreAddress, GetProjectionName(x), GetProjectionContent(x)));
        } 

        private static string GetProjectionName(FileNode node)
        {
            return Path.GetFileNameWithoutExtension(node.FileName);
        }

        private static string GetProjectionContent(FileNode node)
        {
            return File.ReadAllText(node.Url);
        }
    }
}
