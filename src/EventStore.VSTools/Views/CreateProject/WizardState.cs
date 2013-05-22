using System.Collections.Generic;

namespace EventStore.VSTools.Views.CreateProject
{
    public sealed class WizardState
    {
        public string EventStoreConnection { get; set; }
        public List<string> ProjectionsToImport { get; set; }
    }
}
