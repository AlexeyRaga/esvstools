using System;
using EventStore.VSTools.CredentialsManager;
using NUnit.Framework;
using Shouldly;

namespace EventStore.VSTools.Tests.Credentials
{
    [TestFixture]
    public sealed class CredentialEncryptorTests
    {
        [Test]
        public void Should_generate_the_same_hash_for_the_same_data()
        {
            var encryptor = new CredentialsEncryptor();

            var data = Guid.NewGuid().ToByteArray();

            var hash1 = encryptor.Hash(data);
            var hash2 = encryptor.Hash(data);

            hash1.ShouldBe(hash2);
        }
    }
}
