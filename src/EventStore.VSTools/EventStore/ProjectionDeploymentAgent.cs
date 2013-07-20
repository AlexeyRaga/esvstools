using System;
using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools.EventStore
{
    public sealed class ProjectionDeploymentAgent : IConsume<DeployProjection>
    {
        private readonly Func<string, IProjectionsManager> BuildProjectionsManager;
        private readonly IPublish<IMessage> _publisher;

        public ProjectionDeploymentAgent(Func<string, IProjectionsManager> projectionsManagerFactory, IPublish<IMessage> publisher)
        {
            BuildProjectionsManager = projectionsManagerFactory;
            _publisher = publisher;
        }

        public async void Consume(DeployProjection message)
        {
            var projectionsManager = BuildProjectionsManager(message.EventStoreAddress);
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
