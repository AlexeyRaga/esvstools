using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.VS.Tools.Infrastructure
{
    public interface IHttpClient
    {
        HttpResponse Get(string url);
        HttpResponse Post(string url, string data);
        HttpResponse Put(string url, string date);

        Task<HttpResponse> GetAsync(string url);
        Task<HttpResponse> PostAsync(string url, string data);
        Task<HttpResponse> PutAsync(string url, string date);
    }

    internal sealed class SimpleHttpClient : IHttpClient
    {

        public HttpResponse Get(string url)
        {
            return MakeRequest(url, HttpMethod.Get, null);
        }

        public async Task<HttpResponse> GetAsync(string url)
        {
            return await MakeRequestAsync(url, HttpMethod.Get, null);
        }

        public HttpResponse Post(string url, string data)
        {
            return MakeRequest(url, HttpMethod.Post, data);
        }

        public async Task<HttpResponse> PostAsync(string url, string data)
        {
            return await MakeRequestAsync(url, HttpMethod.Post, data);
        }

        public HttpResponse Put(string url, string data)
        {
            return MakeRequest(url, HttpMethod.Put, data);
        }

        public async Task<HttpResponse> PutAsync(string url, string data)
        {
            return await MakeRequestAsync(url, HttpMethod.Put, data);
        }

        private static HttpResponse MakeRequest(string url, HttpMethod method, string data)
        {
            var responseTask = MakeRequestAsync(url, method, data);
            responseTask.Start();
            return responseTask.GetAwaiter().GetResult();
        }

        private static async Task<HttpResponse> MakeRequestAsync(string url, HttpMethod method, string data)
        {
            var request = CreateJsonRequest(url, method);
            if (!String.IsNullOrEmpty(data))
            {
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                request.Content = content;
            }

            using (var client = new HttpClient())
            {
                var result = await client.SendAsync(request);
                var response = await result.Content.ReadAsStringAsync();
                return new HttpResponse(result.StatusCode, response);
            }
        }

        private static HttpRequestMessage CreateJsonRequest(string url, HttpMethod method)
        {
            var request = new HttpRequestMessage(method, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return request;
        }
    }

    public sealed class HttpResponse
    {
        public string Content { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }

        public HttpResponse(HttpStatusCode statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}
