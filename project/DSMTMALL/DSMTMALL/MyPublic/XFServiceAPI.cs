using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEntity;
using DSMTMALL.Core.Common.MyEnum;
using DSMTMALL.XFTAEAPI;
using System;
using System.Collections.Generic;
using System.Web;

namespace DSMTMALL.MyPublic
{
    
    public class XFServiceAPI
    {
        /// <summary>
        /// 调用查询用户余额的XF接口返回用户可用余额
        /// </summary>
        /// <param name="userInfo">用户信息实体类</param>
        /// <returns></returns>
        public static string GetUserCardBalanceInfo(DB.Model.M_Users userInfo)
        {
            DSMT_TAE_ServiceSoapClient taeCilent = new DSMT_TAE_ServiceSoapClient();
            GetQueryAccBalanceEntity balanceEntity = new GetQueryAccBalanceEntity();
            balanceEntity.OpenID = userInfo.OpenID;
            string outMoeny = "0";
            new VerifyHelper().EncryptPmtEntity(balanceEntity);//加密加签
            try
            {
                BackQueryAccBalanceEntity backBalanceEntity = taeCilent.QueryAccBalance(balanceEntity);
                if (new VerifyHelper().CheckPmtSign(ref backBalanceEntity)) //解密解签
                { //AccMoeny:个人余额 ； AccSubsidy:补贴余额 ； AccStatus：账户状态10-未启用20-补贴余额消费30-个人余额消费40-补贴和个人余额消费
                    if ((!string.IsNullOrEmpty(backBalanceEntity.CpySysID) && backBalanceEntity.CpySysID != userInfo.CpySysID )||(!string.IsNullOrEmpty(backBalanceEntity.SimpleName)&& backBalanceEntity.SimpleName!=userInfo.SimpleName))//如果返回的用户公司信息与当前用户公司信息不一致，更新用户信息
                    {
                        userInfo.CpySysID = backBalanceEntity.CpySysID;
                        userInfo.CpyName = backBalanceEntity.CpyName;
                        userInfo.SimpleName = backBalanceEntity.SimpleName;
                        new DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<DB.Model.M_Users>(" UserID=@_UserID ",new { _UserID=userInfo.UserID, CpySysID= userInfo.CpySysID, CpyName= userInfo.CpyName, SimpleName=userInfo.SimpleName });
                        HttpContext.Current.Session[WebLoginHelper.SESSION_ADMIN] = userInfo;//更新session
                    }
                    switch (backBalanceEntity.AccStatus)
                    {
                        case "20":
                            outMoeny = backBalanceEntity.AccSubsidy;
                            break;
                        case "30":
                            outMoeny = backBalanceEntity.AccMoney;
                            break;
                        case "40":
                            outMoeny = (Convert.ToDouble(backBalanceEntity.AccSubsidy) + Convert.ToDouble(backBalanceEntity.AccMoney)).ToString();
                            break;
                        default:
                            break;
                    }
                }
                return outMoeny;
            }
            catch
            {
                return outMoeny;
            }
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
        public static bool SetUserOrderToPayment(string pushType, string pushOrderSn, decimal pushOrderAmount,DateTime orderCreateTime,  out string resPayInfo)
        {
            resPayInfo = string.Empty;
            DSMT_TAE_ServiceSoapClient taeCilent = new DSMT_TAE_ServiceSoapClient();
            GetPayAccBalanceEntity payAccBanlanceEntity = new GetPayAccBalanceEntity();
            BackPayAccBalanceEntity backPayAccBanlanEntity = new BackPayAccBalanceEntity();
            VerifyHelper verifyHelper = new VerifyHelper();
            payAccBanlanceEntity.OpenID = WebLoginHelper.GetUserOpenID();
            payAccBanlanceEntity.UserPhone = WebLoginHelper.GetUserPhone();
            payAccBanlanceEntity.OrderMoney = pushOrderAmount.ToString();
            payAccBanlanceEntity.OrderTime =Convert.ToString(orderCreateTime);
            string logInfo = string.Empty;
            string temp = string.Empty;
            if (pushType == "orderSn")
            {
                string orderUnifySn = new SQLEntityHelper().GetOrderUnifySnByOrderSn(pushOrderSn);
                if (!string.IsNullOrEmpty(orderUnifySn))
                {
                    payAccBanlanceEntity.UnifyOrderNo = orderUnifySn;
                    payAccBanlanceEntity.OrderNo = pushOrderSn;
                }
                else
                {
                    resPayInfo = "系统忙，请稍后再试";
                    return false;
                }
            } else if (pushType == "unifySn")
            {
                payAccBanlanceEntity.UnifyOrderNo = pushOrderSn;
            } else
            {
                resPayInfo = "系统忙，请稍后再试";
                return false;
            }
            verifyHelper.EncryptPmtEntity(payAccBanlanceEntity);
            try
            {
                backPayAccBanlanEntity = taeCilent.PayAccBalance(payAccBanlanceEntity);//调xf接口
                if (verifyHelper.CheckPmtSign(ref backPayAccBanlanEntity))
                {
                    /// 交易状态10-支付成功20-已支付完成30-账户信息不存在40-账户不可用50-手机号不一致60-账户余额不足70-支付失败
                    if (backPayAccBanlanEntity != null && !string.IsNullOrEmpty(backPayAccBanlanEntity.TradeStatus))//XF系统返回的数据不为空，并且状态信息不为空
                    {
                        switch (backPayAccBanlanEntity.TradeStatus)
                        {
                            case "10":
                                if(UpdateOrderStatusByXFResultToPayment(backPayAccBanlanEntity, out temp))//更新订单状态信息为付款中并生成流水号
                                {
                                    return true;//执行数据库操作成功，直接返回true
                                }else
                                {
                                    resPayInfo = "支付错误，请联系客服";//返回给外面resWrite的
                                    logInfo = temp;
                                    new TryCatchErrHelper().Handler1001Error(pushType, pushOrderSn, logInfo);//执行错误代码1001的方法
                                    break;
                                }
                            case "20":
                                resPayInfo = "您已完成支付申请，请联系客服确认支付结果";
                                logInfo = "支付错误，系统检测到该笔订单已被XF系统支付，但当前订单状态仍可支付";
                                new TryCatchErrHelper().Handler1001Error(pushType, pushOrderSn, logInfo);//执行错误代码1001的方法
                                break;
                            case "30":
                                resPayInfo = "非法的操作，账户信息不存在";
                                break;
                            case "40":
                                resPayInfo = "非法的操作，此账户已被冻结";
                                break;
                            case "50":
                                resPayInfo = "非法的操作，系统检测到当前手机号与消费系统预留的手机号不一致";
                                break;
                            case "60":
                                resPayInfo = "支付失败，账户余额不足";
                                break;
                            case "70":
                                resPayInfo = "支付失败，贵公司规定该时段内不允许商城消费";
                                break;
                            case "80":
                                resPayInfo = "支付失败，请重新支付";
                                break;
                            default:
                                resPayInfo = "支付失败";
                                break;
                        }
                    }
                    else
                    {
                        resPayInfo = "结算中心资金结算错误，请联系客服";
                        logInfo = "从XF系统返回的数据为空，或者返回的流水号为空,订单编号："+pushOrderSn+",推送方式:"+pushType;
                        new TryCatchErrHelper().Handler1001Error(pushType, pushOrderSn, logInfo);//执行错误代码1001的方法
                    }
                }else//验签失败
                {
                    resPayInfo = "与结算中心通讯错误,请联系客服";
                    logInfo = "从XF系统返回的数据进行签名验证失败";
                    new TryCatchErrHelper().Handler1001Error(pushType, pushOrderSn, logInfo);//执行错误代码1001的方法
                }
            }
            catch (Exception e)
            {
                ToolHelper.WriteLogInfoToLocalText("推送类型：" + pushType + "，订单编号：" + pushOrderSn + "，与XF系统资金结算出错,错误信息：" + e.Message+"，自定义错误信息"+logInfo);
            }
            return false;
        }

        /// <summary>
        /// 更新订单状态信息为付款中并生成流水号
        /// </summary>
        /// <param name="backPayAccBanlanEntity"></param>
        /// <param name="feeAmount">实际要支付的价格</param>
        /// <returns></returns>
        public static bool UpdateOrderStatusByXFResultToPayment(BackPayAccBalanceEntity backPayAccBanlanEntity, out string logInfo)
        {
            List<DB.Model.M_OrderInfo> orderInfoList = new List<DB.Model.M_OrderInfo>();
            DB.Model.M_OrderInfo orderInfo = new DB.Model.M_OrderInfo();
            decimal amountCount = 0;
            logInfo = string.Empty;
            if (!string.IsNullOrEmpty(backPayAccBanlanEntity.OrderNo))//判断子订单ID是否为空或null，不为空代表是子订单支付的
            {
                orderInfo = new SQLEntityHelper().GetOrderInfoByOrderSn(backPayAccBanlanEntity.OrderNo);
                orderInfoList.Add(orderInfo);
            }else//是统一下单ID下单的
            {
                orderInfoList = new SQLEntityHelper().GetOrderInfoListByOrderUnifySn(backPayAccBanlanEntity.UnifyOrderNo);
            }
            //准备完成
            if (orderInfoList != null && orderInfoList.Count > 0)
            {
                foreach (var item in orderInfoList)
                {
                    amountCount += item.OrderAmount;
                }
                if (amountCount != Convert.ToDecimal(backPayAccBanlanEntity.OrderMoney))
                {
                    logInfo = "与消费系统校验金额存在差异，请检查消费系统的付款流水。订单流水号：" + backPayAccBanlanEntity.TradeNo + "订单编号：" + backPayAccBanlanEntity.UnifyOrderNo + " | " + backPayAccBanlanEntity.OrderNo + " | "+amountCount+" | "+backPayAccBanlanEntity.OrderMoney+"，与XF系统资金结算出错,错误信息：消费系统返回的订单金额对账不相等，请立即检查流水号相应订单";
                    throw new Exception("1001");//向上抛出自定义错误信息1001代表支付流程出错，让外面try-catch到错误信息后执行更新订单状态信息为异常操作
                }
                else//校对成功
                {
                    //开始事务
                    DB.Model.M_Payment paymentInfo = new DB.BLL.MY_Bll(DBEnum.Slave).GetModel<DB.Model.M_Payment>(" PayID=20 ",null);
                    if( new DB.BLL.MY_Bll(DBEnum.Master).UpdateOrderInfoFromXFPayment(orderInfoList, Convert.ToDecimal(backPayAccBanlanEntity.OrderMoney), backPayAccBanlanEntity.TradeNo, Convert.ToDateTime(backPayAccBanlanEntity.TradeTime), paymentInfo, backPayAccBanlanEntity.UnifyOrderNo, backPayAccBanlanEntity.OrderNo, WebLoginHelper.GetUserPhone(), backPayAccBanlanEntity.CpySysID, backPayAccBanlanEntity.CpyName))
                    {
                        return true;//如果更新订单信息成功，直接返回true
                    }else
                    {
                        logInfo = "数据库更新从消费系统返回的记录时发生错误，事务进行回滚，请客服介入，手动更改订单信息，执行订单支付通过操作。订单流水号：" + backPayAccBanlanEntity.TradeNo + "订单编号：" + backPayAccBanlanEntity.UnifyOrderNo + " | " + backPayAccBanlanEntity.OrderNo + "，数据库更新从消费系统返回的记录时发生错误，事务进行回滚，请客服介入，手动更改订单信息，执行订单支付通过操作";
                    }
                }
            }
            else//订单信息都查不到了，直接返回错误信息就行
            {
                logInfo = "获取到消费系统返回的数据内容后，查询本地订单信息不存在，请核实消费系统进行扣款的订单ID是否正确,订单流水号：" + backPayAccBanlanEntity.TradeNo + "订单编号：" + backPayAccBanlanEntity.UnifyOrderNo + " | " + backPayAccBanlanEntity.OrderNo + "，与XF系统资金结算出错,错误信息：消费系统返回的订单号找不到对应的订单信息，请立即检查流水号相应订单";
            }
            if (!string.IsNullOrEmpty(logInfo)) { ToolHelper.WriteLogInfoToLocalText(logInfo); }//判读日志不为空，写文本日志，否则不进行任何操作，最后返回false
            return false;
        }
        
    }

   


}