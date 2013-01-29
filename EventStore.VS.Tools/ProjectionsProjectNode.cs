using System;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio;
using VsCommands = Microsoft.VisualStudio.VSConstants.VSStd97CmdID;
using VsCommands2K = Microsoft.VisualStudio.VSConstants.VSStd2KCmdID;
using System.Diagnostics;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Interop;
using System.Globalization;

namespace EventStore.EventStore_VS_Tools
{
    public class ProjectionsProjectNode : ProjectNode
    {
        private readonly EventStore_VS_ToolsPackage _package;

        public ProjectionsProjectNode(EventStore_VS_ToolsPackage package)
        {
            _package = package;
        }

        public override Guid ProjectGuid
        {
            get { return GuidList.guidEventStore_VS_ProjectionsProject; }
        }

        public override string ProjectType
        {
            get { return this.GetType().Name; }
        }

        public override void AddFileFromTemplate(
            string source, string target)
        {
            FileTemplateProcessor.UntokenFile(source, target);
            FileTemplateProcessor.Reset();
        }

        //protected override Guid[] GetConfigurationIndependentPropertyPages()
        //{
        //    var result = new Guid[1];
        //    result[0] = typeof(GeneralPropertyPage).GUID;
        //    return result;
        //}
        //protected override Guid[] GetPriorityProjectDesignerPages()
        //{
        //    var result = new Guid[1];
        //    result[0] = typeof(GeneralPropertyPage).GUID;
        //    return result;
        //}



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
