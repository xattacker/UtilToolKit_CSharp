using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Xattacker.Utility
{
    /// <summary>
    ///  handle INI file to store or load value
    /// </summary>
    public class IniProperties
    {
        #region dll import

            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString
                (
                string section,
                string key,
                string val,
                string filePath
                );

            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString
                (
                string section,
                string key,
                string def,
                StringBuilder retVal,
                int size,
                string filePath
                );

        #endregion


        #region constructor

        /// <summary>
        ///  Constructor.
        /// </summary>
        /// <param name="path"> file path </param>
        public IniProperties(string path)
        {
            this.FilePath = path;
        }

        #endregion


        #region data member

        public string FilePath { get; private set; }

        #endregion


        #region public function

        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this.FilePath);
        }
            
        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns> value </returns>
        public string ReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(500);

            int i = GetPrivateProfileString(section, key, string.Empty, temp, 500, this.FilePath);
               
            return temp.Length > 0 ? temp.ToString() : string.Empty;
        }

        #endregion
    }
}