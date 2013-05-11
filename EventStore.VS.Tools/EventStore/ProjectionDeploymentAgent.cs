using EventStore.VS.Tools.Infrastructure;
using System;
using System.Net;

namespace EventStore.VS.Tools.EventStore
{
    public sealed class ProjectionDeploymentAgent : IConsume<DeployProjection>
    {
        private IHttpClient _httpClient;
        private IPEndPoint _eventStoreEndpoint;

        public ProjectionDeploymentAgent(IPEndPoint eventStoreEndpoint, IHttpClient httpClient)
        {
            if (httpClient == null) throw new ArgumentNullException("httpClient");
            if (eventStoreEndpoint == null) throw new ArgumentNullException("eventStoreEndpoint");

            _httpClient = httpClient;
            _eventStoreEndpoint = eventStoreEndpoint;
        }

        public void Consume(DeployProjection message)
        {
            if (ProjectionExistsInEventStore(message.Name))
                UpdateProjection(message.Name, message.Content);
            else
                CreateProjection(message.Name, message.Content);
        }

        private void CreateProjection(string projectionName, string content)
        {
            const string isEmitEnabled = "no";
            const string isCheckpointEnabled = "yes";
            const string isEnabled = "yes";
            var projectionLocation = String.Format("/projections/continuous?name={0}&type=JS&emit={1}&checkpoints={2}&enabled={3}",
                projectionName, isEmitEnabled, isCheckpointEnabled, isEnabled);

            var projectionUri = _eventStoreEndpoint.ToHttpUrl(projectionLocation);

            var result = _httpClient.Post(projectionUri, content);

            if (result.StatusCode != HttpStatusCode.Accepted && result.StatusCode != HttpStatusCode.Created && result.StatusCode != HttpStatusCode.OK)
                throw new EventStoreConnectionException(
                    String.Format("Unable to create projection {0}, the response was: {1}", projectionName, result.StatusCode), 
                    result.StatusCode);
        }

        private void UpdateProjection(string projectionName, string content)
        {
            var projectionLocation = "/projection/" + projectionName + "/query?type=JS";
            var locationUri = _eventStoreEndpoint.ToHttpUrl(projectionLocation);

            var result = _httpClient.Put(locationUri, content);
            if (result.StatusCode != HttpStatusCode.OK && result.StatusCode != HttpStatusCode.Accepted)
                throw new EventStoreConnectionException(
                    String.Format("Unable to create projection {0}, the response was: {1}", projectionName, result.StatusCode),
                    result.StatusCode);
        }

        private bool ProjectionExistsInEventStore(string projectionName)
        {
            var projectionLocation = "/projection/" + projectionName + "/query";

            var locaionUri = _eventStoreEndpoint.ToHttpUrl(projectionLocation);
            var response = _httpClient.Get(locaionUri);

            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
