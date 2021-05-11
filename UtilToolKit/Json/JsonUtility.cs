using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using System.Text;

namespace Xattacker.Utility.Json
{
    /// <summary>
    ///  general Json toolkit
    /// </summary>
    public class JsonUtility
    {
        /// <summary>
        /// to hide the constructor
        /// </summary>
        protected JsonUtility()
        {
        }

        /// <summary>
        /// Serialize Ojbect To Json
        /// </summary>
        /// <param name="obj"> Serialized Object </param>
        /// <returns> json </returns>
        public static string SerializeToJson(object obj)
        {
            string json = string.Empty;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                json = Encoding.UTF8.GetString(stream.ToArray());
            }

            return json;
        }

        /// <summary>
        /// Dserialize Ojbect from json
        /// </summary>
        /// <param name="xml"> json string </param>
        /// <returns> Object instance </returns>
        public static T DeserializeFromJson<T>(string json)
        {
            T obj = default(T);
            obj = (T)DeserializeFromJson(typeof(T), json);

            return obj;
        }

        /// <summary>
        /// Dserialize Ojbect from Json
        /// </summary>
        /// <param name="xml"> json string </param>
        /// <returns> Object instance </returns>
        public static object DeserializeFromJson(Type type, string json)
        {
            object obj = null;

            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
                    obj = serializer.ReadObject(ms);
                }
            }
            catch
            {
                obj = null;
            }

            return obj;
        }

        /// <summary>
        /// Dserialize Ojbect from Json
        /// </summary>
        /// <param name="json"> json string </param>
        /// <returns> Dictionary instance </returns>
        public static IDictionary<string, object> DeserializeToDictionary(string json)
        {
            return new JavaScriptSerializer().DeserializeObject(json) as IDictionary<string, object>;
        }
    }


    // Json extension function
    public static class JsonExtension
    {
        public static string SerializeToJson(this object obj)
        {
            return JsonUtility.SerializeToJson(obj);
        }

        public static T DeserializeFromJson<T>(this string str)
        {
            return JsonUtility.DeserializeFromJson<T>(str);
        }
    }
}
