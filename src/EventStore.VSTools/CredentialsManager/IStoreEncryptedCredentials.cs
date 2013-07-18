namespace EventStore.VSTools.CredentialsManager
{
    public interface IStoreEncryptedCredentials
    {
        void Save(byte[] key, byte[] value);
        byte[] Load(byte[] key);
        void Delete(byte[] key);
    }
}
