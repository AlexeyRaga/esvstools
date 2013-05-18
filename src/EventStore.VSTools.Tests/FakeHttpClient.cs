using System.Threading;
using System.Threading.Tasks;
using EventStore.VSTools.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;

namespace EventStore.VSTools.Tests
{
    internal sealed class FakeHttpClient : IHttpClient
    {
        public readonly List<RequestData> GetRequests = new List<RequestData>();
        public readonly List<RequestData> PutRequests = new List<RequestData>();
        public readonly List<RequestData> PostRequests = new List<RequestData>();

        private readonly Func<string, HttpResponse> _getRequest;
        private readonly Func<string, string, HttpResponse> _putRequest;
        private readonly Func<string, string, HttpResponse> _postRequest;

        private CountdownEvent _optionalCounter;
        
        public FakeHttpClient(
            Func<string, HttpResponse> getRequest,
            Func<string, string, HttpResponse> putRequest,
            Func<string, string, HttpResponse> postRequest)
        {
            _getRequest = getRequest ?? (u => new HttpResponse(HttpStatusCode.OK, String.Empty, String.Empty));
            _putRequest = putRequest ?? ((u, d) => new HttpResponse(HttpStatusCode.OK, String.Empty, String.Empty));
            _postRequest = postRequest ?? ((u, d) => new HttpResponse(HttpStatusCode.OK, String.Empty, String.Empty));
        }

        public FakeHttpClient() : this (null, null, null) { }


        public void SetCounter(CountdownEvent counter)
        {
            _optionalCounter = counter;
        }

        private void Count()
        {
            if (_optionalCounter != null) _optionalCounter.Signal();
        }

        public Task<HttpResponse> GetAsync(string url) { return Task.Factory.StartNew(() => Get(url)); }
        public HttpResponse Get(string url)
        {
            GetRequests.Add(new RequestData(url, String.Empty));
            Count();
            return _getRequest(url);
        }

        public Task<HttpResponse> PostAsync(string url, string data) { return Task.Factory.StartNew(() => Post(url, data)); }
        public HttpResponse Post(string url, string data)
        {
            PostRequests.Add(new RequestData(url, data));
            Count();
            return _postRequest(url, data);
        }

        public Task<HttpResponse> PutAsync(string url, string data) { return Task.Factory.StartNew(() => Put(url, data)); }
        public HttpResponse Put(string url, string data)
        {
            PutRequests.Add(new RequestData(url, data));
            Count();
            return _putRequest(url, data);
        }

        public void Clear()
        {
            GetRequests.Clear();
            PutRequests.Clear();
            PostRequests.Clear();
        }

        public sealed class RequestData 
        {
            public string Url { get; private set; }
            public string Data { get; private set; }

            public RequestData (string url, string data)
            {
                Url = url;
                Data = data;
            }
        }
    }
}
