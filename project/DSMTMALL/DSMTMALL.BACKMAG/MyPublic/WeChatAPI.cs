using DSMTMALL.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace DSMTMALL.BACKMAG.MyPublic
{
    public class WeChatAPI
    {
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
            catch { return string.Empty; }
        }

        /// <summary>
        /// 发送提示消息给微信公众号接口
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="openID"></param>
        public static void SendMsg(string title, string content, string openID)
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
        }
    }
}