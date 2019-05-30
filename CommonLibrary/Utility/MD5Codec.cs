using System;
using System.Security.Cryptography;
using System.Text;

namespace Xattacker.Utility
{
    public class MD5Codec
    {
        /// <summary>
        /// to hide the constructor
        /// </summary>
        protected MD5Codec()
        { 
        }

        public static byte[] GetMD5(byte[] content)
        {
            byte[] en_content = null;

            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                en_content = md5.ComputeHash(content);
                md5.Clear();
            }

            return en_content;
        }

        public static byte[] GetMD5(string content)
        {
            return GetMD5(Encoding.UTF8.GetBytes(content));
        }

        public static string GetMD5String(string content)
        {
            return GetMD5String(Encoding.UTF8.GetBytes(content));
        }

        public static string GetMD5String(byte[] content)
        {
            string md5_str = null;

            byte[] en_content = GetMD5(content);
            if (en_content != null)
            {
                StringBuilder builder = new StringBuilder();

                foreach (byte b in en_content)
                {
                    builder.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
                }

                md5_str = builder.ToString();
                en_content = null;
            }

            return md5_str;
        }
    }


    // MD5 extension function
    public static class Md5Extension
    {
        public static string GetMd5String(this string str)
        {
            return MD5Codec.GetMD5String(str);
        }
    }
}
