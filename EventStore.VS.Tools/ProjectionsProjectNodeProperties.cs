using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Project;

namespace EventStore.VS.Tools
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
