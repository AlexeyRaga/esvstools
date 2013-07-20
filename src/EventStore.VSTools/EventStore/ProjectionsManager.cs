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
        Task TestConnectionAsync(Credentials credentials);

        Task<HttpResponse> CreateProjectionAsync(string projectionName, string content, bool enable, bool enableCheckpoint, bool enableEmit);
        Task<HttpResponse> UpdateProjectionQueryAsync(string projectionName, string content);
    }

    public sealed class ProjectionsManager : IProjectionsManager
    {
        private readonly string _baseAddress;
        private readonly IProvideCredentials _credentialsProvider;
        private readonly IHttpClient _httpClient;
        private const int MaxAttempts = 3;

        public ProjectionsManager(string eventStoreAddress, IProvideCredentials credentialsProvider, IHttpClient httpClient)
        {
            _baseAddress = EventStoreAddress.Get(eventStoreAddress);
            _credentialsProvider = credentialsProvider;
            _httpClient = httpClient;
        }

        private async Task<HttpResponse> ExecuteWithCredentials(string resource, int attempt, int numberOfAttempts,
                                                                Func<Credentials, Task<HttpResponse>> function)
        {
            var credentials = _credentialsProvider.GetFor(resource, attempt == 0);
            if (credentials == null) RaiseUnauthorizedException(resource);

            var result = await function(credentials);
            if (result.IsAuthorized()) return result;

            if (attempt >= numberOfAttempts-1)
                RaiseUnauthorizedException(resource);

            return await ExecuteWithCredentials(resource, ++attempt, numberOfAttempts, function);
        }

        public async Task TestConnectionAsync(Credentials credentials)
        {
            var url = _baseAddress + "/projections/all-non-transient";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsAuthorized())
                RaiseUnauthorizedException(_baseAddress);

            if (!response.InStatus(HttpStatusCode.OK))
                throw new EventStoreConnectionException(String.Format("Unable to connect: {0}. ({1} - {2})",
                                                                      response.Content, (int) response.StatusCode,
                                                                      response.StatusCode.ToString().Wordify()),
                                                        response.StatusCode);
        }

        private async Task<HttpResponse> ExecuteWithCredentials(string resource,
                                                                Func<Credentials, Task<HttpResponse>> function)
        {
            return await ExecuteWithCredentials(resource, 0, MaxAttempts, function);
        }

        public async Task<EventStoreResponse<List<ProjectionStatistics>>> GetAllNonTransientAsync()
        {
            var url = _baseAddress + "/projections/all-non-transient";

            var response = await (ExecuteWithCredentials(_baseAddress, credentials => _httpClient.GetAsync(url)));

            if (response.InStatus(HttpStatusCode.OK))
                throw new EventStoreConnectionException(
                    String.Format("Cannot connect to {0} to get the projections list", _baseAddress), response.StatusCode);

            var jsonContent = response.GetJsonContentAsDynamic();

            var statsList = new List<ProjectionStatistics>();
            foreach (var projInfo in jsonContent.projections)
                statsList.Add(new ProjectionStatistics(projInfo));

            return EventStoreResponse<List<ProjectionStatistics>>.Success(response.StatusCode, statsList);
        }

        public async Task<EventStoreResponse<ProjectionConfig>> GetConfigAsync(string projectionName)
        {
            var projectionLocation = "/projection/" + projectionName + "/query?config=yes";
            var locaionUri = _baseAddress + projectionLocation;

            var response = await (ExecuteWithCredentials(_baseAddress, credentials => _httpClient.GetAsync(locaionUri)));

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

            var response = await (ExecuteWithCredentials(_baseAddress, credentials => _httpClient.GetAsync(locaionUri)));

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

            var response = await (ExecuteWithCredentials(_baseAddress, credentials => _httpClient.PutAsync(locationUri, query)));

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
                throw new EventStoreConnectionException(
                    String.Format("Unable to create projection {0}, the response was: {1}", projectionName, response.StatusCode),
                    response.StatusCode);

            return response;
        }

        public async Task<HttpResponse> CreateProjectionAsync(string projectionName, string content, bool enable, bool enableCheckpoint, bool enableEmit)
        {
            var projectionLocation = String.Format("/projections/continuous?name={0}&type=JS&emit={1}&checkpoints={2}&enabled={3}",
                projectionName, enableEmit, enableCheckpoint, enable);

            var locationUri = _baseAddress + projectionLocation;

            var response = await (ExecuteWithCredentials(_baseAddress, credentials => _httpClient.PutAsync(locationUri, content)));

            if (response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.OK)
                throw new EventStoreConnectionException(
                    String.Format("Unable to create projection {0}, the response was: {1}", projectionName, response.StatusCode),
                    response.StatusCode);

            return response;
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

        private static void RaiseUnauthorizedException(string resourceName)
        {
            throw new UnauthorisedRequestException(
                String.Format("{0}: {1} - {2}", resourceName, (int) HttpStatusCode.Unauthorized,
                              HttpStatusCode.Unauthorized.ToString().Wordify()));
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
