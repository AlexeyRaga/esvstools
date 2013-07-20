using System;
using EventStore.VSTools.CredentialsManager;
using NUnit.Framework;
using Shouldly;

namespace EventStore.VSTools.Tests.Credentials
{
    [TestFixture]
    public sealed class CredentialsSerializerTests
    {
        [Test]
        public void Should_serialize_and_deserialize_credentials()
        {
            var credentials = new VSTools.Credentials(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            var serialized = CredentialsSerializer.Serialize(credentials);
            var deserialized = CredentialsSerializer.Deserialize(serialized);

            deserialized.Username.ShouldBe(credentials.Username);
            deserialized.Password.ShouldBe(credentials.Password);
        }
    }
}
