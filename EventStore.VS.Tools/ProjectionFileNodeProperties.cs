using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project;
using System;

namespace EventStore.VS.Tools
{
    [CLSCompliant(false), ComVisible(true)]
    [Guid(GuidList.guidProjectionFileProperties)]
    public sealed class ProjectionFileNodeProperties : SingleFileGeneratorNodeProperties
    {
        public ProjectionFileNodeProperties(HierarchyNode node) : base(node)
        {
        }

        [Browsable(true), Category("Event Store")]
        [DisplayName("Enabled")]
        [DefaultValue(true)]
        public bool Enabled
        {
            get
            {
                bool enabled;
                return !bool.TryParse(Node.ItemNode.GetMetadata("Enabled"), out enabled) || enabled;
            } 
            set { Node.ItemNode.SetMetadata("Enabled", value.ToString()); }
        }

        [Browsable(true), Category("Event Store")]
        [DisplayName("Enable Emit")]
        [Description("Enable or disable emiting events from this projection")]
        [DefaultValue(false)]
        public bool EmitEnabled
        {
            get
            {
                bool enabled;
                return bool.TryParse(Node.ItemNode.GetMetadata("EmitEnabled"), out enabled) && enabled;
            }
            set { Node.ItemNode.SetMetadata("EmitEnabled", value.ToString()); }
        }

        [Browsable(true), Category("Event Store")]
        [DisplayName("Enable CheckPoint")]
        [Description("Enable or disable checkpoint for the projection")]
        [DefaultValue(false)]
        public bool CheckpointEnabled
        {
            get
            {
                bool enabled;
                return bool.TryParse(Node.ItemNode.GetMetadata("CheckpointEnabled"), out enabled) && enabled;
            }
            set { Node.ItemNode.SetMetadata("CheckpointEnabled", value.ToString()); }
        }



        [Browsable(false)]
        public override BuildAction BuildAction
        {
            get { return base.BuildAction;}
            set { base.BuildAction = value; }
        }

        [Browsable(false)]
        public override string CustomTool
        {
            get { return base.CustomTool; }
            set { base.CustomTool = value; }
        }

        [Browsable(false)]
        public override string CustomToolNamespace
        {
            get { return base.CustomToolNamespace; }
            set { base.CustomToolNamespace = value; }
        }

    }
}
