using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EventStore.VSTools.CredentialsManager;
using EventStore.VSTools.EventStore;
using EventStore.VSTools.Infrastructure;
using EventStore.VSTools.Views.CreateProject;
using EventStore.VSTools.Views.Credentials;
using Microsoft.VisualStudio.TemplateWizard;

namespace EventStore.VSTools
{
    public sealed class CreateProjectWizard : IWizard
    {
        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
            
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
            var viewModel = new CreateProjectViewModel(ConfigurationThatReplacesIoC.BuildProjectionsManager);

            var view = new CreateProjectWizardView(viewModel);
            if (!view.ShowDialog().GetValueOrDefault(false)) return;

            var projectNode = (ProjectionsProjectNode) project.Object;

            StoreCredentials(viewModel.State.EventStoreConnection, viewModel.State.Username, viewModel.State.Password);

            if (viewModel.State.ProjectionsToImport.Any())
            {
                var projectionsManager = ConfigurationThatReplacesIoC.BuildProjectionsManager(viewModel.State.EventStoreConnection);
                AsyncHelpers.RunSync(() => ImportProjectionsAsync(projectNode, projectionsManager, viewModel.State.ProjectionsToImport));
            }

            project.Save();
        }

        private static void StoreCredentials(string resource, string username, string password)
        {
            var credentials = new Credentials(username, password);
            var credentialsManager = ConfigurationThatReplacesIoC.BuildCredentialsManager();

            credentialsManager.Put(resource, credentials);
        }

        private static async Task ImportProjectionsAsync(ProjectionsProjectNode project, IProjectionsManager projectionsManager, IList<ProjectionStatistics> projections)
        {
            foreach (var projection in projections)
            {
                var configResponse = await projectionsManager.GetConfigAsync(projection.Name);

                if (!configResponse.IsSuccessful)
                {
                    Output.Pane.OutputStringThreadSafe(
                        string.Format("Unable to fetch projection {0}, server returned {1}", projection.Name,
                                      configResponse.Status));
                    continue;
                }


                AddProjectionFileIntoProject(project, configResponse.Result, projection);
            }
        }

        private static void AddProjectionFileIntoProject(ProjectionsProjectNode project, ProjectionConfig config, ProjectionStatistics stats)
        {
            var name = config.Name;
            var query = config.Query;

            //create file and node
            var projectionNode = (ProjectionFileNode)project.CreateFileNode(name + ".js");
            File.WriteAllText(projectionNode.Url, query);

            //set node properties
            var nodeProps = (ProjectionFileNodeProperties) projectionNode.NodeProperties;
            nodeProps.Enabled = stats.IsEnabled;
            nodeProps.EmitEnabled = config.IsEmitEnabled;
            nodeProps.CheckpointEnabled = "continuous".Equals(stats.Mode, StringComparison.InvariantCultureIgnoreCase);

            //finally add the node into the project
            project.AddChild(projectionNode);
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
            
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
           
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
