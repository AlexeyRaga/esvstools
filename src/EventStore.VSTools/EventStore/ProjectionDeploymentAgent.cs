using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools.EventStore
{
    public sealed class ProjectionDeploymentAgent : IConsume<DeployProjection>
    {
        private readonly IProjectionsManagerFactory _projectionsManagerFactory;
        private readonly IPublish<IMessage> _publisher;

        public ProjectionDeploymentAgent(IProjectionsManagerFactory projectionsManagerFactory , IPublish<IMessage> publisher)
        {
            _projectionsManagerFactory = projectionsManagerFactory;
            _publisher = publisher;
        }

        public async void Consume(DeployProjection message)
        {
            var projectionsManager = _projectionsManagerFactory.BuildProjectionsManager(message.EventStoreAddress);
            var existingProjectionConfig = await projectionsManager.GetConfigAsync(message.Name);

            if (existingProjectionConfig.IsSuccessful)
            {
                //projection exists
                var deployCodeHash = Hash.Compute(message.Content);
                var existingCodeHash = Hash.Compute(existingProjectionConfig.Result.Query);
                if (deployCodeHash != existingCodeHash)
                {
                    await projectionsManager.UpdateProjectionQueryAsync(message.Name, message.Content);
                    _publisher.Publish(new ProjectionUpdated(message.Name));
                }
            }
            else
            {
                //new projection
                await projectionsManager.CreateProjectionAsync(
                    message.Name,
                    message.Content,
                    message.Enable,
                    message.EnableCheckpoint,
                    message.EnableEmit);

                _publisher.Publish(new ProjectionCreated(message.Name));
            }             
        }
    }
}
