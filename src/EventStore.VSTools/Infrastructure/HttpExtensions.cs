using System.Net;

namespace EventStore.VSTools.Infrastructure
{
    internal static class HttpExtensions
    {
        public static string ToHttpUrl(this IPEndPoint endPoint, string rawUrl = null)
        {
            return string.Format("http://{0}:{1}/{2}",
                                 endPoint.Address,
                                 endPoint.Port,
                                 rawUrl != null ? rawUrl.TrimStart('/') : string.Empty);
        }

        public static string ToHttpUrl(this IPEndPoint endPoint, string formatString, params object[] args)
        {
            return string.Format("http://{0}:{1}/{2}",
                                 endPoint.Address,
                                 endPoint.Port,
                                 string.Format(formatString.TrimStart('/'), args));
        }
    }
}
