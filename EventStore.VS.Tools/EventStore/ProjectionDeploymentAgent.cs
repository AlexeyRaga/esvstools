using EventStore.VS.Tools.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
            var projectionName = GetProjectionName(message.FilePath);
            var content = File.ReadAllText(message.FilePath);  

            if (ProjectionExistsInEventStore(projectionName))
                UpdateProjection(projectionName, content);
            else
                CreateProjection(projectionName, content);
        }

        private void CreateProjection(string projectionName, string content)
        {
            var isEmitEnabled = "no";
            var isCheckpointEnabled = "yes";
            var isEnabled = "yes";
            var projectionLocation = String.Format("/projections/continuous?name={0}&type=JS&emit={1}&checkpoints={2}&enabled={3}",
                projectionName, isEmitEnabled, isCheckpointEnabled, isEnabled);

            var projectionUri = _eventStoreEndpoint.ToHttpUrl(projectionLocation);

            var result = _httpClient.Post(projectionUri, content);

            if (result.StatusCode != HttpStatusCode.Accepted && result.StatusCode != HttpStatusCode.Created && result.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException(String.Format("Unable to create projection {0}, the response was: {1}", projectionName, result.StatusCode));
        }

        private void UpdateProjection(string projectionName, string content)
        {
            var projectionLocation = "/projection/" + projectionName + "/query?type=JS";
            var locationUri = _eventStoreEndpoint.ToHttpUrl(projectionLocation);

            var result = _httpClient.Put(locationUri, content);
            if (result.StatusCode != HttpStatusCode.OK && result.StatusCode != HttpStatusCode.Accepted)
                throw new InvalidOperationException(String.Format("Unable to update projection '{0}', the response was: {1}", projectionName, result.StatusCode));
        }

        private bool ProjectionExistsInEventStore(string projectionName)
        {
            var projectionLocation = "/projection/" + projectionName + "/query";

            var locaionUri = _eventStoreEndpoint.ToHttpUrl(projectionLocation);
            var response = _httpClient.Get(projectionLocation);

            return response.StatusCode == HttpStatusCode.OK;
        }

        private static string GetProjectionName(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }
    }
}
