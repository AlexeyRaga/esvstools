using System;

namespace EventStore.VSTools.Infrastructure
{
    public sealed class ErrorMessageWriter : IOutputErrorMessages
    {
        private static void Write(string message, params object[] parameters)
        {
            Output.Pane.OutputStringThreadSafe(String.Format("ERROR: " + message, parameters));
        }

        public void WriteError(string message, params object[] parameters)
        {
            if (!String.IsNullOrEmpty(message)) Write(message, parameters);
            Output.Pane.OutputStringThreadSafe(Environment.NewLine);
        }
    }
}
