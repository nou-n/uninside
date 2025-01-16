using System.Text;
using System;
using System.Collections.Generic;

namespace uninside.Utils
{
    internal static class Utils
    {
        public static Dictionary<string, string> defaultHeaders;

        private static Random random;

        static Utils()
        {
            random = new Random();

            defaultHeaders = new Dictionary<string, string>();
            defaultHeaders.Add("User-Agent", Values.USER_AGENT);
            defaultHeaders.Add("Referer", "http://www.dcinside.com/");
        }

        public static Dictionary<string, string> GetHeaders(Dictionary<string, string> headers = null, Dictionary<string, string> _base = null)
        {
            if (_base == null) _base = defaultHeaders;
            if (headers == null) return _base;

            Dictionary<string, string> mergedHeaders = new Dictionary<string, string>(_base);
            foreach (KeyValuePair<string, string> header in headers)
            {
                mergedHeaders[header.Key] = header.Value;
            }

            return mergedHeaders;
        }

        public static string GenerateRandomString(int length, string characters, Random random = null)
        {
            if (random == null) random = Utils.random;

            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(characters.Length);
                result.Append(characters[index]);
            }
            return result.ToString();
        }

        public static long GenerateRandomLong(Random random = null)
        {
            if (random == null) random = Utils.random;

            ulong high = (ulong)random.Next(int.MinValue, int.MaxValue);
            ulong low = (ulong)random.Next(int.MinValue, int.MaxValue);
            return (long)((high << 32) | low);
        }
    }
}