using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using EventStore.VS.Tools.Infrastructure;

namespace EventStore.VS.Tools.EventStoreServices
{
    internal sealed class ProjectionsManagerLight
    {
        private readonly IPEndPoint _endPoint;
        private readonly HttpClient _client;

        public ProjectionsManagerLight(IPEndPoint endPoint)
        {
            _endPoint = endPoint;
            _client = new HttpClient();
        }

        public string[] GetAllNonSystem()
        {
            var url = _endPoint.ToHttpUrl("/projections/any");
            var response = _client.Get(url);

            dynamic esResponse = JObject.Parse(response.Content);
            var existingProjectionNames = ((IEnumerable<dynamic>) esResponse.projections)
                .Select(x => new {Name = (string) x.name.ToString(), Value = x})
                .Where(x => !x.Name.StartsWith("$"))
                .Select(x => x.Name)
                .ToArray();

            return existingProjectionNames;
        }

        public string Update(string name, string content)
        {
            var url = _endPoint.ToHttpUrl("/projection/{0}/query?type=JS", name);
            var result = _client.Put(url, content);

            var location = result.Headers["Location"];
            return location;
        }

        public string CreateContinuous(string name, string content, bool enableEmit, bool enableCheckpoint, bool enabled)
        {
            var isEmitEnabled = enableEmit ? 1 : 0;
            var isEnabled = enabled ? 1 : 0;
            var isCheckpointEnabled = enableCheckpoint ? 1 : 0;
            var url = _endPoint.ToHttpUrl(
                "/projections/continuous?name={0}&type=JS&emit={1}&checkpoints={2}&enabled={3}",
                name, isEmitEnabled, isCheckpointEnabled, isEnabled);

            var result = _client.Post(url, content);

            var location = result.Headers["Location"];
            return location;
        }

    }
}
