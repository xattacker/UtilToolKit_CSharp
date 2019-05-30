using System;
using System.IO;
using System.Text;

using Xattacker.Utility.Except;

namespace Xattacker.Utility
{
    public enum LogType : ushort
    {
        DAILY = 0, // one log file per day
        MONTHLY, // one log file per month
        YEARLY, // one log file per year
        ALL_ONE // only one log file
    }

    public enum LogLevel : ushort
    {
        EXCEPTION = 0, // only log out exception message
        ERROR, // log out exception and error messages
        WARNING, // log out exception, error and warning messages 
        INFO // log out all kinds of messages
    }


    /// <summary>
    /// the class provide multi instance and singleton mode
    /// </summary>
    public class Logger
    {
        #region data member

        public static Logger Instance { private set; get; }

        private string dirPath;
        private readonly object lockObject;

        #endregion


        #region constructor

        public Logger(LogType type, LogLevel level, string dirPath, bool logToFile, bool logToConsole)
        {
            if (string.IsNullOrEmpty(dirPath))
            {
                throw new CustomException(ErrorId.INVALID_PARARMETER, GetType());
            }

            if (!FileUtility.IsDirExisted(dirPath))
            {
                throw new CustomException(ErrorId.PATH_NOT_FOUND, GetType());
            }

            this.Type = type;
            this.Level = level;
            this.dirPath = dirPath;
            this.EnableLogToFile = logToFile;
            this.EnableLogToConsole = logToConsole;

            this.lockObject = new object();
        }

        #endregion


        #region static function

        public static void ConstructInstance(string dirPath, bool logToFile, bool logToConsole)
        {
            ConstructInstance(LogType.DAILY, LogLevel.INFO, dirPath, logToFile, logToConsole);
        }

        public static void ConstructInstance(LogType type, LogLevel level, string dirPath, bool logToFile, bool logToConsole)
        {
            if (Instance == null)
            {
                Instance = new Logger(type, level, dirPath, logToFile, logToConsole);
            }
        }

        public static void ReleaseInstance()
        {
            Instance = null;
        }

        #endregion


        #region date member related function

        public LogType Type { private set; get; }

        public LogLevel Level { set; get; }

        public bool EnableLogToFile { set; get; }

        public bool EnableLogToConsole { set; get; }

        #endregion


        #region public function

        public void Log(string message)
        {
            if (this.Level < LogLevel.INFO || !this.IsLogEnable)
            {
                return;
            }


            if (!string.IsNullOrEmpty(message))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(" Log: ");
                builder.Append(message);

                this.LogHandle(builder.ToString());
            }
        }

        public void LogWarning(string message, Type thrownType)
        {
            if (this.Level < LogLevel.WARNING || !this.IsLogEnable)
            {
                return;
            }


            if (!string.IsNullOrEmpty(message) && thrownType != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(" Warning in class ");
                builder.Append(thrownType.ToString());
                builder.Append(": ");
                builder.Append(message);

                this.LogHandle(builder.ToString());
            }
        }

        public void LogError(string message, Type thrownType)
        {
            if (this.Level < LogLevel.ERROR || !this.IsLogEnable)
            {
                return;
            }


            if (!string.IsNullOrEmpty(message) && thrownType != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(" Error in class ");
                builder.Append(thrownType.ToString());
                builder.Append(": ");
                builder.Append(message);

                this.LogHandle(builder.ToString());
            }
        }

        public void LogEx(Exception ex, Type thrownType)
        {
            if (!this.IsLogEnable)
            {
                return;
            }


            if (ex != null && thrownType != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(" Exception in class ");
                builder.Append(thrownType.ToString());
                builder.Append(":\n");
                builder.Append(ex.ToString());

                this.LogHandle(builder.ToString());
            }
        }

        // get today's log
        public string GetLog()
        {
            return this.GetLog(DateTime.Now);
        }

        // get some day's log
        public string GetLog(DateTime time)
        {
            string log = string.Empty;

            if (time != null)
            {
                string filePath = this.GetFilePath(time);

                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        log = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }

            return log;
        }

        // delete some day's log
        public void DeleteLog(DateTime time)
        {
            if (time != null)
            {
                string filePath = this.GetFilePath(time);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        // delete today's log
        public void DeleteLog()
        {
            this.DeleteLog(DateTime.Now);
        }

        // delete all log files under the folder
        public void ClearLogs()
        {
            if (Directory.Exists(this.dirPath))
            {
                // can not delete the folder directly and recreate it
                // because in web environment, the folder could not be created by normal way
                string[] files = Directory.GetFiles(this.dirPath);
                if (files != null && files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        #endregion


        #region private function

        private bool IsLogEnable
        {
            get
            {
                return this.EnableLogToConsole || this.EnableLogToFile;
            }
        }

        private void LogHandle(string message)
        {
            lock (this.lockObject)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("[");
                builder.Append(DateTimeUtility.GetDateTimeString(DateTimeFormatType.DATETIME_COMPLETE));
                builder.Append("]");
                builder.Append(message);
                builder.Append("\n");

                message = builder.ToString();

                if (this.EnableLogToConsole)
                {
                    Console.WriteLine(message);
                }

                if (this.EnableLogToFile)
                {
                    string filePath = this.GetFilePath(DateTime.Now);

                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.WriteLine(message);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
        }

        private string GetFilePath(DateTime time)
        {
            string fileName = null;

            switch (this.Type)
            {
                case LogType.DAILY:
                    fileName = DateTimeUtility.GetDateTimeString(time, DateTimeFormatType.DATE_COMPLETE) + ".txt";
                    break;

                case LogType.MONTHLY:
                    fileName = DateTimeUtility.GetDateTimeString(time, DateTimeFormatType.MONTH_COMPLETE) + ".txt";
                    break;

                case LogType.YEARLY:
                    fileName = DateTimeUtility.GetDateTimeString(time, DateTimeFormatType.YEAR) + ".txt";
                    break;

                case LogType.ALL_ONE:
                default:
                    fileName = "log.txt";
                    break;
            }

            return Path.Combine(this.dirPath, fileName);
        }

        #endregion
    }
}
