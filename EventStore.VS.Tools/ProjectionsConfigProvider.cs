using Microsoft.VisualStudio.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.VS.Tools
{
    public sealed class ProjectionsConfigProvider : ConfigProvider
    {
        private ProjectionsProjectNode _project;

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
