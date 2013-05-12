using System;
using EventStore.VS.Tools;
using EventStore.VS.Tools.EventStore;
using EventStore.VS.Tools.Infrastructure;
using NUnit.Framework;

namespace EventStore.Vs.Tools.Tests.EventStore
{
    [TestFixture]
    public sealed class OutputConsumerTest
    {
        private FakeOutput _output;
        private IConsume<ProjectionDeploymentEvent> _consumer;

        [SetUp]
        public void SetUp()
        {
            _output = new FakeOutput();
            _consumer = new DeploymentProcessOutputConsumer(_output);
        }
        
        [Test]
        public void Should_output_created_message()
        {
            var projectionName = Guid.NewGuid().ToString();

            _consumer.Consume(new ProjectionCreated(projectionName));

            Assert.AreEqual(2, _output.OutputItems.Count);
            StringAssert.Contains(projectionName, _output.OutputItems[0]);
            Assert.AreEqual(Environment.NewLine, _output.OutputItems[1]);

        }

        [Test]
        public void Should_output_updated_message()
        {
            var projectionName = Guid.NewGuid().ToString();

            _consumer.Consume(new ProjectionUpdated(projectionName));

            Assert.AreEqual(2, _output.OutputItems.Count);
            StringAssert.Contains(projectionName, _output.OutputItems[0]);
            Assert.AreEqual(Environment.NewLine, _output.OutputItems[1]);
        }

        [Test]
        public void Should_output_skipped_message()
        {
            var projectionName = Guid.NewGuid().ToString();

            _consumer.Consume(new ProjectionNotUpdatedBecauseNotChanged(projectionName));

            Assert.AreEqual(2, _output.OutputItems.Count);
            StringAssert.Contains(projectionName, _output.OutputItems[0]);
            Assert.AreEqual(Environment.NewLine, _output.OutputItems[1]);
        }
    }
}
