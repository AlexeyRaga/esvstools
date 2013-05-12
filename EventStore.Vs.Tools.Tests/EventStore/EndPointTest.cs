using System;
using System.Net.Sockets;
using EventStore.VS.Tools.EventStore;
using NUnit.Framework;

namespace EventStore.Vs.Tools.Tests.EventStore
{
    [TestFixture]
    public sealed class EndPointTest
    {
        [Test]
        public void Should_parse_connection_string_with_ip()
        {
            var endpoint = EventStoreAddress.Get("127.0.0.1:2113");
            Assert.AreEqual("http://127.0.0.1:2113", endpoint);
        }

        [Test]
        public void Should_use_default_port()
        {
            var endpoint = EventStoreAddress.Get("localhost");
            Assert.AreEqual("http://localhost:2113", endpoint);
        }

        [Test, ExpectedException(typeof(SocketException))]
        public void Should_not_accept_crap()
        {
            EventStoreAddress.Get("some crap");
        }

        [Test]
        public void Should_accept_machine_name()
        {
            var endpoint = EventStoreAddress.Get(Environment.MachineName);
            Assert.AreEqual(String.Format("http://{0}:2113", Environment.MachineName), endpoint);
        }
    }
}
