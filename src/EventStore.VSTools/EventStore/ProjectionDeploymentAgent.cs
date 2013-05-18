using System.Threading.Tasks;
using EventStore.VSTools;
using EventStore.VSTools.Infrastructure;
using System;
using System.Net;

namespace EventStore.VSTools.EventStore
{
    public sealed class ProjectionDeploymentAgent : IConsume<DeployProjection>
    {
        private readonly IHttpClient _httpClient;
        private readonly IPublish<IMessage> _publisher;

        public ProjectionDeploymentAgent(IHttpClient httpClient, IPublish<IMessage> publisher)
        {
            if (httpClient == null) throw new ArgumentNullException("httpClient");
            _httpClient = httpClient;
            _publisher = publisher;
        }

        public ProjectionDeploymentAgent(IPublish<IMessage> publisher) : this(new SimpleHttpClient(), publisher) { }

        public async void Consume(DeployProjection message)
        {
            var projectionResponse = await GetExistingProjection(message.EventStoreAddress, message.Name);

            if (projectionResponse.StatusCode == HttpStatusCode.NotFound)
            {
                CreateProjection(message.EventStoreAddress, message.Name, message.Content, message.Enable, message.EnableCheckpoint, message.EnableEmit);
                _publisher.Publish(new ProjectionCreated(message.Name));
            }
            else
            {
                var deployCodeHash = Hash.Compute(message.Content);
                var existingCodeHash = Hash.Compute(projectionResponse.Content);

                if (deployCodeHash != existingCodeHash)
                {
                    UpdateProjection(message.EventStoreAddress, message.Name, message.Content);
                    _publisher.Publish(new ProjectionUpdated(message.Name));
                }
                else
                {
                    _publisher.Publish(new ProjectionNotUpdatedBecauseNotChanged(message.Name));
                }
            }                
        }

        private async void CreateProjection(string eventStoreAddress, string projectionName, string content, bool enable, bool enableCheckpoint, bool enableEmit)
        {
            var projectionLocation = String.Format("/projections/continuous?name={0}&type=JS&emit={1}&checkpoints={2}&enabled={3}",
                projectionName, enableEmit, enableCheckpoint, enable);

            var projectionUri = eventStoreAddress + projectionLocation;

            var result = await _httpClient.PostAsync(projectionUri, content);

            if (result.StatusCode != HttpStatusCode.Accepted && result.StatusCode != HttpStatusCode.Created && result.StatusCode != HttpStatusCode.OK)
                throw new EventStoreConnectionException(
                    String.Format("Unable to create projection {0}, the response was: {1}", projectionName, result.StatusCode), 
                    result.StatusCode);
        }

        private async void UpdateProjection(string eventStoreAddress, string projectionName, string content)
        {
            var projectionLocation = "/projection/" + projectionName + "/query?type=JS";
            var locationUri = eventStoreAddress + projectionLocation;

            var result = await _httpClient.PutAsync(locationUri, content);
            if (result.StatusCode != HttpStatusCode.OK && result.StatusCode != HttpStatusCode.Accepted)
                throw new EventStoreConnectionException(
                    String.Format("Unable to create projection {0}, the response was: {1}", projectionName, result.StatusCode),
                    result.StatusCode);
        }

        private async Task<HttpResponse> GetExistingProjection(string eventStoreAddress, string projectionName)
        {
            var projectionLocation = "/projection/" + projectionName + "/query";
            var locaionUri = eventStoreAddress + projectionLocation;

            var response = await _httpClient.GetAsync(locaionUri);

            return response;
        }
    }
}
