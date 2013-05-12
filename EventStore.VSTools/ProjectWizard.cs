using Microsoft.VisualStudio.TemplateWizard;
using System.Collections.Generic;

namespace EventStore.VSTools
{
    public sealed class ProjectWizard : IWizard
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
            
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
