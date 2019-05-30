using System;
using System.Text;

namespace Xattacker.Utility
{
    public enum DateTimeFormatType : ushort
    {
        YEAR = 0, // yyyy
        MONTH_SIMPLE, // yyyyMM
        DATE_SIMPLE, // yyyyMMdd
        TIME_SIMPLE, // HHmmss
        DATETIME_SIMPLE, // yyyyMMddHHmmss
        MONTH_COMPLETE, // yyyy-MM
        DATE_COMPLETE, // yyyy-MM-dd
        TIME_COMPLETE, // HH:mm:ss
        DATETIME_COMPLETE // yyyy-MM-dd HH:mm:ss
    }


    public interface IDateTimeProvider
    {
        DateTime GetCurrentTime();
    }


    public class DateTimeUtility
    {
        private static IDateTimeProvider datetimeProvider;

        static DateTimeUtility()
        {
            // lazy initialization
            datetimeProvider = new DefaultDateTimeProvider();
        }

        /// <summary>
        /// to hide the constructor
        /// </summary>
        protected DateTimeUtility()
        {
        }

        /// <summary>
        /// set a Custom DateTimeProivder
        /// </summary>
        /// <param name="provider"> IDateTimeProvider implementor </param>
        public static void SetDateTimeProvider(IDateTimeProvider provider)
        {
            if (provider != null)
            {
                datetimeProvider = provider;
            }
        }

        /// <summary>
        /// get datetime format string
        /// </summary>
        /// <param name="type"> format type </param>
        /// <returns> datetime string </returns>
        public static string GetDateTimeString(DateTimeFormatType type)
        {
            return GetCurrentTime().ToString(GetFormatTemplate(type));
        }

        /// <summary>
        /// get datetime format string
        /// </summary>
        /// <param name="time"> reference DateTime instance </param>
        /// <param name="type"> format type </param>
        /// <returns> datetime string </returns>
        public static string GetDateTimeString(DateTime time, DateTimeFormatType type)
        {
            return time.ToString(GetFormatTemplate(type));
        }

        /// <summary>
        /// get datetime format string
        /// </summary>
        /// <param name="time"> reference DateTime instance </param>
        /// <param name="format"> format string</param>
        /// <returns> datetime string </returns>
        public static string GetDateTimeString(DateTime time, string format)
        {
            return time.ToString(format);
        }

        /// <summary>
        /// get datetime format string
        /// </summary>
        /// <param name="time"> reference DateTime instance </param>
        /// <param name="type"> format type </param>
        /// <returns> datetime string </returns>
        public static string GetDateTimeString(ulong timestamp, DateTimeFormatType type)
        {
            return ConvertTimeStampToDateTime(timestamp).ToString(GetFormatTemplate(type));
        }

        /// <summary>
        /// get datetime format string
        /// </summary>
        /// <param name="time"> reference DateTime instance </param>
        /// <param name="format"> format string</param>
        /// <returns> datetime string </returns>
        public static string GetDateTimeString(ulong timestamp, string format)
        {
            return ConvertTimeStampToDateTime(timestamp).ToString(format);
        }

        /// <summary>
        /// get datetime from format string
        /// </summary>
        /// <param name="parsedStr"> parsed string </param>
        /// <param name="type"> format type </param>
        /// <returns> datetime instance </returns>
        public static DateTime? ParseDateTime(string parsedStr, DateTimeFormatType type)
        {
            return DateTime.ParseExact(parsedStr, GetFormatTemplate(type), null);
        }

        /// <summary>
        /// get datetime from format string
        /// </summary>
        /// <param name="parsedStr"> parsed string </param>
        /// <param name="type"> format type </param>
        /// <returns> datetime instance </returns>
        public static DateTime? ParseDateTime(string parsedStr, string format)
        {
            return DateTime.ParseExact(parsedStr, format, null);
        }

        /// <summary>
        /// get total milliseconds from 1970 0101 00:00
        /// </summary>
        /// <returns> milliseconds </returns>
        public static ulong GetCurrentTimeStamp()
        {
            return GetTimeStamp(GetCurrentTime());
        }

        /// <summary>
        /// get total milliseconds from 1970 0101 00:00
        /// </summary>
        /// <param name="time"> reference DateTime instance </param>
        /// <returns> milliseconds </returns>
        public static ulong GetTimeStamp(DateTime time)
        {
            double timestamp = (time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;

            return Convert.ToUInt64(timestamp);
        }

        /// <summary>
        /// covert timestamp to DateTime instance
        /// </summary>
        /// <param name="timestamp"> milliseconds from 1970 0101 00:00 </param>
        /// <returns> DateTime </returns>
        public static DateTime ConvertTimeStampToDateTime(ulong timestamp)
        {
            DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime time = converted.AddMilliseconds(Convert.ToDouble(timestamp));

            return time.ToLocalTime();
        }


        #region private function

        /// <summary>
        /// get current datetime
        /// </summary>
        /// <returns> datetime </returns>
        private static DateTime GetCurrentTime()
        {
            DateTime time = datetimeProvider.GetCurrentTime();

            // fault tolerant
            if (time == null)
            {
                time = DateTime.Now;
            }

            return time;
        }

        /// <summary>
        /// get datetime format template string
        /// </summary>
        /// <param name="type"> format type </param>
        /// <returns> format template </returns>
        private static string GetFormatTemplate(DateTimeFormatType type)
        {
            string format = null;

            switch (type)
            {
                case DateTimeFormatType.YEAR:
                    format = "yyyy";
                    break;

                case DateTimeFormatType.MONTH_SIMPLE:
                    format = "yyyyMM";
                    break;

                case DateTimeFormatType.DATE_SIMPLE:
                    format = "yyyyMMdd";
                    break;

                case DateTimeFormatType.TIME_SIMPLE:
                    format = "HHmmss";
                    break;

                case DateTimeFormatType.DATETIME_SIMPLE:
                    format = "yyyyMMddHHmmss";
                    break;

                case DateTimeFormatType.MONTH_COMPLETE:
                    format = "yyyy-MM";
                    break;

                case DateTimeFormatType.DATE_COMPLETE:
                    format = "yyyy-MM-dd";
                    break;

                case DateTimeFormatType.TIME_COMPLETE:
                    format = "HH:mm:ss";
                    break;

                case DateTimeFormatType.DATETIME_COMPLETE:
                    format = "yyyy-MM-dd HH:mm:ss";
                    break;
            }

            return format;
        }

        /// <summary>
        ///  a default local time provider 
        /// </summary>
        private class DefaultDateTimeProvider : IDateTimeProvider
        {
            public DateTime GetCurrentTime()
            {
                return DateTime.Now;
            }
        }

        #endregion
    }


    // DateTime extension function
    public static class DateTimeExtension
    {
        public static string GetDateTimeString(this DateTime date, string format)
        {
            return DateTimeUtility.GetDateTimeString(date, format);
        }

        public static string GetDateTimeString(this DateTime date, DateTimeFormatType type)
        {
            return DateTimeUtility.GetDateTimeString(date, type);
        }

        public static ulong GetTimeStamp(this DateTime time)
        {
            return DateTimeUtility.GetTimeStamp(time);
        }

        public static DateTime? ParseDateTime(this string str, string format)
        {
            return DateTimeUtility.ParseDateTime(str, format);
        }

        public static DateTime? ParseDateTime(this string str, DateTimeFormatType type)
        {
            return DateTimeUtility.ParseDateTime(str, type);
        }
    }
}
