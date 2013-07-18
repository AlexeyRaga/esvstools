using System.Security.Cryptography;

namespace EventStore.VSTools.CredentialsManager
{
    public sealed class CredentialsEncryptor : IEncryptCredentials
    {
        private readonly byte[] _salt = {0x90, 0x7e, 0x6b, 0xa, 0x7b, 0xd9, 0xe2, 0x3};

        public byte[] Encrypt(byte[] entropy, byte[] data)
        {
            return ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser);
        }

        public byte[] Decrypt(byte[] entropy, byte[] encryptedData)
        {
            return ProtectedData.Unprotect(encryptedData, entropy, DataProtectionScope.CurrentUser);
        }

        public byte[] Hash(byte[] data)
        {
            var hasher = new Rfc2898DeriveBytes(data, _salt, 256);
            return hasher.GetBytes(24);
        }
    }
}
