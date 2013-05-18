using Microsoft.VisualStudio.Project;

namespace EventStore.VSTools
{
    public sealed class ProjectionsConfigProvider : ConfigProvider
    {
        private readonly ProjectionsProjectNode _project;

        public ProjectionsConfigProvider(ProjectionsProjectNode project) : base(project)
        {
            this._project = project;
        }

        protected override ProjectConfig CreateProjectConfiguration(string configName)
        {
            var requestedConfiguration = new ProjectionsProjectConfig(_project, configName);
            return requestedConfiguration;
        }

    }
}
