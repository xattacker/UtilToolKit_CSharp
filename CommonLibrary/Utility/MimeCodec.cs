using System;
using System.IO;
using System.Text;

namespace Xattacker.Utility
{
    /// <summary>
    /// provider mime encoding and decoding function 
    /// </summary>
    public class MimeCodec
    {
        /// <summary>
        /// to hide the constructor
        /// </summary>
        protected MimeCodec()
        {
        }

        public static string Encode(byte[] src)
        {
            return Convert.ToBase64String(src);
        }

        public static string Encode(string src)
        {
            return Encode(Encoding.UTF8.GetBytes(src));
        }

        public static byte[] Decode(string mimeStr)
        {
            return Convert.FromBase64String(mimeStr);
        }

        public static string DecodeToString(string mimeStr)
        {
            byte[] content = Decode(mimeStr);
            return Encoding.UTF8.GetString(content, 0, content.Length);
        }

        public static void Encode(Stream inputStream, Stream outputStream)
        {
            byte[] buffer = new byte[3 * 5120]; // must be a multiple of 3
            int index = -1;
            byte[] temp = null;

            while ((index = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                temp = Encoding.UTF8.GetBytes(Convert.ToBase64String(buffer, 0, index));

                outputStream.Write(temp, 0, temp.Length);
                outputStream.Flush();
                temp = null;
            }

            buffer = null;
            GC.Collect();
        }

        public static void Decode(Stream inputStream, Stream outputStream)
        {
            byte[] buffer = new byte[4 * 5120]; // must be a multiple of 4
            int index = -1;
            byte[] temp = null;

            while ((index = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                temp = Convert.FromBase64String(Encoding.UTF8.GetString(buffer, 0, index));

                outputStream.Write(temp, 0, temp.Length);
                outputStream.Flush();
                temp = null;
            }

            buffer = null;
            GC.Collect();
        }
    }


    // Mime extension function
    public static class MimeExtension
    {
        public static string GetMimeString(this string str)
        {
            return MimeCodec.Encode(str);
        }

        public static byte[] GetMimeDecode(this string str)
        {
            return MimeCodec.Decode(str);
        }
    }
}


