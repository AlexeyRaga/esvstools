using System;
using System.Collections.Generic;
using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools.Tests
{
    public sealed class FakeOutput : IOutputMessages
    {
        public readonly List<string> OutputItems = new List<string>(); 

        public void Write(string message, params object[] parameters)
        {
            OutputItems.Add(String.Format(message, parameters));
        }

        public void WriteLine(string message, params object[] parameters)
        {
            OutputItems.Add(String.Format(message, parameters));
            OutputItems.Add(Environment.NewLine);
        }
    }
}
