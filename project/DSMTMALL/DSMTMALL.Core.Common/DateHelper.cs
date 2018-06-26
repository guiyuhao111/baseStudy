using System;

namespace DSMTMALL.Core.Common
{
    public class DateHelper
    {
        /// <summary>
        /// 获取某月的第一天开始时间
        /// </summary>
        /// <param name="datetime">要获取的时间</param>
        /// <returns>DateTime</returns>
        public static DateTime GetFirstDayOfMonth(DateTime datetime)
        {
            return GetFirstDayNowOfMonth(Convert.ToDateTime(datetime.ToString("yyyy-MM-dd 00:00:00")));
        }
        /// <summary>
        /// 获取某月的最后一天结束时间
        /// </summary>
        /// <param name="datetime">要获取的时间</param>
        /// <returns>DateTime</returns>
        public static DateTime GetLastDayOfMonth(DateTime datetime)
        {
            return GetLastDayNowOfMonth(Convert.ToDateTime(datetime.ToString("yyyy-MM-dd 23:59:59")));
        }
        /// <summary>
        /// 获取某月的第一天当前时间
        /// </summary>
        /// <param name="datetime">要获取的时间</param>
        /// <returns>DateTime</returns>
        public static DateTime GetFirstDayNowOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }
        /// <summary>
        /// 获取某月的最后一天当前时间
        /// </summary>
        /// <param name="datetime">要获取的时间</param>
        /// <returns>DateTime</returns>
        public static DateTime GetLastDayNowOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }
        /// <summary>
        /// 获取上个月的第一天开始时间
        /// </summary>
        /// <param name="datetime">要获取的时间</param>
        /// <returns>DateTime</returns>
        public static DateTime GetFirstDayOfPreviousMonth(DateTime datetime)
        {
            return GetFirstDayNowOfPreviousMonth(Convert.ToDateTime(datetime.ToString("yyyy-MM-dd 00:00:00")));
        }
        /// <summary>
        /// 获取上个月的最后一天结束时间
        /// </summary>
        /// <param name="datetime">要获取的时间</param>
        /// <returns>DateTime</returns>
        public static DateTime GetLastDayOfPrdviousMonth(DateTime datetime)
        {
            return GetLastDayNowOfPrdviousMonth(Convert.ToDateTime(datetime.ToString("yyyy-MM-dd 23:59:59")));
        }
        /// <summary>
        /// 获取上个月的第一天当前时间
        /// </summary>
        /// <param name="datetime">要获取的时间</param>
        /// <returns>DateTime</returns>
        public static DateTime GetFirstDayNowOfPreviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(-1);
        }
        /// <summary>
        /// 获取上个月的最后一天当前时间
        /// </summary>
        /// <param name="datetime">要获取的时间</param>
        /// <returns>DateTime</returns>
        public static DateTime GetLastDayNowOfPrdviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddDays(-1);
        }

        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            long intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = Convert.ToInt64((dateTime - startTime).TotalMilliseconds);
            return intResult;
        }

        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return start.AddMilliseconds(timestamp);
        }

    }
}