using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace EventStore.VS.Tools
{
    internal static class Output
    {
        private const string EventStoreOutputWindowGuid = "78ACFE2C-5E73-4713-8763-4CE836E12AD4";
        public static readonly IVsOutputWindowPane Pane;

        static Output()
        {
            const string customTitle = "EventStore";
            var outWindow = (IVsOutputWindow) Package.GetGlobalService(typeof (SVsOutputWindow));

            // Use e.g. Tools -> Create GUID to make a stable, but unique GUID for your pane.
            // Also, in a real project, this should probably be a static constant, and not a local variable
            var customGuid = new Guid(EventStoreOutputWindowGuid);
            outWindow.CreatePane(ref customGuid, customTitle, 1, 1);

            IVsOutputWindowPane customPane;
            outWindow.GetPane(ref customGuid, out customPane);

            Pane = customPane;
        }
    }
}
