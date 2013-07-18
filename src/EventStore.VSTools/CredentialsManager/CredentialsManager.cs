using System.Text;

namespace EventStore.VSTools.CredentialsManager
{
    public sealed class CredentialsManager : ICredentialsManager
    {
        private readonly IEncryptCredentials _encryptor;
        private readonly IStoreEncryptedCredentials _store;

        public CredentialsManager(IEncryptCredentials encryptor, IStoreEncryptedCredentials store)
        {
            _encryptor = encryptor;
            _store = store;
        }

        public void Put(string resource, Credentials credentials)
        {
            var entropy = BuildEntropy(resource);
            var resourceKey = _encryptor.Hash(entropy);

            var serializedCredentials = CredentialsSerializer.Serialize(credentials);
            var encryptedCredentials = _encryptor.Encrypt(entropy, serializedCredentials);

            _store.Save(resourceKey, encryptedCredentials);
        }

        public Credentials Get(string resource)
        {
            var entropy = BuildEntropy(resource);
            var resourceKey = _encryptor.Hash(entropy);

            var encryptedCredentials = _store.Load(resourceKey);
            if (encryptedCredentials == null) return null;

            var serializedCredentials = _encryptor.Decrypt(entropy, encryptedCredentials);

            var credentials = CredentialsSerializer.Deserialize(serializedCredentials);
            return credentials;
        }

        private static byte[] BuildEntropy(string resource)
        {
            return Encoding.UTF8.GetBytes(resource);
        }

        
    }
}
