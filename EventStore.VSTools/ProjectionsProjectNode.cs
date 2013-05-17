using System;
using EventStore.VSTools.PropertyPages;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio;
using VsCommands = Microsoft.VisualStudio.VSConstants.VSStd97CmdID;
using VsCommands2K = Microsoft.VisualStudio.VSConstants.VSStd2KCmdID;
using Microsoft.VisualStudio.Shell.Interop;

namespace EventStore.VSTools
{
    public class ProjectionsProjectNode : ProjectNode
    {
        private readonly EventStorePackage _package;

        public int ImageListOffset { get; private set; }

        public ProjectionsProjectNode(EventStorePackage package)
        {
            _package = package;
            //SupportsProjectDesigner = true;
            InitializeCATIDs();

            ImageListOffset = ImageHandler.ImageList.Images.Count - 1;

            ImageHandler.AddImage(Resources.projectNodeIcon.ToBitmap());
            ImageHandler.AddImage(Resources.script_16xLG);
        }

        private void InitializeCATIDs()
        {
            //AddCATIDMapping(typeof(GeneralPropertyPage), typeof(GeneralPropertyPage).GUID);
            //AddCATIDMapping(typeof(DeployPropertyPage), typeof(DeployPropertyPage).GUID);

            //AddCATIDMapping(typeof(ProjectionFileNodeProperties), typeof(ProjectionFileNodeProperties).GUID);
            //AddCATIDMapping(typeof(FileNodeProperties), typeof(ProjectionFileNodeProperties).GUID);

            //AddCATIDMapping(typeof(ProjectNodeProperties), typeof(ProjectionsProjectNodeProperties).GUID);
            //AddCATIDMapping(typeof(ProjectionsProjectNodeProperties), typeof(ProjectionsProjectNodeProperties).GUID);
        }

        protected override bool CanShowDefaultIcon()
        {
            return true;
        }

        public override int ImageIndex
        {
            get
            {
                return ImageListOffset + 1;
            }
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
            return new[] {typeof (GeneralPropertyPage).GUID};
        }

        protected override Guid[] GetPriorityProjectDesignerPages()
        {
            Guid[] result = new Guid[1];
            result[0] = typeof(GeneralPropertyPage).GUID;
            return result;
        }

        //protected override Guid[] GetConfigurationIndependentPropertyPages()
        //{
        //    return new[] {typeof (GeneralPropertyPage).GUID, typeof (DeployPropertyPage).GUID};
        //}

        protected override Guid[] GetConfigurationDependentPropertyPages()
        {
            return new[] { typeof(DeployPropertyPage).GUID };
        }

        //protected override Guid[] GetPriorityProjectDesignerPages()
        //{
        //    return new[] { typeof(GeneralPropertyPage).GUID, typeof(DeployPropertyPage).GUID };
        //}

        protected override int ExecCommandOnNode(Guid cmdGroup, uint cmd, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (cmdGroup == GuidList.guidEventStore_VS_ToolsCmdSet)
            {
                var command = _package.FindCommand(cmd);
                if (command != null) command.Execute(this);
                return VSConstants.S_OK;
            }
            return base.ExecCommandOnNode(cmdGroup, cmd, nCmdexecopt, pvaIn, pvaOut);
        }

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
