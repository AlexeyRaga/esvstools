using System;
using System.Net;
using Microsoft.VisualStudio.Project;

namespace EventStore.VS.Tools.EventStore
{
    public sealed class EventStoreAddress
    {
        public static string Get(ProjectNode projectNode)
        {
            var connectionString = projectNode.CurrentConfig.GetPropertyValue(Constants.EventStore.ConnectionString);
            if (String.IsNullOrWhiteSpace(connectionString))
                throw new EventStoreConnectionException(
                    "Unable to connect to the EventStore. Connection string is not specified.",
                    HttpStatusCode.ServiceUnavailable);

            return Get(connectionString);
        }

        public static string Get(string connectionString)
        {
            if (String.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            var hostAndPort = ParseHostAndPort(connectionString);

            EnsureHostNameExists(hostAndPort.Item1);

            return String.Format("http://{0}:{1}", hostAndPort.Item1, hostAndPort.Item2);
        }

        private static void EnsureHostNameExists(string hostName)
        {
            var addresses = Dns.GetHostAddresses(hostName);
            if (addresses.Length == 0)
            {
                throw new EventStoreConnectionException(
                    "Unable to retrieve address from specified host name: " + hostName,
                    HttpStatusCode.ServiceUnavailable
                    );
            }
        }

        private static Tuple<string, int> ParseHostAndPort(string connectionString)
        {
            var hostAndPort = connectionString.Split(new[] { ':' }, 2);
            var rawPort = hostAndPort.Length == 2 ? hostAndPort[1] : "2113";

            int port;
            if (!Int32.TryParse(rawPort, out port))
                throw new InvalidOperationException("EventStore Connection String: port is incorrect: " + rawPort);

            return Tuple.Create(hostAndPort[0].Trim(' ', '/', '\\'), port);
        }
    }
}
