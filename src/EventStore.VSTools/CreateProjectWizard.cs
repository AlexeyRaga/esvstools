using System.Collections.Generic;
using EventStore.VSTools.Views.CreateProject;
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
            var viewModel = new CreateProjectViewModel();
            var view = new CreateProjectWizardView(viewModel);
            if (view.ShowDialog().GetValueOrDefault(false))
            {
                var projectNode = project.Object as ProjectionsProjectNode;
                if (projectNode == null)
                {
                    Output.Pane.OutputStringThreadSafe(string.Format(
                        "Wrong project node. Actual type is: {0}",
                        project.Object == null ? "<null>" : project.Object.GetType().FullName));

                    return;
                }

                projectNode.SetProjectProperty(Constants.EventStore.ConnectionString, viewModel.State.EventStoreConnection);
                project.Save();
            }
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
