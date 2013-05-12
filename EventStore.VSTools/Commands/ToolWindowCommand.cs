using System;
using EventStore.VSTools.Infrastructure;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell.Interop;

namespace EventStore.VSTools.Commands
{
    public sealed class ToolWindowCommand : IVsCommand
    {
        private readonly EventStorePackage _package;
        private readonly IPublish<IMessage> _publisher;
        public uint CmdId { get { return PkgCmdIDList.cmdidToolSettings; } }

        public ToolWindowCommand(EventStorePackage package, IPublish<IMessage> publisher)
        {
            _package = package;
            _publisher = publisher;
        }

        public void Execute(HierarchyNode node)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            var window = _package.FindToolWindow(typeof(MyToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }
            var windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}
