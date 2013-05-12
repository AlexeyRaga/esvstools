using System;
using System.Net.Http;
using EventStore.VSTools.EventStore;
using NUnit.Framework;

namespace EventStore.VSTools.Tests.EventStore
{
    [TestFixture]
    public sealed class ProjectionDeploymentTest
    {
        private const string BaseAddress = "es.localtest.me";

        [Test, Ignore]
        public void IntegrationTest()
        {
            var address = EventStoreAddress.Get(BaseAddress);

            var client = new HttpClient {BaseAddress = new Uri(address)};

            var response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/projection/IntegrationTest/query"));
            response.Wait();

            var result = response.Result;
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
