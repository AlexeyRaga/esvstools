using System.Threading;
using EventStore.VS.Tools;
using EventStore.VS.Tools.EventStore;
using EventStore.VS.Tools.Infrastructure;
using NUnit.Framework;
using System;
using System.Net;

namespace EventStore.Vs.Tools.Tests.EventStore
{
    [TestFixture]
    public sealed class ProjectionDeploymentAgentTest
    {
        private const string EventStoreAddress = "http://localhost:2113";

        [Test]
        public void Should_update_existing_projection()
        {
            var projectionName = Guid.NewGuid().ToString();
            var projectionContent = Guid.NewGuid().ToString();

            var httpClient = new FakeHttpClient(u => new HttpResponse(HttpStatusCode.OK, String.Empty), null, null);
            var counter = new CountdownEvent(2);
            httpClient.SetCounter(counter);

            var publisher = new FakePublisher();

            var agent = new ProjectionDeploymentAgent(httpClient, publisher);
            agent.Consume(new DeployProjection(EventStoreAddress, projectionName, projectionContent));

            Assert.IsTrue(counter.Wait(3000));

            CollectionAssert.IsEmpty(httpClient.PostRequests);
            CollectionAssert.IsNotEmpty(httpClient.GetRequests);
            CollectionAssert.IsNotEmpty(httpClient.PutRequests);

            StringAssert.Contains(projectionName, httpClient.GetRequests[0].Url);
            StringAssert.Contains(projectionName, httpClient.PutRequests[0].Url);
        }

        [Test]
        public void Should_create_new_projection_if_not_exist()
        {
            var projectionName = Guid.NewGuid().ToString();
            var projectionContent = Guid.NewGuid().ToString();

            var httpClient = new FakeHttpClient(u => new HttpResponse(HttpStatusCode.NotFound, String.Empty), null, null);
            var counter = new CountdownEvent(2);
            httpClient.SetCounter(counter);

            var publisher = new FakePublisher();

            var agent = new ProjectionDeploymentAgent(httpClient, publisher);
            agent.Consume(new DeployProjection(EventStoreAddress, projectionName, projectionContent));

            Assert.IsTrue(counter.Wait(3000));

            CollectionAssert.IsEmpty(httpClient.PutRequests);
            CollectionAssert.IsNotEmpty(httpClient.GetRequests);
            CollectionAssert.IsNotEmpty(httpClient.PostRequests);

            StringAssert.Contains(projectionName, httpClient.GetRequests[0].Url);
            StringAssert.Contains(projectionName, httpClient.PostRequests[0].Url);
        }

        [Test, ExpectedException, Ignore]
        public void Should_throw_if_http_error()
        {
            var projectionName = Guid.NewGuid().ToString();
            var projectionContent = Guid.NewGuid().ToString();

            var response = new HttpResponse(HttpStatusCode.InternalServerError, String.Empty);

            var httpClient = new FakeHttpClient(u => response, (u, d) => response, (u, d) => response);
            var counter = new CountdownEvent(2);
            httpClient.SetCounter(counter);

            var publisher = new FakePublisher();

            var agent = new ProjectionDeploymentAgent(httpClient, publisher);
            agent.Consume(new DeployProjection(EventStoreAddress, projectionName, projectionContent));
            Assert.IsTrue(counter.Wait(3000));
        }
    }
}
