using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Project;

namespace EventStore.VS.Tools
{
    public sealed class ProjectionFileNode : FileNode
    {
        public ProjectionFileNode(ProjectNode root, ProjectElement element) : base(root, element)
        {
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
