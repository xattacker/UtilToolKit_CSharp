using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.Utility
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
                    converted = StringUtility.ToTraditionalChinese(source);
                    break;

                case ChineseCharacters.Simplified:
                    converted = StringUtility.ToSimplifiedChinese(source);
                    break;
            }

            return converted;
        }
    }
}
