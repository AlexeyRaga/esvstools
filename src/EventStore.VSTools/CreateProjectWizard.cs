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
            
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
            
        }

        public void RunFinished()
        {
            
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            var viewModel = new CreateProjectViewModel();
            var view = new CreateProjectWizardView(viewModel);
            view.ShowDialog();

        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return false;
        }
    }
}
