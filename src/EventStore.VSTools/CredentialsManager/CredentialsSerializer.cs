using System;
using System.Text;

namespace EventStore.VSTools.CredentialsManager
{
    public static class CredentialsSerializer
    {
        public static byte[] Serialize(Credentials credentials)
        {
            var usernameLength = credentials.Username.Length;
            var str = String.Format("{0},{1}:{2}", usernameLength, credentials.Username, credentials.Password);
            return Encoding.UTF8.GetBytes(str);
        }

        public static Credentials Deserialize(byte[] data)
        {
            var str = Encoding.UTF8.GetString(data);
            var lengthAndPayload = str.Split(new[] { ',' }, 2);
            if (lengthAndPayload.Length != 2) throw new InvalidOperationException("Wrong credentials data");

            var usernameLength = Int32.Parse(lengthAndPayload[0]);
            var username = lengthAndPayload[1].Substring(0, usernameLength);
            var password = lengthAndPayload[1].Substring(usernameLength + 1);

            return new Credentials(username, password);
        }
    }
}
