using EventStore.VSTools;
using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools.EventStore
{
    public sealed class DeploymentProcessOutputConsumer : IConsume<ProjectionDeploymentEvent>
    {
        private readonly IOutputMessages _output;

        public DeploymentProcessOutputConsumer(IOutputMessages output)
        {
            _output = output;
        }

        public void Consume(ProjectionDeploymentEvent message)
        {
            ((dynamic) this).Handle((dynamic) message);
        }

        public void Handle(ProjectionCreated evt)
        {
            _output.WriteLine("[Create] Projection '{0}' has beed deployed.", evt.Name);
        }

        public void Handle(ProjectionUpdated evt)
        {
            _output.WriteLine("[Update] Projection '{0}' has been deployed.", evt.Name);
        }

        public void Handle(ProjectionNotUpdatedBecauseNotChanged evt)
        {
            _output.WriteLine("[Skip] Projection '{0}' has not beed deployed because it was not changed.", evt.Name);
        }

        public void Handle(object _) {}
    }
}
