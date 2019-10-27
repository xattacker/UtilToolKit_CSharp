using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Xattacker.Utility.Except;

namespace Xattacker.Binary
{
    public class BinaryUtility
    {
        /// <summary>
        /// to hide the constructor
        /// </summary>
        protected BinaryUtility()
        { 
        }

        public static byte[] SerializeToBinary(object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            formatter.Serialize(ms, obj);

            ms.Flush();
            ms.Close();
            ms.Dispose();

            byte[] content = ms.ToArray();
            
            return content;
        }

        /// <summary>
        /// Dserialize Ojbect from binary
        /// </summary>
        /// <param name="content"> binary content </param>
        /// <returns> Object instance </returns>
        public static object DeserializeFromBinary(byte[] content)
        {
            object obj = null;

            try
            {
                MemoryStream ms = new MemoryStream(content);
                BinaryFormatter formatter = new BinaryFormatter();

                obj = formatter.Deserialize(ms);

                ms.Flush();
                ms.Close();
                ms.Dispose();
            }
            catch (Exception ex)
            {
                throw new CustomException(ErrorId.DESERIALIZATION, typeof(BinaryUtility), ex);
            }

            return obj;
        }

        public static T DeserializeFromBinary<T>(byte[] content)
        {
            T obj = default(T);

            obj = (T) DeserializeFromBinary(content);

            return obj;
        }
    }
}
