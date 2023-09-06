using System.Collections.Generic;
using System.Linq;


namespace SubredditStats.Frontend.ConsoleClient.Utils
{
    public static class StringExtensions
    {
        public static bool ContainsNonAsciiChars(this string instance)
        {
            return instance.Any(c => !char.IsAscii(c));
        }

        public static bool StartsWithAny(this string instance, IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                return instance.StartsWith(value);
            }
            return false;
        }

        public static bool StartsWithAny(this string instance, IEnumerable<char> values)
        {
            foreach (var value in values)
            {
                return instance.StartsWith(value);
            }
            return false;
        }

        public static string RemoveAll(this string instance, IEnumerable<string> values)
        {
            var removed = instance;
            foreach (var value in values)
            {
                removed = removed.Replace(value, string.Empty);
            }
            return removed;
        }

        public static string RemoveAll(this string instance, IEnumerable<char> chars)
        {
            var removed = instance;
            foreach (var c in chars)
            {
                removed = removed.Replace(c.ToString(), string.Empty);
            }
            return removed;
        }
    }
}
