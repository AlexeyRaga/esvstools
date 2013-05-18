using System;
using EventStore.VSTools.Infrastructure;
using Microsoft.VisualStudio.Shell.Interop;

namespace EventStore.VSTools.EventStore
{
    public sealed class QueryViewConsumer : IConsume<ProjectionExecuted>
    {
        private readonly EventStorePackage _package;

        public QueryViewConsumer(EventStorePackage package)
        {
            _package = package;
        }

        public void Consume(ProjectionExecuted message)
        {
            var window = _package.FindToolWindow(typeof(QueryViewWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }

            var queryView = (QueryViewWindow) window;
            queryView.ShowQueryResult(message.Name, message.Uri, message.Result);

            var windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}
