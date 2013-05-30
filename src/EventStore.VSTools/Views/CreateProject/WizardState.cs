using System.Collections.Generic;
using EventStore.VSTools.EventStore;

namespace EventStore.VSTools.Views.CreateProject
{
    public sealed class WizardState
    {
        public string EventStoreConnection { get; set; }
        public List<ProjectionStatistics> ProjectionsToImport { get; private set; }

        public WizardState()
        {
            ProjectionsToImport = new List<ProjectionStatistics>();
        }
    }
}
