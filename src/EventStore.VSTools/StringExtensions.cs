using System.Text.RegularExpressions;

namespace EventStore.VSTools
{
    public static class StringExtensions
    {
        public static string Wordify(this string pascalCaseString)
        {
            var r = new Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])");
            return r.Replace(pascalCaseString, " ${x}");
        }
    }
}
