using System;

namespace EventStore.VSTools.EventStore
{
    public sealed class ProjectionStatistics
    {
        public string Name { get; private set; }
        public string Mode { get; private set; }
        public bool IsEnabled { get; private set; }

        public ProjectionStatistics(dynamic jsonData)
        {
            Name = jsonData.name;
            Mode = jsonData.mode;
            IsEnabled = "running".Equals((string)jsonData.status, StringComparison.InvariantCultureIgnoreCase);

        }
    }
}
