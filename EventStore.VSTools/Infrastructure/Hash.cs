using System;
using System.Security.Cryptography;
using System.Text;

namespace EventStore.VSTools.Infrastructure
{
    public static class Hash
    {
        private static MD5CryptoServiceProvider _cryptoService = new MD5CryptoServiceProvider();

        internal static string Compute(string plainText)
        {
            var hashBytes = _cryptoService.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            var sb = new StringBuilder();
            foreach (var hashByte in hashBytes)
                sb.Append(Convert.ToString(hashByte, 16).PadLeft(2, '0'));
            return sb.ToString();
        }
    }
}
