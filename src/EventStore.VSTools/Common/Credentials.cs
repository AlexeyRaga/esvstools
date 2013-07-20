namespace EventStore.VSTools
{
    public sealed class Credentials
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public Credentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

    public static class CredentialsExtensions
    {
        public static bool IsEmpty(this Credentials credentials)
        {
            return credentials == null ||
                   (string.IsNullOrWhiteSpace(credentials.Username) && string.IsNullOrWhiteSpace(credentials.Password));
        }
    }
}
