using System.Net;
using EventStore.VSTools;
using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools.EventStore
{
    public sealed class ProjectionRunner : IConsume<RunProjection>
    {
        private const string TransientProjectionUri = "/projections/transient?emit=no&checkpoints=no&enabled=yes";

        private readonly IHttpClient _httpClient;
        private readonly IPublish<IMessage> _publisher;

        public ProjectionRunner(IHttpClient httpClient, IPublish<IMessage> publisher)
        {
            _httpClient = httpClient;
            _publisher = publisher;
        }

        public ProjectionRunner(IPublish<IMessage> publisher) : this(new SimpleHttpClient(), publisher)
        {
            
        }

        public async void Consume(RunProjection message)
        {
            var projectionUri = message.EventStoreAddress + TransientProjectionUri;

            var result = await _httpClient.PostAsync(projectionUri, message.Content);

            if (result.StatusCode != HttpStatusCode.OK && result.StatusCode != HttpStatusCode.Created)
            {
                var status = string.Format("{0}, {1}", (int) result.StatusCode, result.StatusCode.ToString().Wordify());

                var content = (!string.IsNullOrEmpty(result.Content)) ? result.Content : status;
                content = string.Format("Unable to execute projection '{0}': {1}", message.Name, content);
                var errorEvent = new EventStoreConnectionError(status, content);

                _publisher.Publish(errorEvent);

                return;
            }

            var location = result.Location;
            var projection = await _httpClient.GetAsync(location + "/state");
            _publisher.Publish(new ProjectionExecuted(message.Name, location, projection.Content));
        }
    }
}
