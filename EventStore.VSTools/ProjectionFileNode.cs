using System;
using System.IO;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Project;

namespace EventStore.VSTools
{
    public sealed class ProjectionFileNode : FileNode
    {
        private readonly ProjectionsProjectNode _project;

        public string Name { get { return Path.GetFileNameWithoutExtension(FileName); } }

        public ProjectionFileNode(ProjectionsProjectNode project, ProjectElement element) : base(project, element)
        {
            _project = project;
        }

        public override int ImageIndex
        {
            get { return _project.ImageListOffset + 1; }
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
