using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Project;

namespace EventStore.VS.Tools
{
    public sealed class ProjectionFileNode : FileNode
    {
        private readonly ProjectionsProjectNode _project;

        public ProjectionFileNode(ProjectionsProjectNode project, ProjectElement element) : base(project, element)
        {
            _project = project;
        }

        protected override int ExecCommandOnNode(Guid cmdGroup, uint cmd, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (cmdGroup == GuidList.guidEventStore_VS_ToolsCmdSet)
            {
                var esPackage = (EventStorePackage) _project.Package;
                var command = esPackage.FindCommand(cmd);
                if (command != null) command.Execute(this);
                return VSConstants.S_OK;
            }
            return base.ExecCommandOnNode(cmdGroup, cmd, nCmdexecopt, pvaIn, pvaOut);
        }

        protected override NodeProperties CreatePropertiesObject()
        {
            var properties = new ProjectionFileNodeProperties(this);
            //properties.OnCustomToolChanged += new EventHandler<HierarchyNodeEventArgs>(OnCustomToolChanged);
            //properties.OnCustomToolNameSpaceChanged += new EventHandler<HierarchyNodeEventArgs>(OnCustomToolNameSpaceChanged);
            return properties;
        }

    }
}
