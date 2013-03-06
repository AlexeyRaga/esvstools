using Microsoft.VisualStudio.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.VS.Tools
{
    public sealed class ProjectionsProjectConfig : ProjectConfig
    {
        public ProjectionsProjectConfig(ProjectionsProjectNode project, string configuration) : base(project, configuration)
        {

        }
    }
}
