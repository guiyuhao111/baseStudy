using System.IO;
using System.Net;
using System.Text;

namespace DSMTMALL.Core.Common
{
    public class SendSMS
    {
        public static bool GetHtmlFromUrl(string url, out string resStr)
        {

            resStr = string.Empty;
            string strRet = string.Empty;
            if (url != null && url.Trim().ToString() != "")
            {
                string targeturl = url.Trim().ToString();
                try
                {
                    HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                    hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                    hr.Method = "GET";
                    hr.Timeout = 30 * 60 * 1000;
                    using (WebResponse hs = hr.GetResponse())
                    {
                        using (Stream sr = hs.GetResponseStream())
                        {
                            using (StreamReader ser = new StreamReader(sr, Encoding.Default))
                            {
                                strRet = ser.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    strRet = "0";
                }
            }
            int intRes = 0;
            int.TryParse(strRet, out intRes);
            if (intRes > 0)
            {
                return true;
            }
            switch (strRet)
            {
                case "0":
                    resStr = "系统出错，短信没有被发送";
                    break;
                case "-1":
                    resStr = "没有该用户";
                    break;
                case "-21":
                    resStr = "MD5接口密钥加密不正确";
                    break;
                case "-3":
                    resStr = "短信数量不足";
                    break;
                case "-11":
                    resStr = "该账户被禁用";
                    break;
                case "-14":
                    resStr = "短信内容出现非法字符";
                    break;
                case "-41":
                    resStr = "手机号码为空";
                    break;
                case "-42":
                    resStr = "短信内容为空";
                    break;
                case "-51":
                    resStr = "短信签名格式不正确";
                    break;
                case "-6":
                    resStr = "IP限制";
                    break;
                default:
                    break;
            }
            return false;
        }

    }
}
