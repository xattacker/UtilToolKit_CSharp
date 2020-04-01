using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CommonLibrary.Utility
{
    public class StringUtility
    {
        ///
        /// 使用系統 kernel32.dll 進行轉換
        ///
        private const int LocaleSystemDefault = 0x0800;
        private const int LcmapSimplifiedChinese = 0x02000000;
        private const int LcmapTraditionalChinese = 0x04000000;

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LCMapString(int locale, int dwMapFlags, string lpSrcStr, int cchSrc,
                                              [Out] string lpDestStr, int cchDest);

        public static string ToSimplifiedChinese(string source)
        {
            string converted = new String(' ', source.Length);
            LCMapString(LocaleSystemDefault, LcmapSimplifiedChinese, source, source.Length, converted, source.Length);
            return converted;
        }

        public static string ToTraditionalChinese(string source)
        {
            string converted = new String(' ', source.Length);
            LCMapString(LocaleSystemDefault, LcmapTraditionalChinese, source, source.Length, converted, source.Length);
            return converted;
        }
    }


    // string extension function
    public static class StringExtension
    {
        public static string ToSimplifiedChinese(this string str)
        {
            return StringUtility.ToSimplifiedChinese(str);
        }

        public static string ToTraditionalChinese(this string str)
        {
            return StringUtility.ToTraditionalChinese(str);
        }
    }
}
