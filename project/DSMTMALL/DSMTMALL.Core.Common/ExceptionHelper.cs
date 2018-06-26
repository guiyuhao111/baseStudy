using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSMTMALL.Core.Common
{
    public  class ExceptionHelper
    {
        /// <summary>
        /// 系统接口连接超时
        /// </summary>
        public const string Exception1000 = "1000";//系统接口连接超时
        /// <summary>
        /// 与XF系统支付错误
        /// </summary>
        public const string Exception1001 = "1001";//与XF系统支付错误
        /// <summary>
        /// 与XF系统对账错误
        /// </summary>
        public const string Exception1002 = "1002";//与XF系统对账错误
        /// <summary>
        /// 与WMS系统校对订单错误
        /// </summary>
        public const string Exception1003 = "1003";//与WMS系统校对订单错误
        /// <summary>
        /// 与数据库连接出错
        /// </summary>
        public const string Execption1009 = "1009";//数据库连接出错
        
    }

    public class ExceptionNotes
    {
        public const string SystemBusy = "系统忙,请稍后再试";

        public const string SystemSuccess = "SUCCESS";

        public const string SystemUnLogin= "UNLOGIN";
    }
}
