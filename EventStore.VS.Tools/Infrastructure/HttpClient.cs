using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace EventStore.VS.Tools.Infrastructure
{
    public interface IHttpClient
    {
        HttpResponse Get(string url);
        HttpResponse Post(string url, string data);
        HttpResponse Put(string url, string date);
    }

    internal sealed class HttpClient : IHttpClient
    {
        public HttpResponse Get(string url)
        {
            var request = CreateJsonRequest(url, "GET");
            return ExecuteRequestGetResponse(request);
        }

        public HttpResponse Post(string url, string data)
        {
            return MakeRequest(url, "POST", data);
        }

        public HttpResponse Put(string url, string data)
        {
            return MakeRequest(url, "PUT", data);
        }

        private static HttpResponse MakeRequest(string url, string method, string data)
        {
            var request = CreateJsonRequest(url, method);
            if (!String.IsNullOrEmpty(data))
            {
                var payload = Encoding.UTF8.GetBytes(data);

                request.ContentLength = payload.Length;
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(payload, 0, payload.Length);
                }
            }

            return ExecuteRequestGetResponse(request);
        }

        private static HttpWebRequest CreateJsonRequest(string url, string method)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = method;

            return request;
        }

        private static HttpResponse ExecuteRequestGetResponse(WebRequest request)
        {
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var headers = response.SupportsHeaders ? response.Headers : new NameValueCollection();
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    var content = reader.ReadToEnd();
                    return new HttpResponse(response.StatusCode, content, headers);
                }
            }
        }
    }

    public sealed class HttpResponse
    {
        public NameValueCollection Headers { get; private set; }
        public string Content { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }

        public HttpResponse(HttpStatusCode statusCode, string content, NameValueCollection headers)
        {
            StatusCode = statusCode;
            Content = content;
            Headers = headers;
        }
    }
}
