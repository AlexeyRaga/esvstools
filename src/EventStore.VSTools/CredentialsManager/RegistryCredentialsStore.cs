using System;
using Microsoft.Win32;

namespace EventStore.VSTools.CredentialsManager
{
    public sealed class RegistryCredentialsStore : IStoreEncryptedCredentials
    {
        private const string ContainerKey = "Container";
        private readonly string _productName;

        public RegistryCredentialsStore(string productName)
        {
            _productName = "Software\\" + productName;
        }

        public void Save(byte[] key, byte[] value)
        {
            using (var productNode = Registry.CurrentUser.CreateSubKey(_productName))
            {
                using (var container = productNode.CreateSubKey(ContainerKey))
                {
                    var stringKey = GetStringKey(key);
                    container.SetValue(stringKey, value, RegistryValueKind.Binary);
                }
            }
        }

        public byte[] Load(byte[] key)
        {
            var stringKey = GetStringKey(key);
            using (var productNode = Registry.CurrentUser.OpenSubKey(_productName, false))
            {
                if (productNode == null) return null;
                using (var container = productNode.OpenSubKey(ContainerKey))
                {
                    if (container == null) return null;
                    return container.GetValue(stringKey, null) as byte[];
                }
            }
        }

        public void Delete(byte[] key)
        {
            var stringKey = GetStringKey(key);
            using (var productNode = Registry.CurrentUser.OpenSubKey(_productName, true))
            {
                if (productNode == null) return;
                using (var container = productNode.OpenSubKey(ContainerKey, true))
                {
                    if (container == null) return;

                    container.DeleteValue(stringKey);
                }
            }
        }

        private static string GetStringKey(byte[] key)
        {
            return BitConverter.ToString(key).Replace("-", "");
        }
    }
}
