using System;
using System.Net;
using System.Threading;
using EventStore.VSTools.EventStore;
using EventStore.VSTools.Infrastructure;
using NUnit.Framework;

namespace EventStore.VSTools.Tests.EventStore
{
    [TestFixture]
    public sealed class ProjectionManagerTests
    {
        private const string EventStoreAddress = "localhost:2113";


        [Test]
        public void Should_update_existing_projection()
        {
            var projectionName = Guid.NewGuid().ToString();
            var projectionContent = Guid.NewGuid().ToString();

            var httpClient = new FakeHttpClient(u => new HttpResponse(HttpStatusCode.OK, String.Empty, String.Empty), null, null);

            var credentialsProvider = new FakeCredentialsService();
            var manager = new ProjectionsManager(EventStoreAddress, credentialsProvider, httpClient);

            var counter = new CountdownEvent(1);
            httpClient.SetCounter(counter);

            manager.UpdateProjectionQueryAsync(projectionName, projectionContent);

            Assert.IsTrue(counter.Wait(3000));

            CollectionAssert.IsEmpty(httpClient.PostRequests);
            CollectionAssert.IsEmpty(httpClient.GetRequests);
            CollectionAssert.IsNotEmpty(httpClient.PutRequests);

            StringAssert.Contains(projectionName, httpClient.PutRequests[0].Url);
        }

        [Test]
        public void Should_create_new_projection_if_not_exist()
        {
            var projectionName = Guid.NewGuid().ToString();
            var projectionContent = Guid.NewGuid().ToString();

            var httpClient = new FakeHttpClient(u => new HttpResponse(HttpStatusCode.OK, String.Empty, String.Empty), null, null);
            var credentialsProvider = new FakeCredentialsService();
            var manager = new ProjectionsManager(EventStoreAddress, credentialsProvider, httpClient);

            var counter = new CountdownEvent(1);
            httpClient.SetCounter(counter);

            manager.CreateProjectionAsync(projectionName, projectionContent, true, true, false);

            Assert.IsTrue(counter.Wait(3000));

            CollectionAssert.IsEmpty(httpClient.PutRequests);
            CollectionAssert.IsEmpty(httpClient.GetRequests);
            CollectionAssert.IsNotEmpty(httpClient.PostRequests);

            StringAssert.Contains(projectionName, httpClient.PostRequests[0].Url);
        }

        [Test, ExpectedException, Ignore]
        public void Should_throw_if_http_error()
        {
            var projectionName = Guid.NewGuid().ToString();
            var projectionContent = Guid.NewGuid().ToString();

            var response = new HttpResponse(HttpStatusCode.InternalServerError, String.Empty, String.Empty);

            var httpClient = new FakeHttpClient(u => response, (u, d) => response, (u, d) => response);
            var counter = new CountdownEvent(1);
            httpClient.SetCounter(counter);

            var credentialsProvider = new FakeCredentialsService();
            var manager = new ProjectionsManager(EventStoreAddress, credentialsProvider, httpClient);

            manager.GetConfigAsync(projectionName);

            Assert.IsTrue(counter.Wait(3000));
        }
    }
}
