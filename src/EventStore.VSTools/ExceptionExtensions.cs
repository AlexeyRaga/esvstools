using System;

namespace EventStore.VSTools
{
    public static class ExceptionExtensions
    {
        public static string GetDeepMessage(this Exception ex)
        {
            if (ex.InnerException == null) return ex.Message;
            return ex.Message + ": " + ex.InnerException.GetDeepMessage();
        }
    }
}
