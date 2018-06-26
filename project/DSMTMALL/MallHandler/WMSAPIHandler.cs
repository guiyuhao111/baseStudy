using System;
using System.Collections.Generic;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEntity;
using DSMTMALL.Core.Common.MyEnum;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Linq;

namespace MallHandler
{
    public class WMSAPIHandler
    {
        private static string MethodAdd = "StockOut.Notify.Add";
        private static string MethodQuery = "StockOut.Notify.Query";
        private static string MethodStock = "StockOut.Stock.Query";

        //private static string Appsecret = "fxtest123";
        //private static string AppKey = "fxtest";
        //private static string Owner = "";
        //private static string V = "v1.0";
        //private static string Url = @"http://121.40.129.246:8088/OMS_Interface/api";
        //private static string ShopId = "16";

        private static string Appsecret = "2E94F7B2-2557-432C-85FC-FB12EDC2LL";
        private static string AppKey = "nblonglian";
        private static string Owner = "迅城仓（保税仓）";
        private static string V = "v1.0";
        private static string Url = @"http://111.1.41.28:8082/OMSWeb/WebService/OrderService.asmx/Api";
        private static string ShopId = "12";

        private static string Email = "756657790@qq.com";
       
        #region 海淘-推送系统订单
        public static void WMSNotifyAdd()
        {
            //基础参数配置
            DateTime nowTime = DateTime.Now;
            string wmsNotifyAddStr = string.Empty;
            string method = MethodAdd;
            try
            {
                WMSNotifyAdd wmsNotifyAdd = new WMSNotifyAdd(Owner);
                List<DSMTMALL.DB.Model.M_OrderInfo> orderInfo = null;

                //执行订单推送
                SortedDictionary<string, string> dicQuery = new SortedDictionary<string, string>();
                string strWhere = "SuppliersID=101 AND OrderStatus=1 AND PayStatus=2 LIMIT 50";//1.订单的商品来源要是海淘的，2.订单的状态要已确认，3.订单的支付状态是已付款的
                orderInfo = new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Slave).GetModelList<DSMTMALL.DB.Model.M_OrderInfo>(strWhere, null);//获取所有要推送的订单列表
                if (orderInfo != null && orderInfo.Count > 0)
                {
                    List<Payment> payMentList = null;
                    Payment payMentInfo = null;
                    Dictionary<string, string> dicOrderInfo = null;
                    Dictionary<string, string> dicOrderGoods = null;
                    AddContent addContent = null;
                    List<AddRows> addRowsList = null;
                    WMSRetrunInfo resJsonArr = null;
                    OrderHelper wmsStringHandler = new OrderHelper();
                    List<DSMTMALL.DB.Model.M_OrderGoods> orderGoodsList = null;
                    string resWMSVal = string.Empty;
                    string mailContent = string.Empty;
                    string dicSignStr = string.Empty;
                    string param = string.Empty;
                    for (int i = 0; i < orderInfo.Count; i++)//推送当前列表第一条订单信息
                    {
                        dicQuery = new SortedDictionary<string, string>();//每一订单推送后都必须要这个键值对初始化
                        payMentList = new List<Payment>();
                        payMentInfo = new Payment();
                        resJsonArr = new WMSRetrunInfo();
                        resWMSVal = string.Empty;
                        mailContent = string.Empty;
                        param = string.Empty;
                        nowTime = DateTime.Now;
                        orderGoodsList = new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Slave).GetModelList<DSMTMALL.DB.Model.M_OrderGoods>("OrderSn=@_OrderSn", new { _OrderSn = orderInfo[i].OrderSn });
                        if (orderGoodsList != null && orderGoodsList.Count > 0)//订单的商品信息不为空
                        {
                            dicOrderInfo = new Dictionary<string, string>();
                            dicOrderGoods = new Dictionary<string, string>();
                            dicOrderInfo.Add("Created", "AddTime");//订单生成时间
                            dicOrderInfo.Add("Modified", "UpdateTime");//最后修改时间
                            dicOrderInfo.Add("PaymentTime", "PayTime");//订单付款时间
                            dicOrderInfo.Add("OrderOuterId", "OrderUnifySn");//订单外部单号
                            dicOrderInfo.Add("BuyerMessage", "Postscript");//顾客留言
                            dicOrderInfo.Add("SellerComment", "Postscript");//卖家备注
                            dicOrderInfo.Add("LogisticCompany", "ShippingName");//物流公司名称
                            dicOrderInfo.Add("ReceiverName", "Consignee");//收货人姓名
                            dicOrderInfo.Add("ReceiverProvince", "ProvinceName");//收货人所在省
                            dicOrderInfo.Add("ReceiverCity", "CityName");//城市
                            dicOrderInfo.Add("ReceiverDistrict", "DistrictName");//地区
                            dicOrderInfo.Add("ReceiverAddress", "Address");//详细地址
                            dicOrderInfo.Add("ReceiverPhone1", "Mobile");//手机号码
                            dicOrderInfo.Add("BuyerCode", "Mobile");//客户编码用手机号代替
                            dicOrderInfo.Add("BuyerTrueName", "Consignee");//买家身份证号
                            dicOrderInfo.Add("BuyerIdCardNo", "ConsigneeCardNo");//买家真实姓名
                            dicOrderInfo.Add("FeeAmount", "ShippingFee");//向买家收取的运费
                            dicOrderInfo.Add("PayAmount", "OrderAmount");//总订单价格
                            addContent = EntityHelper<AddContent>.EntityValueToEntity(orderInfo[i], dicOrderInfo);
                            //商品信息--一笔订单会有多个商品信息
                            dicOrderGoods.Add("OrderRowId", "OrderID");//订单行ID
                            dicOrderGoods.Add("MerchId", "GoodsID");//第三方平台商品ID
                            dicOrderGoods.Add("MerchCode", "GoodsSn");//商品编号对应海淘
                            dicOrderGoods.Add("RowDesc", "GoodsAttr");//行描述
                            dicOrderGoods.Add("Qty", "BuyNumber");//购买数量
                            dicOrderGoods.Add("Price", "GoodsPrice");//购买价格
                            addRowsList = EntityHelper<AddRows>.EntityListValueToEntityList(orderGoodsList, dicOrderGoods);
                            if(addRowsList!=null && addRowsList.Count > 0)
                            {
                                for (int j = 0; j < addRowsList.Count; j++)
                                {
                                    addRowsList[j].Amount = Convert.ToString(Convert.ToInt32(addRowsList[j].Qty) * Convert.ToDecimal(addRowsList[j].Price));
                                }
                                
                            }
                            addContent.Rows = addRowsList;
                            addContent.ShopId = ShopId;
                            payMentInfo.PaymentMethod = orderInfo[i].PayID.ToString();
                            payMentInfo.Amount = orderInfo[i].OrderAmount.ToString();
                            payMentList.Add(payMentInfo);
                            addContent.PayMents = payMentList;
                            wmsNotifyAdd.Content = addContent;
                            wmsNotifyAddStr = new JavaScriptSerializer().Serialize(wmsNotifyAdd);//将该笔订单信息JSON格式化

                            dicQuery.Add("appkey", AppKey);
                            dicQuery.Add("method", method);
                            dicQuery.Add("param", wmsNotifyAddStr);
                            dicQuery.Add("v", V);
                            dicQuery.Add("timestamp", nowTime.ToString());
                            dicSignStr = wmsStringHandler.WMSStringHanlder(dicQuery, Appsecret);
                            param = "appkey=" + AppKey + "&timestamp=" + nowTime + "&v=" + V + "&method=" + method + "&param=" + wmsNotifyAddStr + "&sign=" + dicSignStr;
                            FileHelper.logger.Debug(param);
                            try
                            {
                                resWMSVal = ToolHelper.Post(Url, param);
                                FileHelper.logger.Debug(resWMSVal);
                            }
                            catch
                            {
                                FileHelper.logger.Warn(string.Format("订单编号：{0}推送订单至海淘出错，错误信息：与海淘的订单校对接口连接出错",orderInfo[i].OrderSn));
                            }
                            resJsonArr = new JavaScriptSerializer().Deserialize<WMSRetrunInfo>(resWMSVal);
                            if (resJsonArr != null)//返回数据反序列化成功
                            {
                                if (resJsonArr.Code == "0")//返回0代表成功
                                {
                                    if (new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<DSMTMALL.DB.Model.M_OrderInfo>(" OrderID=@_OrderID AND Version=@_Version ", new { _OrderID = orderInfo[i].OrderID, _Version = orderInfo[i].Version, OrderStatus = 9, TPLOrderNo = resJsonArr.Desc, Version = orderInfo[i].Version + 1 }))
                                    {
                                       FileHelper.logger.Info(string.Format("{0}推送成功,返回订单号:{1}",orderInfo[i].OrderSn ,resJsonArr.Desc));
                                    }//更新失败必须做处理 ==》改：不必做处理，因为会再次调API接口尝试更新订单信息
                                }
                                else//返回不为0代表失败
                                {
                                    FileHelper.logger.Warn(string.Format("订单编号:{0}推送订单至海淘出错,错误信息:{1}", orderInfo[i].OrderSn, resJsonArr.Desc)); //返回状态不为0代表失败，失败就不用写异常
                                }
                            }
                            else//返回数据反序列化失败
                            {
                                FileHelper.logger.Warn(string.Format("订单编号:{0}推送订单至海淘出错，错误信息：海淘返回信息反序列化出错", orderInfo[i].OrderSn));
                            }
                        }
                        else//获取当前订单的商品数据出错
                        {
                            FileHelper.logger.Warn(string.Format("订单编号:{0}推送订单至海淘出错,错误信息：商品不存在", orderInfo[i].OrderSn));
                        }
                    }//for循环结束

                }//没有任务就什么都不执行
            }
            catch (Exception ex)
            {
                if (ex.Message == ExceptionHelper.Exception1000)
                {
                    return;//检测到接口返回数据出错，直接跳出循环体
                }
                else
                {
                    FileHelper.logger.Warn(ex.Message);
                }
            }
        }

        #endregion

        #region 海淘-校对双方订单
        /// <summary>
        /// 查询订单进行校队
        /// </summary>
        public static void WMSNotifyQuery()
        {
            //基础配置
            OrderHelper wmsStringHandler = new OrderHelper();
            DateTime nowTime = DateTime.Now;
            string method = MethodQuery;
            string email = Email;
            string mailContent = string.Empty;
            string wmsNotifyQueryStr = string.Empty;
            WMSNotifyQuery notifyQuery = new WMSNotifyQuery(Owner);
            List<DSMTMALL.DB.Model.M_OrderGoods> orderGoodsList = null;
            List<WMSStockOutRow> wmsStockOutRow = null;
            WMSReturnQuery resJsonArr = null;
            List<DSMTMALL.DB.Model.M_OrderInfo> orderInfo = null;
            SortedDictionary<string, string> dicQuery = null;// new SortedDictionary<string, string>();//每一订单推送后都必须要这个键值对初始化
            string strWhere = "SuppliersID=101 AND OrderStatus=9 AND PayStatus=2 AND IsVerify=0  LIMIT 50";//1.订单的商品来源要是海淘的，2.订单的状态要已推送订单，3.订单的支付状态是已付款的 4.订单校对为0未校对
            try
            {
                orderInfo = new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Slave).GetModelList<DSMTMALL.DB.Model.M_OrderInfo>(strWhere, null);//获取所有要校对的订单列表
            }
            catch (Exception esql)
            {
                orderInfo = null;
                FileHelper.logger.Warn(string.Format("获取要校对的订单列表信息出错"+esql.Message));
            }
            if (orderInfo != null && orderInfo.Count > 0)
            {
                for (int i = 0; i < orderInfo.Count; i++)//对符合条件的内容进行for循环
                {
                    string dicSignStr = string.Empty;
                    string resWMSVal = string.Empty;
                    nowTime = DateTime.Now;
                    string param = string.Empty;
                    orderGoodsList = null;
                    wmsStockOutRow = null;
                    mailContent = string.Empty;
                    wmsNotifyQueryStr = string.Empty;
                    resJsonArr = null;
                    try
                    {
                        if (!string.IsNullOrEmpty(orderInfo[i].TPLOrderNo))//第三方系统推送回的订单号不为空的情况下执行订单校对
                        {
                            dicQuery = new SortedDictionary<string, string>();//每一订单推送后都必须要这个键值对初始化
                            notifyQuery.Code = orderInfo[i].TPLOrderNo;
                            wmsNotifyQueryStr = new JavaScriptSerializer().Serialize(notifyQuery);//将该笔订单信息JSON格式化
                            dicQuery.Add("appkey", AppKey);
                            dicQuery.Add("method", method);
                            dicQuery.Add("param", wmsNotifyQueryStr);
                            dicQuery.Add("v", V);
                            dicQuery.Add("timestamp", nowTime.ToString());
                            dicSignStr = wmsStringHandler.WMSStringHanlder(dicQuery, Appsecret);
                            param = "appkey=" + AppKey + "&timestamp=" + nowTime + "&v=" + V + "&method=" + method + "&param=" + wmsNotifyQueryStr + "&sign=" + dicSignStr;
                            try
                            {
                                resWMSVal = ToolHelper.Post(Url, param);
                            }
                            catch
                            {
                                FileHelper.logger.Warn(string.Format("{0}与海淘校对订单信息出错，错误信息:与海淘的订单校对接口连接出错", orderInfo[i].OrderSn));
                            }
                            resJsonArr = new JavaScriptSerializer().Deserialize<WMSReturnQuery>(resWMSVal);
                            if (resJsonArr != null)//返回数据反序列化成功
                            {
                                if (resJsonArr.Code == "0")//返回0-成功
                                {
                                    wmsStockOutRow = resJsonArr.Result.Rows;
                                    orderGoodsList = new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Slave).GetModelList<DSMTMALL.DB.Model.M_OrderGoods>(" OrderSn=@_OrderSn", new { _OrderSn = orderInfo[i].OrderSn });//获取该订单商品列表
                                    if (orderGoodsList != null && wmsStockOutRow != null && orderGoodsList.Count > 0 && wmsStockOutRow.Count > 0)//该订单商品信息不为空
                                    {
                                        if (orderGoodsList.Count == wmsStockOutRow.Count)//商品信息品类等长
                                        {
                                            orderGoodsList.Sort((DSMTMALL.DB.Model.M_OrderGoods x, DSMTMALL.DB.Model.M_OrderGoods y) => x.GoodsSn.CompareTo(y.GoodsSn));
                                            wmsStockOutRow.Sort((WMSStockOutRow x, WMSStockOutRow y) => x.MerchId.CompareTo(y.MerchId));
                                            bool isSame = true;
                                            for (int j = 0; j < orderGoodsList.Count; j++)
                                            {   //对俩个按照商品编号排序后的商品信息集合进行校对商品编号与数量是否一致，
                                                if (orderGoodsList[j].GoodsSn != wmsStockOutRow[j].MerchId || Convert.ToDouble(orderGoodsList[j].BuyNumber) != Convert.ToDouble(wmsStockOutRow[j].Qty))
                                                {
                                                    isSame = false;
                                                    break;
                                                }
                                            }
                                            if (isSame)
                                            {
                                                if (new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<DSMTMALL.DB.Model.M_OrderInfo>(" OrderID=@_OrderID AND Version=@_Version ", new { _OrderID = orderInfo[i].OrderID, _Version = orderInfo[i].Version, IsVerify = 1, Version = orderInfo[i].Version + 1 }))
                                                {
                                                    FileHelper.logger.Info(string.Format("{0}校对成功，返回信息:{1}--物流公司:{2}/物流单号:{3}/发货时间:{4}", orderInfo[i].OrderSn, resJsonArr.Desc, resJsonArr.Result.LogisticCompany, resJsonArr.Result.LogisiticNumber, resJsonArr.Result.StockOutDate));
                                                }//校队失败必须做处理，==》改：不必做处理，因为会再次调API接口尝试校队订单信息

                                            }
                                            else
                                            {
                                                FileHelper.logger.Warn(string.Format( "订单编号:{0}与海淘校对订单信息出错,错误信息:返回的商品编号与数量不一致{1}", orderInfo[i].OrderSn, resWMSVal));
                                                throw new Exception(ExceptionHelper.Exception1003);//1003与海淘校对订单出错
                                            }
                                        }
                                        else
                                        {
                                            FileHelper.logger.Warn(string.Format("订单编号:{0}与海淘校对订单信息出错，错误信息：返回的商品数量不一致{1}", orderInfo[i].OrderSn,resWMSVal));
                                            throw new Exception(ExceptionHelper.Exception1003);
                                        }
                                    }
                                    else//订单商品信息为空
                                    {
                                        FileHelper.logger.Warn(string.Format("订单编号:{0}与海淘校对订单信息出错，错误信息：返回的商品数据为空{1}", orderInfo[i].OrderSn, resWMSVal));
                                        throw new Exception(ExceptionHelper.Exception1003);
                                    }
                                }
                                else//返回1
                                {
                                    FileHelper.logger.Warn(string.Format("订单编号:{0}与海淘校对订单信息出,错误信息:{1}}", orderInfo[i].OrderSn, resJsonArr.Desc));
                                }
                            }
                            else
                            {
                                FileHelper.logger.Warn(string.Format("订单编号：{0},与海淘校对订单信息出错，错误信息：海淘返回信息反序列化出错", orderInfo[i].OrderSn));
                            }
                        }
                        else
                        {
                            FileHelper.logger.Warn(string.Format("订单编号:{0}与海淘校对订单信息出错，错误信息：该订单没有第三方系统后返回的订单编号", orderInfo[i].OrderSn));
                        }
                    }
                    catch (Exception e)
                    {
                        if (e.Message == ExceptionHelper.Exception1000)
                        {
                            break;//检测到接口返回数据出错，直接跳出循环体
                        }
                        else
                        {
                            new TryCatchErrHelper().HandlerError(orderInfo[i].OrderID, orderInfo[i].OrderSn, e.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 自定义比较器，实现对实体类WMSStockOutRow进行按MerchId进行排序-已通过lamand表达式实现
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static int IComparer(WMSStockOutRow x, WMSStockOutRow y)
        {
            return x.MerchId.CompareTo(y.MerchId);
        }

        /// <summary>
        /// 自定义比较器 实现对实体类M_OrderGoods进行按GoodsSn进行排序-已通过lamand表达式实现
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static int IComparer(DSMTMALL.DB.Model.M_OrderGoods x, DSMTMALL.DB.Model.M_OrderGoods y)
        {
            return x.GoodsSn.CompareTo(y.GoodsSn);
        }
        #endregion

        #region 海淘-更新发货信息
        /// <summary>
        /// 查询订单-更新发货信息//每天晚上24点之后执行一次，一次同步所有发货信息
        /// </summary>
        public static void WMSNotifyQueryStockOut()
        {
            //基础配置
            OrderHelper wmsStringHandler = new OrderHelper();
            DateTime nowTime = DateTime.Now;
            string method = MethodQuery;
            WMSNotifyQuery notifyQuery = new WMSNotifyQuery(Owner);
            WMSReturnQuery resJsonArr = null;
            List<DSMTMALL.DB.Model.M_OrderInfo> orderInfo = null;
            DSMTMALL.DB.Model.M_OrderInfo orderInfoNow = null;
            SortedDictionary<string, string> dicQuery = null;// new SortedDictionary<string, string>();//每一订单推送后都必须要这个键值对初始化
            //string strWhere = "SuppliersID=101 AND ( OrderStatus=9 OR OrderStatus=1) AND PayStatus=2 ";//1.订单的商品来源要是海淘的，2.订单的状态要已推送订单，3.订单的支付状态是已付款的 
            string strWhere = "SuppliersID=101 AND  OrderStatus=9 AND IsVerify=1 AND PayStatus=2 ";//1.订单的商品来源要是海淘的，2.订单的状态要已推送订单，3.订单的支付状态是已付款的 4.已校对的
            try
            {
                orderInfo = new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Slave).GetModelList<DSMTMALL.DB.Model.M_OrderInfo>(strWhere, null);//获取所有要校对的订单列表
            }
            catch (Exception esql)
            {
                orderInfo = null;
                FileHelper.logger.Warn("获取要同步物流状态的订单列表信息出错,错误信息:"+esql.Message);
            }
            if (orderInfo != null || orderInfo.Count > 0)
            {
                for (int i = 0; i < orderInfo.Count; i++)//对符合条件的内容进行for循环
                {
                    string dicSignStr = string.Empty;
                    string resWMSVal = string.Empty;
                    nowTime = DateTime.Now;
                    string param = string.Empty;
                    resJsonArr = null;
                    if (!string.IsNullOrEmpty(orderInfo[i].TPLOrderNo))//第三方系统推送回的订单号不为空的情况下执行订单校对
                    {
                        dicQuery = new SortedDictionary<string, string>();//每一订单推送后都必须要这个键值对初始化
                        notifyQuery.Code = orderInfo[i].TPLOrderNo;
                        string wmsNotifyQueryStr = new JavaScriptSerializer().Serialize(notifyQuery);//将该笔订单信息JSON格式化
                        dicQuery.Add("appkey", AppKey);
                        dicQuery.Add("method", method);
                        dicQuery.Add("param", wmsNotifyQueryStr);
                        dicQuery.Add("v", V);
                        dicQuery.Add("timestamp", nowTime.ToString());
                        dicSignStr = wmsStringHandler.WMSStringHanlder(dicQuery, Appsecret);
                        param = "appkey=" + AppKey + "&timestamp=" + nowTime + "&v=" + V + "&method=" + method + "&param=" + wmsNotifyQueryStr + "&sign=" + dicSignStr;
                        try
                        {
                            resWMSVal = ToolHelper.Post(Url, param);
                            resJsonArr = new JavaScriptSerializer().Deserialize<WMSReturnQuery>(resWMSVal);
                            if (resJsonArr != null)//返回数据反序列化成功
                            {
                                if (resJsonArr.Code == "0")//返回0-成功 ,并且订单物流状态为0已取消或者40已发货
                                {
                                     if (resJsonArr.Status == "40" || resJsonArr.Status == "10" || (resJsonArr.Status == "20" || resJsonArr.Status == "30") && !string.IsNullOrEmpty(resJsonArr.WhsStatus))
                                    {
                                        orderInfoNow = new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Slave).GetModel<DSMTMALL.DB.Model.M_OrderInfo>(" OrderID=@_OrderID ", new { _OrderID = orderInfo[i].OrderID });//为了严谨再次查数据库从数据库取这个实体
                                        if (orderInfoNow != null)
                                        {
                                            if (resJsonArr.Status == "40")//对方已发货更新物流状态
                                            {
                                                orderInfoNow.Logistical = string.IsNullOrEmpty(resJsonArr.Result.LogisticCompany) ? string.Empty : resJsonArr.Result.LogisticCompany;
                                                orderInfoNow.LogisticalNumber = string.IsNullOrEmpty(resJsonArr.Result.LogisiticNumber) ? string.Empty : resJsonArr.Result.LogisiticNumber;
                                                orderInfoNow.ShippingTime = string.IsNullOrEmpty(resJsonArr.Result.StockOutDate) ? nowTime : Convert.ToDateTime(resJsonArr.Result.StockOutDate);
                                                if (new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<DSMTMALL.DB.Model.M_OrderInfo>("  OrderID=@_OrderID ",
                                                     new
                                                     {
                                                         _OrderID = orderInfo[i].OrderID,
                                                         Logistical = orderInfoNow.Logistical,
                                                         LogisticalNumber = orderInfoNow.LogisticalNumber,
                                                         ShippingTime = orderInfoNow.ShippingTime,
                                                         ShippingStatus = 1,
                                                         OrderStatus = 5,
                                                         Version = orderInfoNow.Version + 1
                                                     })
                                                 ) {//更新数据，更新失败不做任何处理--如果成功则发送微信-发货成功
                                                    WeChatAPI.SendMsg("商品发货提醒", "您的订单" + orderInfo[i].OrderSn + "已从仓库发出，请等待收货，物流单号为：" + orderInfoNow.LogisticalNumber, orderInfo[i].OpenID);
                                                }
                                            }
                                            else if (resJsonArr.Status == "10")//对方已取消发货
                                            {
                                                new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<DSMTMALL.DB.Model.M_OrderInfo>(" OrderID=@_OrderID ",
                                                    new
                                                    {
                                                        _OrderID = orderInfo[i].OrderID,
                                                        OrderStatus = 7,
                                                        Postscript = "海淘取消订单发货:" + resJsonArr.WhsStatus,  //异常状态描述
                                                    Version = orderInfoNow.Version + 1
                                                    });//更新数据，更新失败不做任何处理
                                            }
                                            else if ((resJsonArr.Status == "20" || resJsonArr.Status == "30") && !string.IsNullOrEmpty(resJsonArr.WhsStatus))//对方订单待确认或已配送确认,并且订单异常状态描述不为空，代表有异常信息
                                            {
                                                if (resJsonArr.WhsStatus != "未推送到海关系统" || nowTime > orderInfo[i].PayTime.AddDays(1))//如果返回的错误信息不是	未推送到海关系统 或者 订单付款时间已经超过24小时
                                                {
                                                    new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<DSMTMALL.DB.Model.M_OrderInfo>(" OrderID=@_OrderID ",
                                                       new
                                                       {
                                                           _OrderID = orderInfo[i].OrderID,
                                                           OrderStatus = 7,
                                                           Postscript = "海淘返回的订单状态存在异常，异常原因:" + resJsonArr.WhsStatus,  //异常状态描述
                                                       Version = orderInfoNow.Version + 1
                                                       });//更新数据，更新失败不做任何处理
                                                }
                                            }
                                        }
                                    }//海淘尚未推送订单给海关，暂不作任何处理
                                }
                                else
                                {
                                    FileHelper.logger.Warn(string.Format("{0}订单物流状态更新失败,失败原因:{1}", orderInfo[i].OrderSn, resWMSVal));
                                }
                            }
                        }
                        catch(Exception exmsg)
                        {
                            FileHelper.logger.Warn(string.Format("{0}订单物流状态更新失败,失败原因:{1}", orderInfo[i].OrderSn, exmsg.Message));
                        }
                    }
                }
            }
        }

        #endregion

        #region 海淘-同步库存信息

        public static void WMSSyncStockOut()
        {
            //基础配置
            OrderHelper wmsStringHandler = new OrderHelper();
            DateTime nowTime = DateTime.Now;
            string method = MethodStock;
            double outQuantity = 0;
            WMSStockQuery notifyQuery = new WMSStockQuery(Owner,ShopId );
            WMSStockResult resJsonArr = null;
            List<WMSStocksSku> resWmsSku = null;
            string resWMSVal = string.Empty;
            SortedDictionary<string, string> dicQuery = new SortedDictionary<string, string>();//每一订单推送后都必须要这个键值对初始化
            try
            {
                IEnumerable<dynamic> infoList = new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Slave).GetModelList(" SELECT GoodsSn FROM M_Goods WHERE SuppliersID = 101 AND IsEnable=1 AND IsDelete=0  ORDER BY LastUpdate ASC LIMIT 0,30", null);
                foreach (var item in infoList)
                {
                    notifyQuery.SkuInfo += string.IsNullOrEmpty(notifyQuery.SkuInfo) ? item.GoodsSn : ";" + item.GoodsSn;
                }
            }
            catch
            {
                notifyQuery.SkuInfo = string.Empty;//错误的情况下写日志并赋值为空
                FileHelper.logger.Warn("获取数据库同步库存商品列表失败");
            }
            if (!string.IsNullOrEmpty(notifyQuery.SkuInfo))
            {
                string wmsNotifyQueryStr = new JavaScriptSerializer().Serialize(notifyQuery);//将该笔订单信息JSON格式化
                dicQuery.Add("appkey", AppKey);
                dicQuery.Add("method", method);
                dicQuery.Add("param", wmsNotifyQueryStr);
                dicQuery.Add("v", V);
                dicQuery.Add("timestamp", nowTime.ToString());
                string dicSignStr = wmsStringHandler.WMSStringHanlder(dicQuery, Appsecret);
                string param = "appkey=" + AppKey + "&timestamp=" + nowTime + "&v=" + V + "&method=" + method + "&param=" + wmsNotifyQueryStr + "&sign=" + dicSignStr;
                try
                {
                    resWMSVal = ToolHelper.Post(Url, param);
                    resJsonArr = new JavaScriptSerializer().Deserialize<WMSStockResult>(resWMSVal);
                    if (resJsonArr != null)
                    {
                        if (resJsonArr.Code == "0")
                        {
                            if (resJsonArr.Result != null && resJsonArr.Result.Stocks != null)
                            {
                                resWmsSku = resJsonArr.Result.Stocks;
                                for (int i = 0; i < resWmsSku.Count; i++)
                                {
                                    outQuantity = 0;
                                    if (!string.IsNullOrEmpty(resWmsSku[i].SkuCode) && !string.IsNullOrEmpty(resWmsSku[i].Quantity) && double.TryParse(resWmsSku[i].Quantity, out outQuantity))
                                    {
                                        new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<DSMTMALL.DB.Model.M_Goods>( "GoodsSn=@_GoodsSn",new { GoodsNumber = (int)outQuantity, LastUpdate = nowTime,_GoodsSn=resWmsSku[i].SkuCode });
                                    }
                                }
                            }
                            else
                            {
                                FileHelper.logger.Warn("同步库存信息失败,错误信息:" + resWMSVal);
                            }
                        }
                        else
                        {
                            FileHelper.logger.Warn("同步库存信息失败,错误信息:"+resJsonArr.Desc);
                        }
                    }
                }
                catch (Exception emsg)
                {
                    FileHelper.logger.Warn("同步库存信息失败" + emsg.ToString());
                }
            }
        }
        #endregion

    }
}
