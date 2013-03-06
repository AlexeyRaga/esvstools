using System;
using System.ComponentModel;
using System.IO;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio;
using VsCommands = Microsoft.VisualStudio.VSConstants.VSStd97CmdID;
using VsCommands2K = Microsoft.VisualStudio.VSConstants.VSStd2KCmdID;
using System.Diagnostics;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Interop;
using System.Globalization;
using EventStore.VS.Tools.PropertyPages;

namespace EventStore.VS.Tools
{
    public class ProjectionsProjectNode : ProjectNode
    {
        private readonly EventStorePackage _package;

        public ProjectionsProjectNode(EventStorePackage package)
        {
            _package = package;
            SupportsProjectDesigner = true;
            InitializeCATIDs();
        }

        private void InitializeCATIDs()
        {
            AddCATIDMapping(typeof(GeneralPropertyPage), typeof(GeneralPropertyPage).GUID);
            AddCATIDMapping(typeof(DeployPropertyPage), typeof(DeployPropertyPage).GUID);

            AddCATIDMapping(typeof(ProjectionFileNodeProperties), typeof(ProjectionFileNodeProperties).GUID);
            AddCATIDMapping(typeof(FileNodeProperties), typeof(ProjectionFileNodeProperties).GUID);

            AddCATIDMapping(typeof(ProjectNodeProperties), typeof(ProjectionsProjectNodeProperties).GUID);
            AddCATIDMapping(typeof(ProjectionsProjectNodeProperties), typeof(ProjectionsProjectNodeProperties).GUID);
        }

        public override Guid ProjectGuid
        {
            get { return GuidList.guidEventStore_VS_ProjectionsProject; }
        }

        public override string ProjectType
        {
            get { return GetType().Name; }
        }

        public override void AddFileFromTemplate(
            string source, string target)
        {
            FileTemplateProcessor.UntokenFile(source, target);
            FileTemplateProcessor.Reset();
        }

        protected override NodeProperties CreatePropertiesObject()
        {
            var properties = new ProjectionsProjectNodeProperties(this);
            //properties.OnCustomToolChanged += new EventHandler<HierarchyNodeEventArgs>(OnCustomToolChanged);
            //properties.OnCustomToolNameSpaceChanged += new EventHandler<HierarchyNodeEventArgs>(OnCustomToolNameSpaceChanged);
            return properties;
        }

        public override FileNode CreateFileNode(ProjectElement item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var newNode = new ProjectionFileNode(this, item);

            return newNode;
        }


        protected override Guid[] GetConfigurationIndependentPropertyPages()
        {
            return new[] {typeof (GeneralPropertyPage).GUID, typeof (DeployPropertyPage).GUID};
        }

        protected override Guid[] GetConfigurationDependentPropertyPages()
        {
            return new[] {typeof (GeneralPropertyPage).GUID, typeof (DeployPropertyPage).GUID};
        }

        protected override Guid[] GetPriorityProjectDesignerPages()
        {
            return new[] { typeof(GeneralPropertyPage).GUID, typeof(DeployPropertyPage).GUID };
        }

        //protected override int ExecCommandOnNode(Guid cmdGroup, uint cmd, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        //{
        //    if (cmdGroup == GuidList.ProjectionsCmdSet)
        //    {
        //        if (cmd == (uint)ProjectMenus.DeployToEventStore.ID)
        //        {
        //            Debug.WriteLine("Deploying!");
        //        }
        //    }
        //    return base.ExecCommandOnNode(cmdGroup, cmd, nCmdexecopt, pvaIn, pvaOut);
        //}

        //protected override int QueryStatusOnNode(Guid cmdGroup, uint cmd, IntPtr pCmdText, ref QueryStatusResult result)
        //{
        //    if (cmdGroup == GuidList.ProjectionsCmdSet)
        //    {
        //        if (cmd == (uint)ProjectMenus.DeployToEventStore.ID)
        //        {
        //            result |= QueryStatusResult.SUPPORTED | QueryStatusResult.ENABLED;
        //            return VSConstants.S_OK;
        //        }
        //    }
        //    return base.QueryStatusOnNode(cmdGroup, cmd, pCmdText, ref result);
        //}

        protected override ReferenceContainerNode CreateReferenceContainerNode()
        {
            return null;
        }

        protected override void WalkSourceProjectAndAdd(IVsHierarchy sourceHierarchy, uint itemId, HierarchyNode targetNode, bool addSiblings)
        {
            if (targetNode == null || targetNode is ReferenceContainerNode) return;
            base.WalkSourceProjectAndAdd(sourceHierarchy, itemId, targetNode, addSiblings);
        }

        protected override bool DisableCmdInCurrentMode(Guid commandGroup, uint command)
        {
            if (commandGroup == VsMenus.guidStandardCommandSet97 || commandGroup == VsMenus.guidStandardCommandSet2K)
            {
                if (commandGroup == VsMenus.guidStandardCommandSet97)
                {
                    switch ((VsCommands)command)
                    {
                        default:
                            break;
                        case VsCommands.Start:
                        case VsCommands.StartNoDebug:
                        case VsCommands.StepInto:
                        case VsCommands.Restart:
                        case VsCommands.Resume:
                        case VsCommands.SetStartupProject:
                            return true;
                    }
                }
                else if (commandGroup == VsMenus.guidStandardCommandSet2K)
                {
                    switch ((VsCommands2K)command)
                    {
                        default:
                            break;
                        case VsCommands2K.PROJSTARTDEBUG:
                        case VsCommands2K.PROJSTEPINTO:
                        case VsCommands2K.ADDWEBREFERENCECTX:
                        case VsCommands2K.ADDWEBREFERENCE:
                        case VsCommands2K.ADDREFERENCE:
                        case VsCommands2K.SETASSTARTPAGE:
                            return true;
                    }
                }
            }
            return base.DisableCmdInCurrentMode(commandGroup, command);
        }
    }
}
