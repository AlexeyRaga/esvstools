using EventStore.VS.Tools.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.Vs.Tools.Tests
{
    internal sealed class FakeHttpClient : IHttpClient
    {
        public readonly List<RequestData> GetRequests = new List<RequestData>();
        public readonly List<RequestData> PutRequests = new List<RequestData>();
        public readonly List<RequestData> PostRequests = new List<RequestData>();

        private Func<string, HttpResponse> _getRequest;
        private Func<string, string, HttpResponse> _putRequest;
        private Func<string, string, HttpResponse> _postRequest;

        public FakeHttpClient(
            Func<string, HttpResponse> getRequest,
            Func<string, string, HttpResponse> putRequest,
            Func<string, string, HttpResponse> postRequest)
        {
            _getRequest = getRequest ?? new Func<string, HttpResponse>(u => new HttpResponse(HttpStatusCode.OK, String.Empty, new NameValueCollection()));
            _putRequest = putRequest ?? new Func<string, string, HttpResponse>((u, d) => new HttpResponse(HttpStatusCode.OK, String.Empty, new NameValueCollection()));
            _postRequest = postRequest ?? new Func<string, string, HttpResponse>((u, d) => new HttpResponse(HttpStatusCode.OK, String.Empty, new NameValueCollection()));
        }

        public FakeHttpClient() : this (null, null, null) { }

        public HttpResponse Get(string url)
        {
            GetRequests.Add(new RequestData(url, String.Empty));
            return _getRequest(url);
        }

        public HttpResponse Post(string url, string data)
        {
            PostRequests.Add(new RequestData(url, data));
            return _postRequest(url, data);
        }

        public HttpResponse Put(string url, string data)
        {
            PutRequests.Add(new RequestData(url, data));
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
