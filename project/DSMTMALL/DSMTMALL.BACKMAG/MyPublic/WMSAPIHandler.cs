using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEntity;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace DSMTMALL.BACKMAG.MyPublic
{
    public class WMSAPIHandler
    {
        private static string Appsecret = "2E94F7B2-2557-432C-85FC-FB12EDC2TEST";
        private static string MethodAdd = "StockOut.Notify.Add";
        private static string MethodQuery = "StockOut.Notify.Query";
        private static string Test = "test";
        private static string V = "v1.0";
        private static string Url = @"http://120.55.240.59:8082/OMSWeb/WebService/OrderService.asmx/Api";
        private static string Email = "756657790@qq.com";
        private static string Owner = "线下测试1号仓库";
        private static readonly string AssemblyPath = ConfigurationManager.AppSettings["SUCLog"];
        

        ///// <summary>
        ///// 推送订单
        ///// </summary>
        //public static void WMSNotifyAdd()
        //{
        //    try
        //    {
        //        //基础参数配置
        //        DateTime nowTime = DateTime.Now;
        //        string appsecret = Appsecret;
        //        string wmsNotifyAddStr = string.Empty;
        //        string test = Test;
        //        string method = MethodAdd;
        //        string v = V;
        //        string url = Url;
        //        string email = Email;
        //        WMSNotifyAdd wmsNotifyAdd = new WMSNotifyAdd(Owner);
        //        string logPath = "D:/DSMTMALL/" + nowTime.ToString("yyyymm/dd") + AssemblyPath + ".log";
        //        //执行订单推送
        //        SortedDictionary<string, string> dicQuery = new SortedDictionary<string, string>();
        //        string strWhere = "SuppliersID=1 AND OrderStatus=1 AND PayStatus=2 LIMIT 50";//1.订单的商品来源要是海淘的，2.订单的状态要已确认，3.订单的支付状态是已付款的
        //        List<DB.Model.M_OrderInfo> orderInfo = new DB.BLL.M_OrderInfo(DBEnum.Slave).GetModelList(strWhere);//获取所有要推送的订单列表
        //        if (orderInfo != null || orderInfo.Count > 0)
        //        {
        //            List<Payment> payMentList = null;
        //            Payment payMentInfo = null;
        //            Dictionary<string, string> dicOrderInfo = null;
        //            Dictionary<string, string> dicOrderGoods = null;
        //            AddContent addContent = null;
        //            List<AddRows> addRowsList = null;
        //            WMSRetrunInfo resJsonArr = null;
        //            OrderHelper wmsStringHandler = new OrderHelper();
        //            List<DB.Model.M_OrderGoods> orderGoodsList = null;
        //            string resWMSVal = string.Empty;
        //            string mailContent = string.Empty;
        //            string dicSignStr = string.Empty;
        //            string param = string.Empty;
        //            for (int i = 0; i < orderInfo.Count; i++)//推送当前列表第一条订单信息
        //            {
        //                dicQuery = new SortedDictionary<string, string>();//每一订单推送后都必须要这个键值对初始化
        //                payMentList = new List<Payment>();
        //                payMentInfo = new Payment();
        //                resJsonArr = new WMSRetrunInfo();
        //                resWMSVal = string.Empty;
        //                mailContent = string.Empty;
        //                param = string.Empty;
        //                nowTime = DateTime.Now;
        //                orderGoodsList = new DB.BLL.M_OrderGoods(DBEnum.Slave).GetModelList("OrderSn='" + orderInfo[i].OrderSn + "'");
        //                if (orderGoodsList != null && orderGoodsList.Count > 0)//订单的商品信息不为空
        //                {
        //                    dicOrderInfo = new Dictionary<string, string>();
        //                    dicOrderGoods = new Dictionary<string, string>();
        //                    dicOrderInfo.Add("Created", "AddTime");//订单生成时间
        //                    dicOrderInfo.Add("Modified", "UpdateTime");//最后修改时间
        //                    dicOrderInfo.Add("PaymentTime", "PayTime");//订单付款时间
        //                    dicOrderInfo.Add("OrderOuterId", "OrderUnifySn");//订单外部单号
        //                    dicOrderInfo.Add("BuyerMessage", "Postscript");//顾客留言
        //                    dicOrderInfo.Add("SellerComment", "Postscript");//卖家备注
        //                    dicOrderInfo.Add("LogisticCompany", "ShippingName");//物流公司名称
        //                    dicOrderInfo.Add("ReceiverName", "Consignee");//收货人姓名
        //                    dicOrderInfo.Add("ReceiverProvince", "ProvinceName");//收货人所在省
        //                    dicOrderInfo.Add("ReceiverCity", "CityName");//城市
        //                    dicOrderInfo.Add("ReceiverDistrict", "DistrictName");//地区
        //                    dicOrderInfo.Add("ReceiverAddress", "Address");//详细地址
        //                    dicOrderInfo.Add("ReceiverPhone1", "Mobile");//手机号码
        //                    dicOrderInfo.Add("BuyerCode", "Mobile");//客户编码用手机号代替
        //                    dicOrderInfo.Add("BuyerTrueName", "Consignee");//买家身份证号
        //                    dicOrderInfo.Add("BuyerIdCardNo", "ConsigneeCardNo");//买家真实姓名
        //                    dicOrderInfo.Add("PayAmount", "OrderAmount");//总订单价格
        //                    addContent = EntityHelper<AddContent>.EntityValueToEntity(orderInfo[i], dicOrderInfo);
        //                    //商品信息--一笔订单会有多个商品信息

        //                    dicOrderGoods.Add("OrderRowId", "OrderID");//订单行ID
        //                    dicOrderGoods.Add("MerchId", "GoodsID");//第三方平台商品ID
        //                    dicOrderGoods.Add("MerchCode", "GoodsSn");//商品编号对应海淘
        //                    dicOrderGoods.Add("RowDesc", "GoodsAttr");//行描述
        //                    dicOrderGoods.Add("Qty", "BuyNumber");//购买数量
        //                    dicOrderGoods.Add("Price", "GoodsPrice");//购买价格
        //                    addRowsList = EntityHelper<AddRows>.EntityListValueToEntityList(orderGoodsList, dicOrderGoods);
        //                    addContent.Rows = addRowsList;
        //                    payMentInfo.PaymentMethod = (orderInfo[i].PayID.ToString() == "10") ? "10" : "90";
        //                    payMentInfo.Amount = orderInfo[i].OrderAmount.ToString();
        //                    payMentList.Add(payMentInfo);
        //                    addContent.PayMents = payMentList;
        //                    wmsNotifyAdd.Content = addContent;
        //                    //wmsNotifyAddStr = JsonHelper.EntityToJson(wmsNotifyAdd);//将该笔订单信息JSON格式化
        //                    wmsNotifyAddStr = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(wmsNotifyAdd);//将该笔订单信息JSON格式化

        //                    dicQuery.Add("appkey", test);
        //                    dicQuery.Add("method", method);
        //                    dicQuery.Add("param", wmsNotifyAddStr);
        //                    dicQuery.Add("v", v);
        //                    dicQuery.Add("timestamp", nowTime.ToString());

        //                    dicSignStr = wmsStringHandler.WMSStringHanlder(dicQuery, appsecret);
        //                    param = "appkey=" + test + "&timestamp=" + nowTime + "&v=" + v + "&method=" + method + "&param=" + wmsNotifyAddStr + "&sign=" + dicSignStr;
        //                    try
        //                    {
        //                        resWMSVal = ToolHelper.Post(url, param);
        //                    }
        //                    catch
        //                    {
        //                        mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "推送订单至海淘出错，错误信息：与海淘的订单推送接口连接出错";
        //                        SendEmail.SendEmails(email, "订单推送出错", mailContent);
        //                    }
        //                    resJsonArr = JsonHelper.JsonToEntity<WMSRetrunInfo>(resWMSVal);
        //                    if (resJsonArr != null)//返回数据反序列化成功
        //                    {
        //                        if (resJsonArr.Code == "0")//返回0代表成功
        //                        {
        //                            DB.Model.M_OrderInfo orderInfoNow = new DB.BLL.M_OrderInfo(DBEnum.Slave).GetModel(orderInfo[i].OrderID);//为了严谨再次查数据库从数据库取这个实体
        //                            if (orderInfoNow != null)//不为空
        //                            {
        //                                orderInfoNow.OrderStatus = 9;
        //                                new DB.BLL.M_OrderInfo(DBEnum.Master).Update(orderInfoNow);//更新数据，更新失败不做任何处理
        //                                ToolHelper.WriteTxt(logPath, nowTime + orderInfo[i].OrderSn + "推送成功，返回订单号：" + resJsonArr.Desc, false);
        //                            }
        //                        }
        //                        else//返回不为0代表失败
        //                        {
        //                            mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "推送订单至海淘出错，错误信息：" + resJsonArr.Desc + "";
        //                            SendEmail.SendEmails(email, "订单推送出错", mailContent);
        //                        }
        //                    }
        //                    else//返回数据反序列化失败
        //                    {
        //                        mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "推送订单至海淘出错，错误信息：海淘返回信息反序列化出错";
        //                        SendEmail.SendEmails(email, "订单推送出错", mailContent);
        //                    }
        //                }
        //                else//获取当前订单的商品数据出错
        //                {
        //                    mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "推送订单至海淘出错,错误信息：商品不存在";
        //                    SendEmail.SendEmails(email, "订单推送出错", mailContent);
        //                }
        //            }//for循环结束

        //        }//没有任务就什么都不执行
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = "D:/timerErrs.txt";
        //        ToolHelper.WriteTxt(path, ex.ToString(), false);
        //    }
        //}

        ///// <summary>
        ///// 查询订单
        ///// </summary>
        //public static void WMSNotifyQuery()
        //{
        //    WMSNotifyQuery notifyQuery = new WMSNotifyQuery(Owner);
        //    notifyQuery.Code = "161202100526377";
        //    string wmsNotifyQueryStr = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(notifyQuery);//将该笔订单信息JSON格式化
        //    SortedDictionary<string,string> dicQuery = new SortedDictionary<string, string>();//每一订单推送后都必须要这个键值对初始化
        //    DateTime nowTime = DateTime.Now;

        //    string appsecret = Appsecret;
        //    //string wmsNotifyAddStr = string.Empty;
        //    string test = Test;
        //    string method = MethodQuery;
        //    string v = V;
        //    string url = Url;
        //    string email = Email;
        //    string resWMSVal = string.Empty;
        //    dicQuery.Add("appkey", test);
        //    dicQuery.Add("method", method);
        //    dicQuery.Add("param", wmsNotifyQueryStr);
        //    dicQuery.Add("v", v);
        //    dicQuery.Add("timestamp", nowTime.ToString());
        //    OrderHelper wmsStringHandler = new OrderHelper();
        //    string dicSignStr = wmsStringHandler.WMSStringHanlder(dicQuery, appsecret);
        //    string   param = "appkey=" + test + "&timestamp=" + nowTime + "&v=" + v + "&method=" + method + "&param=" + wmsNotifyQueryStr + "&sign=" + dicSignStr;
        //    try
        //    {
        //        resWMSVal = ToolHelper.Post(url, param);
        //    }
        //    catch
        //    {
        //        string mailContent = nowTime + "订单编号：" + "161202100526377" + "推送订单至海淘出错，错误信息：与海淘的订单推送接口连接出错";
        //        SendEmail.SendEmails(email, "订单推送出错", mailContent);
        //    }
        //    WMSReturnQuery resJsonArr = JsonHelper.JsonToEntity<WMSReturnQuery>(resWMSVal);
        //}

        //#region 海淘-校对双方订单
        ///// <summary>
        ///// 查询订单进行校队
        ///// </summary>
        //public static void WMSNotifyQueryToVerify()
        //{
        //    //基础配置
        //    OrderHelper wmsStringHandler = new OrderHelper();
        //    DateTime nowTime = DateTime.Now;
        //    string appsecret = Appsecret;
        //    string test = Test;
        //    string method = MethodQuery;
        //    string v = V;
        //    string url = Url;
        //    string email = Email;
        //    string mailContent = string.Empty;
        //    string logPath = "D:/d.log";
        //    WMSNotifyQuery notifyQuery = new WMSNotifyQuery(Owner);
        //    List<DSMTMALL.DB.Model.M_OrderGoods> orderGoodsList = null;
        //    List<WMSStockOutRow> wmsStockOutRow = null;
        //    SortedDictionary<string, string> dicQuery = null;// new SortedDictionary<string, string>();//每一订单推送后都必须要这个键值对初始化
        //    string strWhere = "SuppliersID=1 AND OrderStatus=9 AND PayStatus=2 AND IsVerify=0  LIMIT 50";//1.订单的商品来源要是海淘的，2.订单的状态要已推送订单，3.订单的支付状态是已付款的 4.订单校对为0未校对
        //    List<DSMTMALL.DB.Model.M_OrderInfo> orderInfo = new DSMTMALL.DB.BLL.M_OrderInfo(DBEnum.Slave).GetModelList(strWhere);//获取所有要校对的订单列表
        //    if (orderInfo != null || orderInfo.Count > 0)
        //    {
        //        for (int i = 0; i < orderInfo.Count; i++)//对符合条件的内容进行for循环
        //        {
        //            string dicSignStr = string.Empty;
        //            string resWMSVal = string.Empty;
        //            nowTime = DateTime.Now;
        //            string param = string.Empty;
        //            orderGoodsList = null;
        //            wmsStockOutRow = null;
        //            mailContent = string.Empty;
        //            if (!string.IsNullOrEmpty(orderInfo[i].TPLOrderNo))//第三方系统推送回的订单号不为空的情况下执行订单校对
        //            {
        //                dicQuery = new SortedDictionary<string, string>();//每一订单推送后都必须要这个键值对初始化
        //                notifyQuery.Code = orderInfo[i].TPLOrderNo;
        //                string wmsNotifyQueryStr = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(notifyQuery);//将该笔订单信息JSON格式化
        //                dicQuery.Add("appkey", test);
        //                dicQuery.Add("method", method);
        //                dicQuery.Add("param", wmsNotifyQueryStr);
        //                dicQuery.Add("v", v);
        //                dicQuery.Add("timestamp", nowTime.ToString());
        //                dicSignStr = wmsStringHandler.WMSStringHanlder(dicQuery, appsecret);
        //                param = "appkey=" + test + "&timestamp=" + nowTime + "&v=" + v + "&method=" + method + "&param=" + wmsNotifyQueryStr + "&sign=" + dicSignStr;
        //                try
        //                {
        //                    resWMSVal = ToolHelper.Post(url, param);
        //                }
        //                catch
        //                {
        //                    mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "与海淘校对订单信息出错，错误信息：与海淘的订单校对接口连接出错";
        //                    SendEmail.SendEmails(email, "订单校对出错", mailContent);
        //                }
        //                WMSReturnQuery resJsonArr = JsonHelper.JsonToEntity<WMSReturnQuery>(resWMSVal);
        //                if (resJsonArr != null)//返回数据反序列化成功
        //                {
        //                    if (resJsonArr.Code == "0")//返回0-成功
        //                    {
        //                        wmsStockOutRow = resJsonArr.Result.Rows;
        //                        orderGoodsList = new DSMTMALL.DB.BLL.M_OrderGoods(DBEnum.Slave).GetModelList("OrderSn=" + orderInfo[i].OrderSn);//获取该订单商品列表
        //                        if (orderGoodsList != null && wmsStockOutRow != null && orderGoodsList.Count > 0 && wmsStockOutRow.Count > 0)//该订单商品信息不为空
        //                        {
        //                            if (orderGoodsList.Count == wmsStockOutRow.Count)//等长
        //                            {
        //                                orderGoodsList.Sort((DSMTMALL.DB.Model.M_OrderGoods x, DSMTMALL.DB.Model.M_OrderGoods y) => x.GoodsSn.CompareTo(y.GoodsSn));
        //                                wmsStockOutRow.Sort((WMSStockOutRow x, WMSStockOutRow y) => x.MerchId.CompareTo(y.MerchId));
        //                                bool isSame = true;
        //                                for (int j = 0; j < orderGoodsList.Count; j++)
        //                                {   //对俩个按照商品编号排序后的商品信息集合进行校对商品编号与数量是否一致，
        //                                    if (orderGoodsList[j].GoodsSn != wmsStockOutRow[j].MerchId ||Convert.ToDouble(orderGoodsList[j].BuyNumber) != Convert.ToDouble(wmsStockOutRow[j].Qty))
        //                                    {
        //                                        isSame = false;
        //                                        break;
        //                                    }
        //                                }
        //                                if (isSame)
        //                                {
        //                                    //核对一致，更改数据库数据，修改核实字段
        //                                    DSMTMALL.DB.Model.M_OrderInfo orderInfoNow = new DSMTMALL.DB.BLL.M_OrderInfo(DBEnum.Slave).GetModel(orderInfo[i].OrderID);//为了严谨再次查数据库从数据库取这个实体
        //                                    if (orderInfoNow != null)//不为空
        //                                    {
        //                                        orderInfoNow.IsVerify = 1;
        //                                        if (new DSMTMALL.DB.BLL.M_OrderInfo(DBEnum.Master).Update(orderInfoNow))//更新数据，更新失败不做任何处理
        //                                        { ToolHelper.WriteTxt(logPath, nowTime + "/" + orderInfo[i].OrderSn + "校对成功，返回信息：" + resJsonArr.Desc+"物流公司："+resJsonArr.Result.LogisticCompany+"物流单号：" + resJsonArr.Result.LogisiticNumber+"发货时间：" + resJsonArr.Result.StockOutDate, false); }
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "与海淘校对订单信息出错，错误信息：该笔订单存在异常与第三方生成的订单商品存在差异，请立即人工校对商品信息";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "与海淘校对订单信息出错，错误信息：该笔订单存在异常与第三方生成的订单商品存在差异，请立即人工校对商品信息";
        //                            }
        //                        }
        //                        else//订单商品信息为空
        //                        {
        //                            mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "与海淘校对订单信息出错，错误信息：该笔订单存在异常与第三方生成的订单商品存在差异，请立即人工校对商品信息";
        //                        }
        //                    }
        //                    else//返回1
        //                    {
        //                        mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "与海淘校对订单信息出错，错误信息：" + resJsonArr.Desc + "";
        //                    }
        //                }
        //                else
        //                {
        //                    mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "与海淘校对订单信息出错，错误信息：海淘返回信息反序列化出错";
        //                }
        //            }
        //            else
        //            {
        //                mailContent = nowTime + "订单编号：" + orderInfo[i].OrderSn + "与海淘校对订单信息出错，错误信息：该订单没有第三方系统后返回的订单编号";
        //            }
        //            if (!string.IsNullOrEmpty(mailContent))
        //            {
        //                SendEmail.SendEmails(email, "订单校对出错", mailContent);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// 自定义比较器，实现对实体类WMSStockOutRow进行按MerchId进行排序-已通过lamand表达式实现
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <returns></returns>
        //private static int IComparer(WMSStockOutRow x, WMSStockOutRow y)
        //{
        //    return x.MerchId.CompareTo(y.MerchId);
        //}

        ///// <summary>
        ///// 自定义比较器 实现对实体类M_OrderGoods进行按GoodsSn进行排序-已通过lamand表达式实现
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <returns></returns>
        //private static int IComparer(DSMTMALL.DB.Model.M_OrderGoods x, DSMTMALL.DB.Model.M_OrderGoods y)
        //{
        //    return x.GoodsSn.CompareTo(y.GoodsSn);
        //}
        //#endregion

    }
}