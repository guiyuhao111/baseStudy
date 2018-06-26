using DSMTMALL.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSMTMALL.BACKMAG.MyPublic
{
    public class WebLogHelper
    {
        /// <summary>
        /// 当系统catch到错误的时候写日志
        /// </summary>
        /// <param name="errInfo">错误信息</param>
        /// <param name="filePath">文件路径</param>
        public static void WriteErrLog(string errInfo ,string filePath)
        {
            if (string.IsNullOrEmpty(filePath))//如果路径为空，则使用默认的路径
            {
                filePath = "/files/error/system.txt";
            }
            ToolHelper.WriteTxt(filePath, errInfo, false);
        }
    }
}