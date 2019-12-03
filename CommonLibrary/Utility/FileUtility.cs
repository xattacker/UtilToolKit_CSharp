using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Xattacker.Utility
{
    /// <summary>
    /// provider some file releated helpful function
    /// </summary>
    public class FileUtility
    {
        #region dll import

        [DllImport("coredll.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetDiskFreeSpaceEx
            (
            string lpDirectoryName,
            out ulong lpFreeBytesAvailable,
            out ulong lpTotalNumberOfBytes,
            out ulong lpTotalNumberOfFreeBytes
            );

        #endregion


        /// <summary>
        /// to hide the constructor
        /// </summary>
        protected FileUtility()
        { 
        }

        /// <summary>
        ///  get disk avaiable storage size 
        /// </summary>
        /// <param name="dirName"> driectory name </param>
        /// <returns> dir avaiable size(bytes) </returns>
        public static ulong GetAvaiableStorageSize(string dirName)
        {
            ulong avail;
            ulong total;
            ulong free;

            GetDiskFreeSpaceEx(dirName, out avail, out total, out free);

            return avail;
        }

        /// <summary>
        ///  check the file path is duplicated or not.
        ///  if not, return the original path
        /// </summary>
        /// <param name="filePath"> file path </param>
        /// <returns> checked file path </returns>
        public static string CheckFileDuplicate(string filePath)
        {
            string file_path = filePath;

            if (File.Exists(file_path))
            {
                int dot_index = filePath.LastIndexOf(".");

                string file_sub = null;
                string file_type = null;

                if (dot_index != -1)
                {
                    file_sub = filePath.Substring(0, dot_index);
                    file_type = filePath.Substring(dot_index);
                }
                else
                {
                    // do not have file type
                    file_sub = filePath;
                    file_type = string.Empty;
                }

                int index = 1;

                do
                {
                    file_path = file_sub + "(" + index + ")" + file_type;
                    // max count is 99
                }
                while (File.Exists(file_path) && ++index <= 99);
            }

            return file_path;
        }

        /// <summary>
        ///  get the runtime app path.
        /// </summary>
        /// <returns> execute file paht </returns>
        public static string GetAppPath()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

            // some IO classes does not support URI format (like StreamWriter)
            if (path.ToLower().StartsWith("file:\\"))
            {
                path = path.Substring(6, path.Length - 6);
            }

            return path;
        }

        /// <summary>
        ///  check the file is existed or not.
        /// </summary>
        /// <param name="filePath"> file path </param>
        /// <returns> true = existed, otherwise is not </returns>
        public static bool IsFileExisted(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            return File.Exists(filePath);
        }

        /// <summary>
        ///  check the directory is existed or not.
        /// </summary>
        /// <param name="filePath"> dir path </param>
        /// <returns> true = existed, otherwise is not </returns>
        public static bool IsDirExisted(string dirPath)
        {
            if (string.IsNullOrEmpty(dirPath))
            {
                return false;
            }

            return Directory.Exists(dirPath);
        }

        /// <summary>
        ///  get a format file size string 
        /// </summary>
        /// <param name="bytesSize"> file size </param>
        /// <returns>  format string </returns>
        public static string GetFileSizeStr(long bytesSize)
        {
            double MB_SIZE = 1024 * 1024;
            double KB_SIZE = 1024;
            string str = null;

            if (bytesSize >= MB_SIZE)
            {
                double size = bytesSize / MB_SIZE;

                str = string.Format("{0:0.##} mb", size);
            }
            else if (bytesSize >= KB_SIZE)
            {
                double size = bytesSize / KB_SIZE;

                str = string.Format("{0:0.##} kb", size);
            }
            else
            {
                str = string.Format("{0:0} bytes", bytesSize);
            }

            return str;
        }

        public static string GetFileSizeStr(string filePath)
        {
            string str = null;

            if (IsFileExisted(filePath))
            {
                str = GetFileSizeStr(new FileInfo(filePath).Length);
            }

            return str;
        }

        /// <summary>
        ///  delete files from path completely, include sub paths
        /// </summary>
        /// <param name="path"> destination folder </param>
        public static void DeleteDirectoryCompletely(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string s in files)
            {
                File.Delete(s);
            }

            string[] dirs = Directory.GetDirectories(path);
            foreach (string d in dirs)
            {
                DeleteDirectoryCompletely(d);
            }

            Directory.Delete(path);
        }

        /// <summary>
        ///  copy files from one folder to another folder completely
        /// </summary>
        /// <param name="sourcePath"> source folder </param>
        /// <param name="destinationPath"> destination folder </param>
        public static void CopyFiles(string sourcePath, string destinationPath)
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            string[] files = Directory.GetFiles(sourcePath);
            foreach (string s in files)
            {
                // Use static Path methods to extract only the file name from the path.
                string fileName = Path.GetFileName(s);
                string destFile = Path.Combine(destinationPath, fileName);
                File.Copy(s, destFile, true);
            }

            string[] dirs = Directory.GetDirectories(sourcePath);
            foreach (string d in dirs)
            {
                string d_name = Path.GetFileName(d);
                string destD = Path.Combine(destinationPath, d_name);
                CopyFiles(d, destD);
            }
        }
    }


    // FileUtility extension function
    public static class FileUtilityExtension
    {
        public static bool IsFileExisted(this string str)
        {
            return FileUtility.IsFileExisted(str);
        }

        public static bool IsDirExisted(this string str)
        {
            return FileUtility.IsDirExisted(str);
        }

        public static string GetFileSizeStr(this string str)
        {
            return FileUtility.GetFileSizeStr(str);
        }

        public static string GetFileSizeStr(this FileInfo file)
        {
            return FileUtility.GetFileSizeStr(file.FullName);
        }

        public static void DeleteCompletely(this DirectoryInfo dir)
        {
            FileUtility.DeleteDirectoryCompletely(dir.FullName);
        }

        public static void CopyFiles(this DirectoryInfo dir, string destinationPath)
        {
            FileUtility.CopyFiles(dir.FullName, destinationPath);
        }

        public static void CopyFiles(this DirectoryInfo dir, DirectoryInfo destinationDir)
        {
            FileUtility.CopyFiles(dir.FullName, destinationDir.FullName);
        }
    }
}
