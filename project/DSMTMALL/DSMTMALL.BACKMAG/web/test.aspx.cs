using DSMTMALL.BACKMAG.MyPublic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEntity;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace DSMTMALL.BACKMAG.web
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 下载媒体文件
            // WebClient client = new WebClient();
            // client.DownloadFile("http://image.yoohobox.com/public/images/d9/c9/4f/1662c45edec02c43faf6179e373c2cd7f92a2b35.jpg", @"G:\DSMTMALL\DSMTMALL.BACKMAG\files\imgs\test.jpg");
            // string fff = "s";

            #endregion

            #region WMSADD

            ////基础参数配置
            //DateTime nowTime = DateTime.Now;
            //string appsecret = "2E94F7B2-2557-432C-85FC-FB12EDC2TEST";
            //string wmsNotifyAddStr = string.Empty;
            //string test = "test";
            //string method ="StockOut.Notify.Add";
            //string v = "v1.0";
            //string url = @"http://120.55.240.59:8082/OMSWeb/WebService/OrderService.asmx/Api";
            //string email = "756657790@qq.com";
            //WMSNotifyAdd wmsNotifyAdd = new WMSNotifyAdd("线下测试1号仓库");

            ////执行订单推送
            //SortedDictionary<string, string> dicQuery = new SortedDictionary<string, string>();
            //string strWhere = "SuppliersID=1 AND OrderStatus=1 AND PayStatus=2";//1.订单的商品来源要是海淘的，2.订单的状态要已确认，3.订单的支付状态是已付款的
            //List<DB.Model.M_OrderInfo> orderInfo = new DB.BLL.M_OrderInfo(DBEnum.Slave).GetModelList(strWhere);//获取所有要推送的订单列表
            //if (orderInfo != null || orderInfo.Count > 0)
            //{
            //    List<Payment> payMentList =null;
            //    Payment payMentInfo = null;
            //    Dictionary<string, string> dicOrderInfo = null;
            //    Dictionary<string, string> dicOrderGoods = null;
            //    AddContent addContent = null;
            //    List<AddRows> addRowsList = null;
            //    WMSRetrunInfo resJsonArr =null;
            //    OrderHelper wmsStringHandler = new OrderHelper();
            //    List<DB.Model.M_OrderGoods> orderGoodsList = null;
            //    string resWMSVal = string.Empty;
            //    string mailContent = string.Empty;
            //    string dicSignStr = string.Empty;
            //    string param = string.Empty;
            //    for (int i = 0; i < orderInfo.Count; i++)//推送当前列表第一条订单信息
            //    {
            //        dicQuery = new SortedDictionary<string, string>();//每一订单推送后都必须要这个键值对初始化
            //        payMentList = new List<Payment>();
            //        payMentInfo = new Payment();
            //        resJsonArr =new WMSRetrunInfo();
            //        resWMSVal = string.Empty;
            //        mailContent = string.Empty;
            //        param = string.Empty;
            //        nowTime = DateTime.Now;
            //        orderGoodsList = new DB.BLL.M_OrderGoods(DBEnum.Slave).GetModelList("OrderSn='" + orderInfo[i].OrderSn + "'");
            //        if (orderGoodsList != null && orderGoodsList.Count > 0)//订单的商品信息不为空
            //        {
            //            dicOrderInfo = new Dictionary<string, string>();
            //            dicOrderGoods = new Dictionary<string, string>();
            //            dicOrderInfo.Add("Created", "AddTime");//订单生成时间
            //            dicOrderInfo.Add("Modified", "UpdateTime");//最后修改时间
            //            dicOrderInfo.Add("PaymentTime", "PayTime");//订单付款时间
            //            dicOrderInfo.Add("OrderOuterId", "OrderUnifySn");//订单外部单号
            //            dicOrderInfo.Add("BuyerMessage", "Postscript");//顾客留言
            //            dicOrderInfo.Add("SellerComment", "Postscript");//卖家备注
            //            dicOrderInfo.Add("LogisticCompany", "ShippingName");//物流公司名称
            //            dicOrderInfo.Add("ReceiverName", "Consignee");//收货人姓名
            //            dicOrderInfo.Add("ReceiverProvince", "ProvinceName");//收货人所在省
            //            dicOrderInfo.Add("ReceiverCity", "CityName");//城市
            //            dicOrderInfo.Add("ReceiverDistrict", "DistrictName");//地区
            //            dicOrderInfo.Add("ReceiverAddress", "Address");//详细地址
            //            dicOrderInfo.Add("ReceiverPhone1", "Mobile");//手机号码
            //            dicOrderInfo.Add("BuyerCode", "Mobile");//客户编码用手机号代替
            //            dicOrderInfo.Add("BuyerTrueName", "Consignee");//买家身份证号
            //            dicOrderInfo.Add("BuyerIdCardNo", "ConsigneeCardNo");//买家真实姓名
            //            dicOrderInfo.Add("PayAmount", "OrderAmount");//总订单价格
            //            addContent = EntityHelper<AddContent>.EntityValueToEntity(orderInfo[i], dicOrderInfo);
            //            //商品信息--一笔订单会有多个商品信息

            //            dicOrderGoods.Add("OrderRowId", "OrderID");//订单行ID
            //            dicOrderGoods.Add("MerchId", "GoodsID");//第三方平台商品ID
            //            dicOrderGoods.Add("MerchCode", "GoodsSn");//商品编号对应海淘
            //            dicOrderGoods.Add("RowDesc", "GoodsAttr");//行描述
            //            dicOrderGoods.Add("Qty", "BuyNumber");//购买数量
            //            dicOrderGoods.Add("Price", "GoodsPrice");//购买价格
            //            addRowsList = EntityHelper<AddRows>.EntityListValueToEntityList(orderGoodsList, dicOrderGoods);
            //            addContent.Rows = addRowsList;
            //            payMentInfo.PaymentMethod = (orderInfo[i].PayID.ToString() == "10") ? "10" : "90";
            //            payMentInfo.Amount = orderInfo[i].OrderAmount.ToString();
            //            payMentList.Add(payMentInfo);
            //            addContent.PayMents = payMentList;
            //            wmsNotifyAdd.Content = addContent;
            //            //wmsNotifyAddStr = JsonHelper.EntityToJson(wmsNotifyAdd);//将该笔订单信息JSON格式化
            //            wmsNotifyAddStr = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(wmsNotifyAdd);//将该笔订单信息JSON格式化

            //            dicQuery.Add("appkey", test);
            //            dicQuery.Add("method", method);
            //            dicQuery.Add("param", wmsNotifyAddStr);
            //            dicQuery.Add("v", v);
            //            dicQuery.Add("timestamp", nowTime.ToString());

            //            dicSignStr = wmsStringHandler.WMSStringHanlder(dicQuery, appsecret);
            //            param = "appkey=" + test + "&timestamp=" + nowTime + "&v=" + v + "&method=" + method + "&param=" + wmsNotifyAddStr + "&sign=" + dicSignStr;
            //            try
            //            {
            //                resWMSVal = ToolHelper.Post(url, param);
            //            }
            //            catch
            //            {
            //                mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "推送订单至海淘出错，错误信息：与海淘的订单推送接口连接出错";
            //                SendEmail.SendEmails(email, "订单推送出错", mailContent);
            //            }
            //            resJsonArr = JsonHelper.JsonToEntity<WMSRetrunInfo>(resWMSVal);
            //            if (resJsonArr != null)//返回数据反序列化成功
            //            {
            //                if (resJsonArr.Code == "0")//返回0代表成功
            //                {
            //                    DB.Model.M_OrderInfo orderInfoNow = new DB.BLL.M_OrderInfo(DBEnum.Slave).GetModel(orderInfo[i].OrderID);//为了严谨再次查数据库从数据库取这个实体
            //                    if (orderInfoNow != null)//不为空
            //                    {
            //                        orderInfoNow.OrderStatus = 9;
            //                        new DB.BLL.M_OrderInfo(DBEnum.Master).Update(orderInfoNow);//更新数据，更新失败不做任何处理
            //                        ToolHelper.WriteTxt(Server.MapPath("/log.txt"), nowTime + orderInfo[i].OrderSn + "推送成功，返回订单号：" + resJsonArr.Desc, false);
            //                    }
            //                }
            //                else//返回不为0代表失败
            //                {
            //                    mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "推送订单至海淘出错，错误信息：" + resJsonArr.Desc + "";
            //                    SendEmail.SendEmails(email, "订单推送出错", mailContent);
            //                }
            //            }else//返回数据反序列化失败
            //            {
            //                mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "推送订单至海淘出错，错误信息：海淘返回信息反序列化出错";
            //                SendEmail.SendEmails(email, "订单推送出错", mailContent);
            //            }
            //        }
            //        else//获取当前订单的商品数据出错
            //        {
            //            mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "推送订单至海淘出错,错误信息：商品不存在";
            //            SendEmail.SendEmails(email, "订单推送出错", mailContent);
            //        }
            //    }//for循环结束

            //}//没有任务就什么都不执行

            #endregion

            #region 查询订单状态

            //WMSAPIHandler.WMSNotifyQuery();

            //WMSAPIHandler.WMSNotifyQueryToVerify();

            //int ss = DateTime.Now.Day;
            //int hh = DateTime.Now.Hour;
            //if()

            #endregion


            #region XF系统对接

            //List<DB.Model.M_OrderInfo> orderInfoList = new DB.BLL.M_OrderInfo(DBEnum.Slave).GetModelList(" OrderStatus =0 AND PayStatus= 1 AND PayID=20 ");
            //if(orderInfoList != null && orderInfoList.Count > 0)
            //{
            //    foreach (var item in orderInfoList)
            //    {
            //        XFServiceAPI.GetOrderInfoPaymentStatus(item);
            //    }
            //}
            #endregion


            #region 正则表达式      

            //string str ="<img src=\"http://files/imgs/20161215/201612150352085186detial.jpg\" alt=\"\" /><img src=\"http://files/imgs/20161215/201612150349185418detial.jpg\" alt=\"\" /> < h1 >uuuuuuu </ h1 > ";
            ////string reg = "<img(.*?)(src)=\"[^(http)]"; //     [^()]+  非括弧号内内容的其它任意字符
            //string reg = @"<img.+src\b*=\b*\""(http.*?jpg)";
            //Regex tmpreg = new Regex(reg);
            //string ddd = string.Empty;

            //string ggg = "";
            //Match match = tmpreg.Match(str);
            //GroupCollection group = match.Groups;
            //for (int i = 0; i < group.Count; i++)
            //{
            //    ggg += i+":"+group[i];
            //}
            //string s= str;
            //MatchCollection ce = tmpreg.Matches(str);
            //foreach (var item in ce)
            //{
            //    ddd = item.ToString();
            //}

            //string ddsdfd=   tmpreg.Replace(str, "<img src=\"mallmanage.51ipc.com/");
            // string ddd = string.Empty;

            //str.Replace(str,    );




            #endregion

            //string ss = new DSMTMALL.DB.BLL.MH_Bll(DBEnum.Slave).GetWMSSyncGoodsStock();
            //new DB.BLL.MH_Bll(DBEnum.Master).SyncWMSGoodsStock("15JP01600203-XZM80", 44444444);

            


        }






        /// <summary>  
        /// 提交Json数据且获取接口返回的数据  
        /// </summary>  
        /// <param name="url">网址</param>  
        /// <param name="method">Get/Post</param>  
        /// <param name="postDataStr">提交数据{"name":"zhangsan","pwd":"123456"}</param>  
        /// <returns></returns>  
        //private string HttpPost(string url, string method, string postDataStr)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    request.Method = method;

        //    if (!string.IsNullOrWhiteSpace(postDataStr))
        //    {
        //        request.ContentType = "application/json";
        //        byte[] postData = Encoding.UTF8.GetBytes(postDataStr);
        //        request.ContentLength = postData.Length;
        //        System.IO.Stream outputStream = request.GetRequestStream();
        //        outputStream.Write(postData, 0, postData.Length);
        //        outputStream.Close();
        //    }
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    Stream myResponseStream = response.GetResponseStream();
        //    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
        //    string retString = myStreamReader.ReadToEnd();
        //    myStreamReader.Close();
        //    myResponseStream.Close();
        //    return retString;
        //}
    }
}