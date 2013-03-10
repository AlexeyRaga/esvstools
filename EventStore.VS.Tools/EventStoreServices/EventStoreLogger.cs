using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace EventStore.VS.Tools.EventStoreServices
{
    public sealed class EventStoreLogger : EventStore.ClientAPI.ILogger
    {

        public void Debug(Exception ex, string format, params object[] args)
        {
            if (ex != null) format += Environment.NewLine + ex;
            System.Diagnostics.Debug.WriteLine(String.Format(format, args), "EventStore");
        }

        public void Debug(string format, params object[] args)
        {
            Debug(null, format, args);
        }

        public void Error(Exception ex, string format, params object[] args)
        {
            if (ex != null) format += Environment.NewLine + ex;
            format += Environment.NewLine;
            format = "Error: " + format;
            Output.Pane.OutputString(String.Format(format, args));
        }

        public void Error(string format, params object[] args)
        {
            Error(null, format, args);
        }

        public void Info(string format, params object[] args)
        {
            Info(null, format, args);
        }

        public void Info(Exception ex, string format, params object[] args)
        {
            if (ex != null) format += Environment.NewLine + ex;
            Trace.WriteLine(String.Format(format, args), "EventStore");
        }
    }
}
