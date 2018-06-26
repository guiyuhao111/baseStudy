using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;

namespace DSMTMALL.Core.Common
{
    public class FileHelper
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 获取服务器路径
        /// </summary>
        /// <param name="pathName">路径名称</param>
        /// <returns>string</returns>
        public static string GetServerPath(string pathName)
        {
            return HttpContext.Current.Server.MapPath(pathName);
        }
        /// <summary>
        /// 判断文件扩展名
        /// </summary>
        /// <param name="pathName">文件路径</param>
        /// <param name="extensionName">文件扩展名</param>
        /// <returns>bool</returns>
        public static bool IsExtension(string pathName, string extensionName)
        {
            string ExtensionName = Path.GetExtension(pathName.ToLower());
            string[] tempArr = extensionName.Trim().ToLower().Split(',');
            for (int i = 0; i < tempArr.Length; i++)
            {
                if (ExtensionName == tempArr[i].ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断文件扩展名和大小
        /// </summary>
        /// <param name="pathName">文件路径</param>
        /// <param name="extensionName">文件扩展名</param>
        /// <param name="fileSize">文件大小</param>
        /// <param name="maxSize">文件最大限制</param>
        /// <returns>bool</returns>
        public static bool IsExtensionAndSize(string pathName, string extensionName, int fileSize, int maxSize)
        {
            if (IsExtension(pathName, extensionName) && fileSize <= maxSize)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 保存上传文件
        /// </summary>
        /// <param name="httpFileCollection">上传文件</param>
        /// <param name="index">文件索引</param>
        /// <param name="filePath">保存路径</param>
        /// <returns>bool</returns>
        public static bool SaveFile(HttpFileCollection httpFileCollection, int index, string filePath)
        {
            if (httpFileCollection != null && httpFileCollection.Count > 0)
            {
                try
                {
                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    }
                    httpFileCollection[index].SaveAs(filePath);
                    return true;
                }
                catch { }
            }
            return false;
        }
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="byteArr">字节数组</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>bool</returns>
        public static bool SaveImage(byte[] byteArr, string filePath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            try
            {
                using (MemoryStream stream = new MemoryStream(byteArr))
                {
                    using (Bitmap bitmap = new Bitmap(stream))
                    {
                        bitmap.Save(filePath);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>bool</returns>
        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 删除整个文件夹
        /// </summary>
        /// <param name="directoryPath">文件夹路径</param>
        /// <returns>bool</returns>
        public static bool DeleteDirectory(string directoryPath)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                if (directoryInfo.Exists)
                {
                    directoryInfo.Delete(true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        
    }
}