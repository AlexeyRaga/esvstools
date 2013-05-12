namespace EventStore.VS.Tools.Infrastructure
{
    public interface IOutputMessages
    {
        void Write(string message, params object[] parameters);
        void WriteLine(string message, params object[] parameters);
    }

    public static class OutputMessageExtensions
    {
        public static void WriteLine(this IOutputMessages output)
        {
            output.WriteLine(null);
        }
    }
}
