using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DSMTMALL.Core.Common
{
    public class ToolHelper
    {
        public const string ProductCode = "D4CB0B49-EFC6-4279-B752-34F2F71FEB1A";
        /// <summary>
        /// 执行cmd命令
        /// </summary>
        /// <param name="strCmd">cmd命令</param>
        public static void RunCmd(string strCmd)
        {
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = false;
            proc.Start();
            proc.StandardInput.WriteLine(strCmd);
            proc.Close();
        }
        /// <summary>
        /// 写入txt文件
        /// </summary>
        /// <param name="txtFile">txt文件路径</param>
        /// <param name="txtContent">txt文件内容</param>
        /// <param name="isCovered">是否覆盖原文件</param>
        public static void WriteTxt(string txtFile, string txtContent, bool isCovered)
        {
            if (!Directory.Exists(Path.GetDirectoryName(txtFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(txtFile));
            }
            if (isCovered)
            {
                File.WriteAllText(txtFile, txtContent);
            }
            else
            {
                if (!File.Exists(txtFile))
                {
                    File.WriteAllText(txtFile, txtContent);
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(txtFile))
                    {
                        sw.WriteLine(txtContent);
                    }
                }
            }
        }
        /// <summary>
        /// 安装msi操作
        /// </summary>
        /// <param name="msiFile">msi文件路径</param>
        /// <param name="serviceName">服务名称</param>
        public static void InstallMsi(string msiFile, string serviceName)
        {
            string arg = String.Format("/q /{0} \"{1}\"", false ? "x" : "i", msiFile);
            ProcessStartInfo psi = new ProcessStartInfo("msiexec.exe", arg);
            psi.UseShellExecute = false;
            Process process = new Process();
            process.StartInfo = psi;
            process.Start();
            process.WaitForExit();
            if (!string.IsNullOrEmpty(serviceName) && process.ExitCode == 0)
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                    }
                }
            }
            process.Close();
        }
        /// <summary>
        /// 卸载msi操作
        /// </summary>
        /// <param name="softProductCode">卸载版本ProductCode</param>
        public static void UninstallMsi(string softProductCode)
        {
            string arg = "/x {" + softProductCode + "} /qn";
            ProcessStartInfo psi = new ProcessStartInfo("msiexec.exe", arg);
            psi.UseShellExecute = false;
            Process process = new Process();
            process.StartInfo = psi;
            process.Start();
            process.WaitForExit();
            process.Close();
        }
        /// <summary>
        /// 升级msi操作
        /// </summary>
        /// <param name="msiFile">msi文件路径</param>
        /// <param name="serviceName">服务名称</param>
        /// <param name="productCode">上个版本ProductCode</param>
        public static void UpdateMsi(string msiFile, string serviceName, string productCode)
        {
            string arg = "/x {" + productCode + "} /qn";
            ProcessStartInfo psi = new ProcessStartInfo("msiexec.exe", arg);
            psi.UseShellExecute = false;
            Process process = new Process();
            process.StartInfo = psi;
            process.Start();
            process.WaitForExit();
            arg = String.Format("/q /{0} \"{1}\"", false ? "x" : "i", msiFile);
            psi = new ProcessStartInfo("msiexec.exe", arg);
            psi.UseShellExecute = false;
            process.StartInfo = psi;
            process.Start();
            process.WaitForExit();
            if (!string.IsNullOrEmpty(serviceName) && process.ExitCode == 0)
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                    }
                }
            }
            process.Close();
        }
        /// <summary>
        /// 一级数组去重复
        /// </summary>
        /// <param name="myArr">原始数组</param>
        /// <returns>去重复后数组</returns>
        public static string[] DelRepeatData(string[] myArr)
        {
            List<string> myList = new List<string>();
            for (int i = 0; i < myArr.Length; i++)
            {
                if (myList.IndexOf(myArr[i]) == -1)
                {
                    myList.Add(myArr[i]);
                }
            }
            return myList.ToArray();
        }
        /// <summary>
        /// 二级数组去重复
        /// </summary>
        /// <param name="myArr">原始数组</param>
        /// <param name="mySign">分隔符</param>
        /// <returns>去重复后数组</returns>
        public static string[] DelRepeatData(string[] myArr, char mySign)
        {
            List<string> myList = new List<string>();
            string[] tempArr = null;
            for (int i = 0; i < myArr.Length; i++)
            {
                int myCount = 0;
                tempArr = myArr[i].Split(mySign);
                for (int j = 0; j < myList.Count; j++)
                {
                    if (myList[j].Split(mySign)[0] == tempArr[0])
                    {
                        myCount += 1;
                    }
                }
                if (myCount == 0)
                {
                    myList.Add(myArr[i]);
                }
            }
            return myList.ToArray();
        }
        /// <summary>
        /// Url参数编码
        /// </summary>
        /// <param name="strParameter">待编码字符串</param>
        /// <param name="strIsNull">待编码字符串为空时返回值</param>
        /// <returns>编码后字符串</returns>
        public static string UrlParEncode(string strParameter, string strIsNull)
        {
            if (!string.IsNullOrEmpty(strParameter))
            {
                return string.IsNullOrEmpty(strParameter.Trim()) ? strIsNull : HttpUtility.UrlEncode(strParameter.Trim());
            }
            return strIsNull;
        }
        /// <summary>
        /// Url参数解码
        /// </summary>
        /// <param name="strParameter">待解码字符串</param>
        /// <param name="strIsNull">待解码字符串为空时返回值</param>
        /// <returns>解码后字符串</returns>
        public static string UrlParDecode(string strParameter, string strIsNull)
        {
            if (!string.IsNullOrEmpty(strParameter))
            {
                return string.IsNullOrEmpty(strParameter.Trim()) ? strIsNull : HttpUtility.UrlDecode(strParameter.Trim());
            }
            return strIsNull;
        }
        /// <summary>
        /// 获取Post或Get参数
        /// </summary>
        /// <param name="strParameter">待获取字符串</param>
        /// <param name="strIsNull">待获取字符串为空时返回值</param>
        /// <returns>获取字符串</returns>
        public static string GetPostOrGetPar(string strParameter, string strIsNull)
        {
            if (!string.IsNullOrEmpty(strParameter))
            {
                return string.IsNullOrEmpty(strParameter.Trim()) ? strIsNull : strParameter.Trim();
            }
            return strIsNull;
        }

        /// <summary>
        /// 用自定义符号对字符串连接
        /// </summary>
        /// <param name="myStr">待连接字符串</param>
        /// <param name="addStr">需连接字符串</param>
        /// <param name="mySign">分隔符</param>
        /// <returns>连接后字符串</returns>
        public static string StringAdd(string myStr, string addStr, string mySign)
        {
            if (string.IsNullOrEmpty(myStr))
            {
                myStr = addStr;
            }
            else
            {
                myStr += mySign + addStr;
            }
            return myStr;
        }

        /// <summary>
        /// 字符串转字典集合
        /// </summary>
        /// <param name="myStr">原字符串</param>
        /// <param name="mySign1">一级分隔符</param>
        /// <param name="mySign2">二级分隔符</param>
        /// <returns>字典集合</returns>
        public static Dictionary<string, string> StringToDic(string myStr, char mySign1, char mySign2)
        {
            Dictionary<string, string> myDic = new Dictionary<string, string>();
            string[] myArr = myStr.Split(mySign1);
            string[] tempArr = null;
            for (int i = 0; i < myArr.Length; i++)
            {
                tempArr = myArr[i].Split(mySign2);
                if (!myDic.ContainsKey(tempArr[0]))
                {
                    myDic.Add(tempArr[0], tempArr[1]);
                }
            }
            return myDic;
        }

        /// <summary>
        /// 替换查询参数
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="dicQueryPar">参数字典集合</param>
        /// <param name="parCount">剩余参数个数</param>
        /// <returns>替换后查询语句</returns>
        public static string GetReportSQL(string strSql, Dictionary<string, string> dicQueryPar, out int parCount)
        {
            string queryPar = "";
            int myCount = 0;
            MatchCollection matchCollection = new Regex(@"@\w+").Matches(strSql);
            for (int i = 0; i < matchCollection.Count; i++)
            {
                if (dicQueryPar.TryGetValue(matchCollection[i].Value.Replace("@", ""), out queryPar))
                {
                    strSql = strSql.Replace(matchCollection[i].Value, queryPar);
                    myCount++;
                }
            }
            parCount = matchCollection.Count - myCount;
            return strSql;
        }

        /// <summary>
        /// 获取查询参数
        /// </summary>
        /// <param name="dicQueryPar">参数字典集合</param>
        /// <returns>MySqlParameter[]</returns>
        public static MySqlParameter[] GetMySqlParameter(Dictionary<string, object> dicQueryPar)
        {
            MySqlParameter mySqlParameter = null;
            MySqlParameter[] mySqlParameterArr = new MySqlParameter[dicQueryPar.Count];
            if (dicQueryPar.Count > 0)
            {
                int myCount = 0;
                foreach (var queryPar in dicQueryPar)
                {
                    mySqlParameter = new MySqlParameter(queryPar.Key, queryPar.Value);
                    mySqlParameterArr[myCount] = mySqlParameter;
                    myCount++;
                }
            }
            return mySqlParameterArr;
        }

        /// <summary>
        /// 将一个IEnumerable<dynamic>类型的转化为字典
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="iEnumerable">委托</param>
        /// <returns></returns>
        public static Dictionary<string, string> IEnumerableListToDic(IEnumerable<dynamic> dt,Action<IEnumerable<dynamic>, Dictionary<string, string>> iEnumerable)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            iEnumerable(dt,dic);
            return dic;
        }

        /// <summary>
        /// DataTable转为Dictionary
        /// </summary>
        /// <param name="dt">待转换DataTable</param>
        /// <param name="id">id列名称</param>
        /// <param name="name">name列名称</param>
        /// <returns>转换后Dictionary</returns>
        public static Dictionary<string, string> DataTableToDic(DataTable dt, string id, string name)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!dic.ContainsKey(dt.Rows[i][id].ToString()))
                    {
                        dic.Add(dt.Rows[i][id].ToString(), dt.Rows[i][name].ToString());
                    }
                }
            }
            return dic;
        }

        /// <summary>
        /// DataTable转为Dictionary
        /// </summary>
        /// <param name="dt">待转换DataTable</param>
        /// <param name="id">id列名称</param>
        /// <param name="name">编号列名称</param>
        /// <param name="name">name列名称</param>
        /// <returns>转换后Dictionary</returns>
        public static Dictionary<string, string> DataTableToDic(DataTable dt, string id, string number, string name)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!dic.ContainsKey(dt.Rows[i][id].ToString()))
                    {
                        dic.Add(dt.Rows[i][id].ToString(), dt.Rows[i][number].ToString() + dt.Rows[i][name].ToString());
                    }
                }
            }
            return dic;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">要发送的邮箱</param>
        /// <param name="mailSubject">邮箱系列</param>
        /// <param name="mailContent">邮箱内容</param>
        /// <returns>返回发送邮箱的结果</returns>
        public static bool SendEmail(string mailTo, string mailSubject, string mailContent)
        {
            //设置发送方的邮件信息,例如使用网易的smtp
            string smtpServer = "smtp.mxhichina.com"; //SMTP服务器
            string mailFrom = "d.xu@chicun.design"; //登陆用户名
            string userPassword = "Xdx199112";//登陆密码
            //邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer; //指定SMTP服务器
            smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码
            //发送邮件设置
            MailMessage mailMessage = new MailMessage(mailFrom, mailTo);//发送人和收件人
            mailMessage.Subject = mailSubject;//系列
            mailMessage.Body = mailContent;//内容
            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = MailPriority.Low;//优先级
            try
            {
                smtpClient.Send(mailMessage);//发送邮件
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取图片长宽
        /// </summary>
        /// <param name="imgPath">图片路径</param>
        /// <param name="imgWidth">图片宽度</param>
        /// <param name="imgHeight">图片长度</param>
        public static void GetImageSize(string imgPath, out int imgWidth, out int imgHeight)
        {
            imgWidth = 0;
            imgHeight = 0;
            if (File.Exists(imgPath))
            {
                using (Image image = Image.FromFile(imgPath))
                {
                    imgWidth = image.Width;
                    imgHeight = image.Height;
                }
            }
        }

        /// <summary>
        /// 接口数据的POST提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Post(string url, string param)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] payload = Encoding.UTF8.GetBytes(param);
            request.ContentLength = payload.Length;
            try
            {
                using (Stream writer = request.GetRequestStream())
                {
                    writer.Write(payload, 0, payload.Length);
                    writer.Close();
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream s = response.GetResponseStream())
                        {
                            string StrDate = "";
                            string strValue = "";
                            using (StreamReader Reader = new StreamReader(s, Encoding.UTF8))
                            {
                                while ((StrDate = Reader.ReadLine()) != null)
                                {
                                    strValue += StrDate + "\r\n";
                                }
                                return strValue;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string s = e.ToString();
                return s;
            }
        }

        /// <summary>
        /// 验证身份证号码是否有效
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public static bool IsCardNoIsPass(string cardNo)
        {
            string reg = @"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";
            Regex tmpreg = new Regex(reg);
            return tmpreg.IsMatch(cardNo);
        }

        /// <summary>
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址
        /// </summary>
        public static string IPAddress()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (result != null && result != string.Empty)
            {
                //可能有代理
                if (result.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式
                { result = null; }
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。
                        result = result.Replace(" ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (MetarnetRegex.IsIPAddress(temparyip[i]) && temparyip[i].Substring(0, 3) != "10." && temparyip[i].Substring(0, 7) != "192.168" && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i];    //找到不是内网的地址
                            }
                        }
                    }
                    else if (MetarnetRegex.IsIPAddress(result)) //代理即是IP格式 ,IsIPAddress判断是否是IP的方法,
                    { return result; }
                    else { result = null; }   //代理中的内容 非IP，取IP
                }
            }
            string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (null == result || result == string.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (result == null || result == string.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }

        #region 工具箱

        /// <summary>
        /// 将字符串转化为整形
        /// </summary>
        /// <param name="inVal"></param>
        /// <returns></returns>
        public static int ConventToInt32(string inVal)
        {
            int outInval = 0;
            int.TryParse(inVal, out outInval);
            return outInval;
        }

        /// <summary>
        /// 将字符串转化为金额
        /// </summary>
        /// <param name="inval"></param>
        /// <returns></returns>
        public static decimal ConventToDecimal(string inval)
        {
            decimal outInval = 0;
            decimal.TryParse(inval, out outInval);
            return outInval;
        }

        /// <summary>
        /// 将字符串转化为整形，如果转换失败，返回传入的默认值
        /// </summary>
        /// <param name="inVal"></param>
        /// <param name="outVal"></param>
        /// <returns></returns>
        public static int ConventToInt32(string inVal, int outVal)
        {
            int outInval = 0;
            if (!int.TryParse(inVal, out outInval))
            {
                outInval = outVal;
            }
            return outInval;
        }

        /// <summary>
        /// 将字符串转化为整形，如果转换失败，返回传入的默认值
        /// </summary>
        /// <param name="inVal"></param>
        /// <param name="outVal"></param>
        /// <returns></returns>
        public static double ConventToDouble(string inVal, double outVal)
        {
            double outInval = 0;
            if (!double.TryParse(inVal, out outInval))
            {
                outInval = outVal;
            }
            return outInval;
        }

        /// <summary>
        /// 将字符串转化为金额
        /// </summary>
        /// <param name="inval"></param>
        /// <returns></returns>
        public static decimal ConventToDecimal(string inval, int outVal)
        {
            decimal outInval = 0;
            if (!decimal.TryParse(inval, out outInval))
            {
                outInval = outVal;
            }
            return outInval;
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
        public DateTime UnixTimestampToDateTime(DateTime target, long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, 0, target.Kind);
            return start.AddMilliseconds(timestamp);
        }

        /// <summary>
        /// 统计俩个时间点时间差
        /// </summary>
        /// <param name="firstTime">大的时间点</param>
        /// <param name="secondTime">小的时间点</param>
        /// <returns></returns>
        public static long TimeQuantum(DateTime firstTime, DateTime secondTime)
        {
            return DateTimeToUnixTimestamp(firstTime) - DateTimeToUnixTimestamp(secondTime);
        }

        /// <summary>
        /// 在指定的文件中写入指定的内容//web网页服务器写本地日志文件
        /// </summary>
        /// <param name="textInfo"></param>
        public static void WriteLogInfoToLocalText(string textInfo)
        {
            string path = HttpContext.Current.Server.MapPath("/logfile/") + DateTime.Now.ToString("yyyy/MM/dd") + "errLog.txt";
            WriteTxt(path, DateTime.Now + ":" + textInfo, false);//在指定的文件中写入指定的内容
        }

        /// <summary>
        /// 将字符串转化为时间
        /// </summary>
        /// <param name="inVal"></param>
        /// <param name="outVal"></param>
        /// <returns></returns>
        public static DateTime ConventToDateTime(string inVal, DateTime outVal)
        {
            DateTime outInval = DateTime.Now;
            if (!DateTime.TryParse(inVal, out outInval))
            {
                outInval = outVal;
            }
            return outInval;
        }

        /// <summary>
        /// 获取当月第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime MonthFirstDay()
        {
            DateTime now = DateTime.Now;
            return new DateTime(now.Year, now.Month, 1);
        }

        /// <summary>
        /// 获取当月最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime MonthLastDay()
        {
            DateTime now = DateTime.Now;
            DateTime dt1 = new DateTime(now.Year, now.Month, 1);
            return Convert.ToDateTime(dt1.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
        }
        #endregion

    }
}