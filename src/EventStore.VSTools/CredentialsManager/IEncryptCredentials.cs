namespace EventStore.VSTools.CredentialsManager
{
    public interface IEncryptCredentials
    {
        byte[] Encrypt(byte[] entropy, byte[] data);
        byte[] Decrypt(byte[] entropy, byte[] encryptedData);
        byte[] Hash(byte[] data);
    }
}
