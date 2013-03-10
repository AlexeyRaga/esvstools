using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.VS.Tools.EventStoreServices;
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
            ExecuteAsync(node).Wait();
        }

        public async Task ExecuteAsync(HierarchyNode node)
        {
            var projectNode = (ProjectionsProjectNode) node;
            var projectionNodes = new List<ProjectionFileNode>();
            projectNode.FindNodesOfType(projectionNodes);

            var connectionString = node.ProjectMgr.CurrentConfig.GetPropertyValue("ESConnectionString");
            var endpoint = EventStoreConnectionFactory.GetEventStoreEndPoint(connectionString);
            var projectionManager = new ProjectionsManagerLight(endpoint);

            var existing = projectionManager.GetAllNonSystem();

            var toUpdate = projectionNodes
                .Where(x => existing.Contains(x.Name))
                .ToList();

            var toCreate = projectionNodes.Except(toUpdate);

            DeployProjections("Create", toCreate, fileNode =>
                {
                    var props = new ProjectionFileNodeProperties(fileNode);
                    var query = File.ReadAllText(fileNode.Url);
                    return projectionManager.CreateContinuous(fileNode.Name, query, props.EmitEnabled,
                                                                   props.CheckpointEnabled, props.Enabled);
                });

            DeployProjections("Update", toUpdate, fileNode =>
                {
                    var query = File.ReadAllText(fileNode.Url);
                    return projectionManager.Update(fileNode.Name, query);
                });
        }

        private void DeployProjections(string action, IEnumerable<ProjectionFileNode> nodes,
                                             Func<ProjectionFileNode, string> realDeploy)
        {
            foreach (var fileNode in nodes)
            {
                WriteOutput("[{0}] Deploying {1}...", action, fileNode.FileName);

                try
                {
                    realDeploy(fileNode);
                }
                catch (Exception ex)
                {
                    WriteOutputLine("\tFAILED!");
                    WriteOutputLine("\t{0}", ex);
                    throw;
                }

                WriteOutputLine("\tSuccessful.");
            }
        } 
    }
}
