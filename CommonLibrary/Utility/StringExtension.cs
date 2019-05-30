using System;

namespace Xattacker.Utility
{
    // string extension function
    public static class StringExtension
    {
        public static string Reverse(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            char[] chars = str.ToCharArray();
            Array.Reverse(chars);
            return new String(chars);
        }
    }
}
