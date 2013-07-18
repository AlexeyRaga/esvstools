using System;
using EventStore.VSTools.CredentialsManager;
using Microsoft.Win32;
using NUnit.Framework;
using Shouldly;

namespace EventStore.VSTools.Tests.Credentials
{
    [TestFixture]
    public sealed class When_does_not_exist
    {
        [Test]
        public void Should_return_null_when_no_product()
        {
            var store = new RegistryCredentialsStore("UnitTest");
            var value = store.Load(new byte[] {198, 2, 3, 4});

            value.ShouldBe(null);
        }
    }

    [TestFixture]
    public sealed class When_exists
    {
        private const string ProductKey = "UnitTest";

        private byte[] _key;
        private byte[] _value;

        [SetUp]
        public void SetUp()
        {
            var store = new RegistryCredentialsStore(ProductKey);
            _key = Guid.NewGuid().ToByteArray();
            _value = Guid.NewGuid().ToByteArray();

            store.Save(_key, _value);
        }

        [Test]
        public void Should_write_and_read_value()
        {
            var store = new RegistryCredentialsStore(ProductKey);
            var readValue = store.Load(_key);

            readValue.ShouldNotBe(null);
            readValue.ShouldBe(_value);
        }

        [Test]
        public void Should_return_null_when_value_does_not_exist()
        {
            var store = new RegistryCredentialsStore(ProductKey);
            var key = Guid.NewGuid().ToByteArray();

            var nonExistentValue = store.Load(key);
            nonExistentValue.ShouldBe(null);
        }

        [Test]
        public void Should_delete()
        {
            var store = new RegistryCredentialsStore(ProductKey);
            store.Delete(_key);

            var store2 = new RegistryCredentialsStore(ProductKey);
            var shouldNotExist = store2.Load(_key);

            shouldNotExist.ShouldBe(null);
        }

        [TearDown]
        public void TearDown()
        {
            Registry.CurrentUser.DeleteSubKeyTree("Software\\" + ProductKey);
        }
    }
}
