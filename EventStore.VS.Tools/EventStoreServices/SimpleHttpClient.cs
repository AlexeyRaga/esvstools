using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace EventStore.VS.Tools.EventStoreServices
{
    internal sealed class SimpleHttpClient
    {
        public SimpleWebResponse Get(string url)
        {
            var request = CreateJsonRequest(url, "GET");
            return ExecuteRequestGetResponse(request);
        }

        public SimpleWebResponse Post(string url, string data)
        {
            return MakeRequest(url, "POST", data);
        }

        public SimpleWebResponse Put(string url, string data)
        {
            return MakeRequest(url, "PUT", data);
        }

        private static SimpleWebResponse MakeRequest(string url, string method, string data)
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

        private static SimpleWebResponse ExecuteRequestGetResponse(WebRequest request)
        {
            using (var response = request.GetResponse())
            {
                var headers = response.SupportsHeaders ? response.Headers : new NameValueCollection();
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    var content = reader.ReadToEnd();
                    return new SimpleWebResponse(content, headers);
                }
            }
        }
    }

    internal sealed class SimpleWebResponse
    {
        public NameValueCollection Headers { get; private set; }
        public string Content { get; private set; }
        public SimpleWebResponse(string content, NameValueCollection headers)
        {
            Content = content;
            Headers = headers;
        }
    }
}
