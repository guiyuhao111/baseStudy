using DSMTMALL.BACKMAG.XFTAEAPI;
using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSMTMALL.BACKMAG.MyPublic
{
    public class XFServiceAPI
    {
       
        /// <summary>
        /// 调用XF接口获取开通商城消费的所有公司信息
        /// </summary>
        /// <returns></returns>
        public static string GetCpyInfo()
        {
            string resWrite = "UNLOGIN";
            DSMT_TAE_ServiceSoapClient taeCilent = new DSMT_TAE_ServiceSoapClient();
            GetQueryCpyInfoEntity queryCpyInfoEntity = new GetQueryCpyInfoEntity();
            VerifyHelper verifyHelper = new VerifyHelper();
            queryCpyInfoEntity.GetNo = Guid.NewGuid().ToString();
            verifyHelper.EncryptPmtEntity(queryCpyInfoEntity);
            try
            {
                BackQueryCpyInfoEntity backQueryCpyInfoEntity = taeCilent.QueryCpyInfo(queryCpyInfoEntity);
                if (verifyHelper.CheckPmtSign(ref backQueryCpyInfoEntity))
                {
                    resWrite= backQueryCpyInfoEntity.CpyInfo;
                }
            }catch{}
            return resWrite;
        }

        /// <summary>
        /// 获取申请退款XF系统的流水状态
        /// </summary>
        /// <param name="cpySysID"></param>
        /// <param name="reBackID"></param>
        /// <param name="tradeNo"></param>
        /// <param name="reBackMoney"></param>
        /// <param name="outTradNo"></param>
        /// <param name="resMsg"></param>
        /// <returns></returns>
        public static bool ReBackOrderPayMoney(string cpySysID,string reBackID,string tradeNo,string reBackMoney,DateTime oTraderCreateTime,DateTime rebackOrderCreateTme,out string outTradNo,out string resMsg)
        {
            DSMT_TAE_ServiceSoapClient taeCilent = new DSMT_TAE_ServiceSoapClient();
            GetRefundAccBalanceEntity getRefundAccBalanceEntity = new GetRefundAccBalanceEntity();
            getRefundAccBalanceEntity.CpySysID = cpySysID;
            getRefundAccBalanceEntity.OrderNo = reBackID ;//退回的订单编号
            getRefundAccBalanceEntity.TradeNo = tradeNo;
            getRefundAccBalanceEntity.TradeMoney = reBackMoney;
            getRefundAccBalanceEntity.TradeTime =Convert.ToString( oTraderCreateTime);
            getRefundAccBalanceEntity.OrderTime =Convert.ToString( rebackOrderCreateTme);
            VerifyHelper verifyHelper = new VerifyHelper();
            verifyHelper.EncryptPmtEntity(getRefundAccBalanceEntity);
            outTradNo = string.Empty;
            resMsg = string.Empty;
            try
            {
                BackRefundAccBalanceEntity backRefundAccBalanceEntity=  taeCilent.RefundAccBalance(getRefundAccBalanceEntity);
                if(verifyHelper.CheckPmtSign(ref backRefundAccBalanceEntity))//校验并解密
                {
                    outTradNo = backRefundAccBalanceEntity.TradeNo;
                    switch (backRefundAccBalanceEntity.TradeStatus)
                    {
                        case "10":
                            return true;
                        case "20":
                            return true;
                        case "30" :
                            resMsg = "XF系统返回交易信息不存在";
                            break;
                        case "40":
                            resMsg = "XF系统返回退款金额异常";
                            break;
                        case "50":
                            resMsg = "XF系统执行退款失败";
                            break;
                        default:
                            resMsg = "XF系统返回意外的内容";
                            break;
                    }
                }
            }
            catch(Exception e){ ToolHelper.WriteLogInfoToLocalText(e.Message); }
            return false;
        }
        
        /// <summary>
        /// 将订单的付款金额推送给XF系统进行资金结算
        /// </summary>
        /// <param name="pushType">推送类型</param>
        /// <param name="pushOrderSn">推送订单编号</param>
        /// <param name="pushOrderAmount">推送订单总金额</param>
        /// <param name="pushFeeAmount">推送订单运费金额</param>
        /// <param name="resPayInfo">返回的支付信息</param>
        /// <returns></returns>
        public static bool SeacherThisOrderTrade(string orderID, out string resPayInfo)
        {
            resPayInfo = string.Empty;
            DSMT_TAE_ServiceSoapClient taeCilent = new DSMT_TAE_ServiceSoapClient();
            GetQueryOrderStatusEntity getQueryOrderStatusEntity = new GetQueryOrderStatusEntity();
            BackQueryOrderStatusEntity backQueryOrderStatusEntity = new BackQueryOrderStatusEntity();
            VerifyHelper verifyHelper = new VerifyHelper();
            DB.Model.M_OrderInfo orderInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<DB.Model.M_OrderInfo>(" OrderID=@_OrderID ", new { _OrderID = orderID });
            DB.Model.M_Payment payMentInfo = new DB.BLL.MB_Bll(DBEnum.Slave).GetModel<DB.Model.M_Payment>(" PayID=@_PayID ", new { _PayID = 20 });
            if (orderInfo != null && payMentInfo != null)
            {
                getQueryOrderStatusEntity.OpenID  = orderInfo.OpenID;
                getQueryOrderStatusEntity.OrderNo = orderInfo.OrderSn;
                getQueryOrderStatusEntity.UnifyOrderNo = orderInfo.OrderUnifySn;
                getQueryOrderStatusEntity.OrderTime =Convert.ToString(orderInfo.AddTime);
                string resMsg = string.Empty;
                string orderSn = string.Empty;
                verifyHelper.EncryptPmtEntity(getQueryOrderStatusEntity);
                try
                {
                    backQueryOrderStatusEntity = taeCilent.QueryOrderStatus(getQueryOrderStatusEntity);//调xf接口
                    if (verifyHelper.CheckPmtSign(ref backQueryOrderStatusEntity))
                    {
                        /// 交易状态10-成功20-失败30-待同步40-交易信息不存在
                        if (backQueryOrderStatusEntity != null && !string.IsNullOrEmpty(backQueryOrderStatusEntity.TradeStatus) && !string.IsNullOrEmpty(backQueryOrderStatusEntity.TradeNo))//XF系统返回的数据不为空，并且状态信息不为空
                        {
                            if (backQueryOrderStatusEntity.TradeStatus == "10" || backQueryOrderStatusEntity.TradeStatus == "20" || backQueryOrderStatusEntity.TradeStatus == "30")
                            {
                                orderSn = (backQueryOrderStatusEntity.IsUnify == "0")? orderInfo.OrderSn:orderSn;
                                if (new DB.BLL.MB_Bll(DBEnum.Master).UpdateOrderInfoFromXFPayment(orderInfo.OrderUnifySn, orderSn, ToolHelper.ConventToDecimal(backQueryOrderStatusEntity.TradeMoney, 9999), backQueryOrderStatusEntity.TradeNo, ToolHelper.ConventToDateTime(backQueryOrderStatusEntity.TradeTime, DateTime.Now), payMentInfo, backQueryOrderStatusEntity.CpySysID, backQueryOrderStatusEntity.CpyName, out resMsg))
                                {
                                    //WeChatAPI.SendMsg("异常订单处理成功", "您有一笔异常订单已被成功处理，请关注最新的订单状态",orderInfo.OpenID);
                                    return true;
                                }else
                                {
                                    resPayInfo = resMsg;
                                }
                            } else
                            {
                                resPayInfo = "交易信息不存在";
                            }
                        }else
                        {
                            resPayInfo = "XF系统返回数据出错，请稍后再试";
                        }
                    }
                    else//验签失败
                    {
                        resPayInfo = "从XF系统返回的数据通讯错误,签名验证失败";
                    }
                }
                catch (Exception e)
                {
                    resPayInfo = "与XF系统的通讯出错，返回错误信息：" + e.Message;
                }
            }else
            {
                resPayInfo = "获取相应的数据出错";
            }
            return false;
        }

        /// <summary>
        /// 管理员进行手动同步流水状态
        /// </summary>
        /// <param name="tradeSysID"></param>
        /// <param name="resMsg"></param>
        /// <returns></returns>
        public static bool GetOrderInfoPaymentStatusSync(string tradeSysID, out string resMsg)
        {
            resMsg = string.Empty;
            DB.Model.M_OrderTrade orderTradeInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<DB.Model.M_OrderTrade>(" TradeSysID=@_TradeSysID ", new { _TradeSysID = tradeSysID });
            if (orderTradeInfo != null)
            {
                DSMT_TAE_ServiceSoapClient taeCilent = new DSMT_TAE_ServiceSoapClient();
                GetQueryTradeStatusEntity queryTradeStatusEntity = null;
                VerifyHelper verifyHelper = new VerifyHelper();
                BackQueryTradeStatusEntity backQueryTradeStatusEntity = null;
                DateTime nowTime = DateTime.Now;
                string outOpenID = string.Empty;
                queryTradeStatusEntity = new GetQueryTradeStatusEntity();//初始化
                backQueryTradeStatusEntity = null;
                if (!string.IsNullOrEmpty(orderTradeInfo.TradeNo) && !string.IsNullOrEmpty(orderTradeInfo.CpySysID))//判断流水号公司ID都存在的情况下调用XF接口,并且流水号的创建时间超时10分钟
                {
                    queryTradeStatusEntity.CpySysID = orderTradeInfo.CpySysID;
                    queryTradeStatusEntity.TradeNo = orderTradeInfo.TradeNo;
                    queryTradeStatusEntity.TradeTime =Convert.ToString(orderTradeInfo.PayTime);//流水时间是XF系统返回给我的时间
                    verifyHelper.EncryptPmtEntity(queryTradeStatusEntity);//加密加签
                    try
                    {
                        backQueryTradeStatusEntity = taeCilent.QueryTradeStatus(queryTradeStatusEntity);//调用XF接口
                    }
                    catch (Exception e)//通讯异常
                    {
                        resMsg = "与XF系统扣款状态同步查询出错,错误信息：" + e.Message;
                    }
                    if (verifyHelper.CheckPmtSign(ref backQueryTradeStatusEntity))//尝试解签，解签失败的话不执行任何代码
                    {
                        if (backQueryTradeStatusEntity.TradeType == "0")//交易类型是0
                        {
                            if (backQueryTradeStatusEntity.TradeStatus == "10" || backQueryTradeStatusEntity.TradeStatus == "20") //交易状态10-已完成20-已撤销(余额不足付款失败?)30-待同步40-交易信息不存在
                            {
                                if (new DB.BLL.MY_Bll(DBEnum.Master).UpdateOrderStatusByXFPaymentToComfirm(ToolHelper.ConventToDecimal(backQueryTradeStatusEntity.TradeMoney, 999), orderTradeInfo.TradeSysID, backQueryTradeStatusEntity.TradeStatus, out outOpenID))
                                {
                                    //WeChatAPI.SendMsg("付款审核成功", "您有一笔订单流水付款审核已通过，正等待仓库发货", outOpenID);
                                    return true;
                                }
                            }else if (backQueryTradeStatusEntity.TradeStatus == "40")
                            {
                                resMsg = "与XF系统扣款状态同步查询出错,错误信息：XF系统该笔流水号信息不存在";
                            }else if (backQueryTradeStatusEntity.TradeStatus == "30")
                            {
                                resMsg = "XF系统尚未同步，扣款尚未完成";
                            }
                        }else { resMsg = "XF系统返回的数据有误，返回的流水类型是退款流水"; }
                    } else{resMsg = "XF系统返回的解签数据出错";}
                }else { resMsg = "该流水号数据存在问题"; }
            }else { resMsg = "该流水号数据存在问题"; }
            return false;
        }


    }
}