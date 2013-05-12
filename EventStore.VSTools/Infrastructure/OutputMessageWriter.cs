using System;

namespace EventStore.VSTools.Infrastructure
{
    public sealed class OutputMessageWriter : IOutputMessages
    {
        public void Write(string message, params object[] parameters)
        {
            Output.Pane.OutputStringThreadSafe(String.Format(message, parameters));
        }

        public void WriteLine(string message, params object[] parameters)
        {
            if (!String.IsNullOrEmpty(message)) Write(message, parameters);
            Output.Pane.OutputStringThreadSafe(Environment.NewLine);
        }
    }
}
