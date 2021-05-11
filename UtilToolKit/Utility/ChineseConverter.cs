
using System;
using System.Runtime.InteropServices;

namespace Xattacker.Utility
{
    public enum ChineseCharacters
    {
        Simplified,
        Traditional
    }


    /// <summary>
    /// Chinese Characters Converter
    /// </summary>
    public class ChineseConverter
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

        private ChineseCharacters characters;

        public ChineseConverter(ChineseCharacters characters)
        {
            this.characters = characters;
        }

        public string Convert(string source)
        {
            string converted = string.Empty;

            switch (this.characters)
            {
                case ChineseCharacters.Traditional:
                    converted = ToTraditionalChinese(source);
                    break;

                case ChineseCharacters.Simplified:
                    converted = ToSimplifiedChinese(source);
                    break;
            }

            return converted;
        }
    }
}
