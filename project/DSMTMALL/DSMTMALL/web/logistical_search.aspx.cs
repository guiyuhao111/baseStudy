using DSMTMALL.Core.Common;
using DSMTMALL.MyPublic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;

namespace DSMTMALL.web
{
    public partial class logistical_search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (WebLoginHelper.IsLogin())
            {
                try
                {
                    string name =Server.UrlDecode(Request.QueryString["name"].Trim());
                    string number = Request.QueryString["number"].Trim();
                    string typeCom = string.Empty;
                    string tempKey = string.Empty;
                    object tempValue = string.Empty;
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(number))
                    {
                        switch (name)
                        {
                            case "德邦物流":
                                typeCom = "debangwuliu";
                                break;
                            case "申通快递":
                                typeCom = "shentong";
                                break;
                            case "汇通快递":
                                typeCom = "huitongkuaidi";
                                break;
                            case "佳吉物流":
                                typeCom = "jiajiwuliu";
                                break;
                            case "天地华宇":
                                typeCom = "tiandihuayu";
                                break;
                            case "天天快递":
                                typeCom = "tiantian";
                                break;
                            case "圆通快递":
                                typeCom = "yuantong";
                                break;
                            case "韵达快递":
                                typeCom = "yunda";
                                break;
                            case "宅急送":
                                typeCom = "zhaijisong";
                                break;
                            case "中铁快运":
                                typeCom = "zhongtiewuliu";
                                break;
                            case "中通快递":
                                typeCom = "zhongtong";
                                break;
                            case "顺丰快递":
                                typeCom = "shunfeng";
                                break;
                            default:
                                typeCom = string.Empty;
                                break;
                        }
                        //string url_1 = string.Format("https://m.kuaidi100.com/autonumber/auto?num={0}",number); 根据订单号获取快递名称英文简称

                        if (!string.IsNullOrEmpty(typeCom))//如果不为空的情况下调用接口获取json数据
                        {
                            string url = string.Format("https://m.kuaidi100.com/query?type={0}&postid={1}", typeCom, number);
                            string resInfo = ToolHelper.Post(url, string.Empty);
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            StringBuilder liHtml = new StringBuilder();
                            if(!string.IsNullOrEmpty(resInfo))
                            {
                                Dictionary<string, dynamic> dic = serializer.Deserialize<Dictionary<string, dynamic>>(resInfo);
                                if (dic.TryGetValue("message", out tempValue))
                                {
                                    if (Convert.ToString(tempValue) == "ok" && dic.TryGetValue("data", out tempValue))
                                    {
                                        if (tempValue!=null)
                                        {
                                            tempValue=  serializer.Serialize(tempValue);
                                            LogisticalInfo[] logisticalInfo = serializer.Deserialize<LogisticalInfo[]>(Convert.ToString(tempValue));
                                            if (logisticalInfo != null && logisticalInfo.Length > 0)
                                            {
                                                foreach (var item in logisticalInfo)
                                                {
                                                    liHtml.Append("<li><span>" + item.time + "</span>");
                                                    liHtml.Append("<strong>" + item.context + "</strong></li>");
                                                }
                                            }
                                        }
                                    }
                                }
                                showLogistical.InnerHtml = liHtml.ToString();
                            }
                        }
                    }
                }
                catch
                {
                    
                }
            }
        }
    }

    public class LogisticalInfo
    {
        public string time { get; set; }
        public string context { get; set; }
    }
}