using System;
using System.Linq;
using System.Net;

namespace EventStore.VS.Tools.EventStoreServices
{
    public sealed class EventStoreConnectionFactory
    {
        public static IPEndPoint GetEventStoreEndPoint(string connectionString)
        {

            if (String.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            var hostAndPort = ParseHostAndPort(connectionString);
            var endpoint = GetIPEndPointFromHostName(hostAndPort.Item1, hostAndPort.Item2);

            return endpoint;
        }

        private static Tuple<string, int> ParseHostAndPort(string connectionString)
        {
            var hostAndPort = connectionString.Split(new[] { ':' }, 2);
            var rawPort = hostAndPort.Length == 2 ? hostAndPort[1] : "1113";

            int port;
            if (!Int32.TryParse(rawPort, out port))
                throw new InvalidOperationException("EventStore Connection String: port is incorrect: " + rawPort);

            return Tuple.Create(hostAndPort[0], port);
        }

        private static IPEndPoint GetIPEndPointFromHostName(string hostName, int port)
        {
            var addresses = Dns.GetHostAddresses(hostName);
            if (addresses.Length == 0)
            {
                throw new ArgumentException(
                    "Unable to retrieve address from specified host name.",
                    "hostName"
                );
            }
            return new IPEndPoint(addresses.Last(), port); // Port gets validated here.
        }

    }
}
