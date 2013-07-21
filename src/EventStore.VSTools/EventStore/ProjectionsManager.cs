using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EventStore.VSTools.Infrastructure;

namespace EventStore.VSTools.EventStore
{
    public interface IProjectionsManager
    {
        Task<EventStoreResponse<List<ProjectionStatistics>>> GetAllNonTransientAsync();
        Task<EventStoreResponse<ProjectionConfig>> GetConfigAsync(string projectionName);
        Task<EventStoreResponse<ProjectionStatistics>> GetStatisticsAsync(string projectionName);

        Task<HttpResponse> CreateProjectionAsync(string projectionName, string content, bool enable, bool enableCheckpoint, bool enableEmit);
        Task<HttpResponse> UpdateProjectionQueryAsync(string projectionName, string content);
    }

    public sealed class ProjectionsManager : IProjectionsManager
    {
        private readonly string _baseAddress;
        private readonly IHttpClient _httpClient;

        public ProjectionsManager(string eventStoreConnectionString, IHttpClient httpClient)
        {
            _baseAddress = EventStoreAddress.Get(eventStoreConnectionString);
            _httpClient = httpClient;
        }

        public async Task<EventStoreResponse<List<ProjectionStatistics>>> GetAllNonTransientAsync()
        {
            var url = _baseAddress + "/projections/all-non-transient";
            var result = await _httpClient.GetAsync(url);

            if (!result.InStatus(HttpStatusCode.OK))
                throw new EventStoreConnectionException(
                    String.Format("Cannot connect to {0} to get the projections list", _baseAddress), result.StatusCode);

            var jsonContent = result.GetJsonContentAsDynamic();

            var statsList = new List<ProjectionStatistics>();
            foreach (var projInfo in jsonContent.projections)
                statsList.Add(new ProjectionStatistics(projInfo));

            return EventStoreResponse<List<ProjectionStatistics>>.Success(result.StatusCode, statsList);
        }

        public async Task<EventStoreResponse<ProjectionConfig>> GetConfigAsync(string projectionName)
        {
            var projectionLocation = "/projection/" + projectionName + "/query?config=yes";
            var locaionUri = _baseAddress + projectionLocation;

            var response = await _httpClient.GetAsync(locaionUri);

            if (!response.InStatus(HttpStatusCode.OK, HttpStatusCode.NotFound))
                RaiseCannotConnectToGetProjectionException(projectionName, response);

            if (!response.InStatus(HttpStatusCode.OK))
                return EventStoreResponse<ProjectionConfig>.Fail(response.StatusCode);

            var config = new ProjectionConfig(response.GetJsonContentAsDynamic());
            return EventStoreResponse<ProjectionConfig>.Success(response.StatusCode, config);
        }

        public async Task<EventStoreResponse<ProjectionStatistics>> GetStatisticsAsync(string projectionName)
        {
            var projectionLocation = "/projection/" + projectionName + "/statistics";
            var locaionUri = _baseAddress + projectionLocation;
            var response = await _httpClient.GetAsync(locaionUri);

            if (!response.InStatus(HttpStatusCode.OK, HttpStatusCode.NotFound))
                RaiseCannotConnectToGetProjectionException(projectionName, response);

            if (!response.InStatus(HttpStatusCode.OK))
                return EventStoreResponse<ProjectionStatistics>.Fail(response.StatusCode);

            var stats = new ProjectionStatistics(response.GetJsonContentAsDynamic());
            return EventStoreResponse<ProjectionStatistics>.Success(response.StatusCode, stats);
        }

        public async Task<HttpResponse> UpdateProjectionQueryAsync(string projectionName, string query)
        {
            var projectionLocation = "/projection/" + projectionName + "/query?type=JS";
            var locationUri = _baseAddress + projectionLocation;

            var result = await _httpClient.PutAsync(locationUri, query);
            if (result.StatusCode != HttpStatusCode.OK && result.StatusCode != HttpStatusCode.Accepted)
                throw new EventStoreConnectionException(
                    String.Format("Unable to create projection {0}, the response was: {1}", projectionName, result.StatusCode),
                    result.StatusCode);

            return result;
        }

        public async Task<HttpResponse> CreateProjectionAsync(string projectionName, string content, bool enable, bool enableCheckpoint, bool enableEmit)
        {
            var projectionLocation = String.Format("/projections/continuous?name={0}&type=JS&emit={1}&checkpoints={2}&enabled={3}",
                projectionName, enableEmit, enableCheckpoint, enable);

            var projectionUri = _baseAddress + projectionLocation;

            var result = await _httpClient.PostAsync(projectionUri, content);

            if (result.StatusCode != HttpStatusCode.Accepted && result.StatusCode != HttpStatusCode.Created && result.StatusCode != HttpStatusCode.OK)
                throw new EventStoreConnectionException(
                    String.Format("Unable to create projection {0}, the response was: {1}", projectionName, result.StatusCode),
                    result.StatusCode);

            return result;
        }

        private static void RaiseCannotConnectToGetProjectionException(string projectionName, HttpResponse response)
        {
            if (!response.InStatus(HttpStatusCode.OK, HttpStatusCode.NotFound))
                throw new EventStoreConnectionException(
                    String.Format("Cannot get projection '{0}' from the EventStore: {1}",
                                  projectionName,
                                  response.StatusCode.ToString().Wordify()),
                    response.StatusCode);
        }
    }

    public sealed class EventStoreResponse<T>
    {
        public T Result { get; private set; }
        public HttpStatusCode Status { get; private set; }
        public bool IsSuccessful { get; private set; }

        private EventStoreResponse(HttpStatusCode status, bool success, T result)
        {
            Status = status;
            IsSuccessful = success;
            Result = result;
        }

        public static EventStoreResponse<T> Success(HttpStatusCode status, T result)
        {
            return new EventStoreResponse<T>(status, true, result);
        }

        public static EventStoreResponse<T> Fail(HttpStatusCode status)
        {
            return new EventStoreResponse<T>(status, false, default(T));
        }
    }
}
