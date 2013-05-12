using System.IO;
using EventStore.VSTools.EventStore;
using EventStore.VSTools.Infrastructure;
using Microsoft.VisualStudio.Project;

namespace EventStore.VSTools.Commands
{
    public sealed class RunCommand : CommandBase
    {
        public RunCommand(IPublish<IMessage> publisher) : base(publisher)
        {
        }

        public override uint CmdId { get { return PkgCmdIDList.cmdidRunProjection; } }


        public override void Execute(HierarchyNode node)
        {
            var projectionNode = (ProjectionFileNode) node;

            var eventStoreAddress = EventStoreAddress.Get(node.ProjectMgr);

            var command = new RunProjection(eventStoreAddress,
                                            GetProjectionName(projectionNode),
                                            GetProjectionContent(projectionNode));

            Publisher.Publish(command);
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
