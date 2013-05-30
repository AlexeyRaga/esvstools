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

            if (result.StatusCode != HttpStatusCode.OK)
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

            if (!response.HasStatus(HttpStatusCode.OK))
                return EventStoreResponse<ProjectionConfig>.Fail(response.StatusCode);

            var config = new ProjectionConfig(response.GetJsonContentAsDynamic());
            return EventStoreResponse<ProjectionConfig>.Success(response.StatusCode, config);
        }

        public async Task<EventStoreResponse<ProjectionStatistics>> GetStatisticsAsync(string projectionName)
        {
            var projectionLocation = "/projection/" + projectionName + "/statistics";
            var locaionUri = _baseAddress + projectionLocation;
            var response = await _httpClient.GetAsync(locaionUri);

            if (!response.HasStatus(HttpStatusCode.OK))
                return EventStoreResponse<ProjectionStatistics>.Fail(response.StatusCode);

            var stats = new ProjectionStatistics(response.GetJsonContentAsDynamic());
            return EventStoreResponse<ProjectionStatistics>.Success(response.StatusCode, stats);
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
