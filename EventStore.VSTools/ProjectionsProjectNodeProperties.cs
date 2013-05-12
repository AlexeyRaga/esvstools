using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project;

namespace EventStore.VSTools
{
    [CLSCompliant(false), ComVisible(true)]
    [Guid(GuidList.guidProjectionFileProperties)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public sealed class ProjectionsProjectNodeProperties : ProjectNodeProperties
    {
        public ProjectionsProjectNodeProperties(ProjectNode node) : base(node)
        {
        }

        [Browsable(true)]
        [Category("Alexey")]
        [DisplayName("Some property")]
        public string SomeProperty { get; set; }
    }
}
