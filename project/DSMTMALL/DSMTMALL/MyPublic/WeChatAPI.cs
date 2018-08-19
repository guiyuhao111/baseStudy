using DSMTMALL.Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace DSMTMALL.MyPublic
{
    public class WeChatAPI
    {
        private const string appid = "wxe903cd14a12c720d";//正式
        private const string secret = "5727a3e20151ed97b60d69ad931ca767";//正式

        /// <summary>
        /// 数组排序（冒泡排序法）
        /// </summary>
        /// <param name="originalArray">待排序字符串数组</param>
        /// <returns> 经过冒泡排序过的字符串数组</returns>
        private static String[] BubbleSort(String[] originalArray)
        {
            int i, j; //交换标志
            String temp;
            Boolean exchange;

            for (i = 0; i < originalArray.Length; i++)
            {//最多做R.Length-1趟排序
                exchange = false; //本趟排序开始前，交换标志应为假
                for (j = originalArray.Length - 2; j >= i; j--)
                {
                    if (originalArray[j + 1].CompareTo(originalArray[j]) < 0)
                    {//交换条件
                        temp = originalArray[j + 1];
                        originalArray[j + 1] = originalArray[j];
                        originalArray[j] = temp;
                        exchange = true; //发生了交换，故将交换标志置为真
                    }
                }
                if (!exchange)
                {//本趟排序未发生交换，提前终止算法
                    break;
                }
            }
            return originalArray;
        }

        /// <summary>
        /// 发送消息给微信公众平台接口
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        private static string SignMsg(string title, string content, string openID)
        {
            string param = "description=" + content + "&openId=" + openID + "&title=" + title;
            string[] msg = { "description" + HttpUtility.UrlEncode(content, Encoding.UTF8), "openId" + HttpUtility.UrlEncode(openID, Encoding.UTF8), "title" + HttpUtility.UrlEncode(title, Encoding.UTF8), KeyHelper.wechatMD5Key };
            string[] msg_sort = BubbleSort(msg);
            string postMsg = string.Empty;
            foreach (var item in msg_sort)
            {
                postMsg += item;
            }
            string sign = new MD5Helper(string.Empty).Encrypt(postMsg);
            param = param + "&sign=" + sign;
            try
            {
                return ToolHelper.Post("http://wechat.51ipc.com/sendCustomMsg", param);
            }
            catch(Exception e) { FileHelper.logger.Debug(e.ToString()); return string.Empty; }
        }

        /// <summary>
        /// 发送提示消息给微信公众号接口
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="openID"></param>
        public static void SendMsg(string title, string content, string openID)
        {
            try
            {
                if (!string.IsNullOrEmpty(openID))
                {
                    string resMsg = SignMsg(title, content, openID);
                    if (!string.IsNullOrEmpty(resMsg))
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(resMsg);
                        if (dic != null && dic.ContainsKey("errcode"))
                        {
                            if (dic["errcode"] == "40001" || dic["errcode"] == "42001")
                            {
                                SignMsg(title, content, openID);
                            }
                        }
                    }
                }
            }catch(Exception e) { FileHelper.logger.Debug(e.ToString()); }
        }

        /// <summary>
        /// 获取微信公众号的OPENID与Access_token
        /// </summary>
        /// <param name="code"></param>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetWeChatAPI(string code)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, secret,code);
            try
            {
                string resJson = ToolHelper.Post(url, string.Empty);
                Dictionary<string, string> respDic = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(resJson);
                return respDic;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取微信用户的头像
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static string GetWeChatAPIUserImg(string access_token,string openid )
        {
            string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN ", access_token, openid);
            try
            {
                string resJson = ToolHelper.Post(url, string.Empty);
                Dictionary<string, object> respDic = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(resJson);
                object headimgUrl = string.Empty;
                respDic.TryGetValue("headimgurl", out headimgUrl);
                return Convert.ToString(headimgUrl); 
            }
            catch
            {
                return string.Empty;
            }
        }
    }

}