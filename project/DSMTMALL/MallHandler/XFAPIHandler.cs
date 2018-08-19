using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using MallHandler.XFTAEAPI;
using System;
using System.Collections.Generic;
using System.IO;

namespace MallHandler
{
    public class XFAPIHandler
    {
        private static string logPathErr = "D:/DSMTMALL/" + DateTime.Now.ToString("yyyyMM/dd") + "errXFLog.txt";

        #region 同步订单扣款状态

        /// <summary>
        /// 调用XF系统接口进行交易扣款状态同步查询
        /// </summary>
        public static void GetOrderInfoPaymentStatusSync()
        {
            List<DSMTMALL.DB.Model.M_OrderTrade> orderTradeList = new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Slave).GetModelList<DSMTMALL.DB.Model.M_OrderTrade>(" ComfirmStatus=10 ORDER BY CreateTime ASC LIMIT 50 ", null);
            if (orderTradeList != null && orderTradeList.Count > 0)
            {
                DSMT_TAE_ServiceSoapClient taeCilent = new DSMT_TAE_ServiceSoapClient();
                GetQueryTradeStatusEntity queryTradeStatusEntity = null;
                VerifyHelper verifyHelper = new VerifyHelper();
                BackQueryTradeStatusEntity backQueryTradeStatusEntity = null;
                DateTime nowTime = DateTime.Now;
                string outOpenID = string.Empty;
                foreach (var item in orderTradeList)
                {
                    queryTradeStatusEntity = new GetQueryTradeStatusEntity();//初始化
                    backQueryTradeStatusEntity = null;
                    if (!string.IsNullOrEmpty(item.TradeNo) && !string.IsNullOrEmpty(item.CpySysID) && item.CreateTime.AddMinutes(10) < nowTime)//判断流水号公司ID都存在的情况下调用XF接口,并且流水号的创建时间超时10分钟
                    {
                        if (item.CreateTime.AddMinutes(360) > nowTime)//如果超过6个小时，XF系统还未同步完成，记录异常
                        {
                            queryTradeStatusEntity.CpySysID = item.CpySysID;
                            queryTradeStatusEntity.TradeNo = item.TradeNo;
                            verifyHelper.EncryptPmtEntity(queryTradeStatusEntity);//加密加签
                            try
                            {
                                backQueryTradeStatusEntity = taeCilent.QueryTradeStatus(queryTradeStatusEntity);//调用XF接口
                            }
                            catch (Exception e)//通讯异常
                            {
                                ToolHelper.WriteTxt(logPathErr, DateTime.Now + "流水号：" + item.TradeNo + "与XF系统扣款状态同步查询出错,错误信息：" + e.Message, false);
                                continue;
                            }
                            if (verifyHelper.CheckPmtSign(ref backQueryTradeStatusEntity))//尝试解签，解签失败的话不执行任何代码
                            {
                                if (backQueryTradeStatusEntity.TradeType == "0")//交易类型是0
                                {
                                    if (backQueryTradeStatusEntity.TradeStatus == "10" || backQueryTradeStatusEntity.TradeStatus == "20") //交易状态10-已完成20-已撤销(余额不足付款失败?)30-待同步40-交易信息不存在
                                    {
                                       if( new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Master).UpdateOrderStatusByXFPaymentToComfirm(ToolHelper.ConventToDecimal(backQueryTradeStatusEntity.TradeMoney,999),item.TradeSysID, backQueryTradeStatusEntity.TradeStatus,out outOpenID))
                                       {
                                            WeChatAPI.SendMsg("付款审核成功", "您有一笔订单流水付款审核已通过，正等待仓库发货", outOpenID);
                                       }
                                    }
                                    else if (backQueryTradeStatusEntity.TradeStatus == "40")
                                    {
                                        string resMsg = DateTime.Now + "流水号：" + item.TradeNo + "与XF系统扣款状态同步查询出错,错误信息：XF系统该笔流水号信息不存在";
                                        new DSMTMALL.DB.BLL.MB_Bll(DBEnum.Master).RecordErrInfoNote("同步程序查询XF系统扣款状态发现异常", resMsg);
                                        new TryCatchErrHelper().HandlerTradeError(item.TradeSysID);//写入错误日志后再更新流水状态为异常
                                        ToolHelper.WriteTxt(logPathErr, resMsg, false);
                                    }//30就不做任何处理
                                }
                            }
                        }
                        else
                        {
                            new DSMTMALL.DB.BLL.MB_Bll(DBEnum.Master).RecordErrInfoNote("同步程序查询XF系统扣款状态发现异常", "流水号：" + item.TradeNo + "与XF系统扣款状态同步查询已过6个小时，XF系统仍未同步完成");
                            new TryCatchErrHelper().HandlerTradeError(item.TradeSysID);//写入错误日志后再更新流水状态为异常
                        }
                    }
                }
            }
        }

        #endregion

        #region 同步订单退款状态

        /// <summary>
        /// 调用XF系统接口进行交易退款状态同步查询
        /// </summary>
        public static void GetReBackPayAccStatusSync()
        {
            List<DSMTMALL.DB.Model.M_ReBackTrade> orderTradeList = new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Slave).GetModelList<DSMTMALL.DB.Model.M_ReBackTrade>(" ComfirmStatus=10 ORDER BY CreateTime ASC LIMIT 50 ", null);
            if (orderTradeList != null && orderTradeList.Count > 0)
            {
                DSMT_TAE_ServiceSoapClient taeCilent = new DSMT_TAE_ServiceSoapClient();
                GetQueryTradeStatusEntity queryTradeStatusEntity = null;
                VerifyHelper verifyHelper = new VerifyHelper();
                BackQueryTradeStatusEntity backQueryTradeStatusEntity = null;
                DateTime nowTime = DateTime.Now;
                string outOpenID = string.Empty;
                foreach (var item in orderTradeList)
                {
                    queryTradeStatusEntity =new GetQueryTradeStatusEntity();//初始化
                    backQueryTradeStatusEntity = null;
                    if (!string.IsNullOrEmpty(item.TradeNo) && !string.IsNullOrEmpty(item.CpySysID) && item.CreateTime.AddMinutes(10) < nowTime)//判断流水号公司ID都存在的情况下调用XF接口,并且流水号的创建时间超时10分钟
                    {
                        if (item.CreateTime.AddMinutes(360) > nowTime)//如果超过6个小时，XF系统还未同步完成，记录异常
                        {
                            queryTradeStatusEntity.CpySysID = item.CpySysID;
                            queryTradeStatusEntity.TradeNo = item.ReBackTradeNo;
                            verifyHelper.EncryptPmtEntity(queryTradeStatusEntity);//加密加签
                            try
                            {
                                backQueryTradeStatusEntity = taeCilent.QueryTradeStatus(queryTradeStatusEntity);//调用XF接口
                            }
                            catch (Exception e)//通讯异常
                            {
                                ToolHelper.WriteTxt(logPathErr, DateTime.Now + "退款流水号：" + item.ReBackTradeNo + "与XF系统退款状态同步查询出错,错误信息：" + e.Message, false);
                                continue;
                            }
                            if (verifyHelper.CheckPmtSign(ref backQueryTradeStatusEntity))//尝试解签，解签失败的话不执行任何代码
                            {
                                if (backQueryTradeStatusEntity.TradeType == "1")
                                {
                                    if (backQueryTradeStatusEntity.TradeStatus == "10" || backQueryTradeStatusEntity.TradeStatus == "20") //交易状态10-已完成20-已撤销(即失败)30-待同步40-交易信息不存在
                                    {
                                        if( new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Master).UpdateReBackPayAccByXFPaymentToComfirm(ToolHelper.ConventToDecimal(backQueryTradeStatusEntity.TradeMoney, 999), item.ReBackTradeSysID, backQueryTradeStatusEntity.TradeStatus,out outOpenID))
                                        {
                                            WeChatAPI.SendMsg("退款资金到账通知", "您有一笔退款金额已通过原支付方式退还至您的账户,请注意查收", outOpenID);
                                        }
                                    }
                                    else if (backQueryTradeStatusEntity.TradeStatus == "40")
                                    {
                                        string resMsg = DateTime.Now + "退款流水号：" + item.ReBackTradeNo + "与XF系统退款状态同步查询出错,错误信息：XF系统该笔流水号信息不存在";
                                        new DSMTMALL.DB.BLL.MB_Bll(DBEnum.Master).RecordErrInfoNote("同步程序查询XF系统退款状态发现异常", resMsg);
                                        new TryCatchErrHelper().HandlerReBackTradeError(item.ReBackTradeSysID);
                                        ToolHelper.WriteTxt(logPathErr, resMsg, false);
                                    }//30就不做任何处理
                                }
                            }
                        }
                        else
                        {
                            new DSMTMALL.DB.BLL.MB_Bll(DBEnum.Master).RecordErrInfoNote("同步程序查询XF系统退款状态发现异常", "退款流水号：" + item.TradeNo + "与XF系统退款状态同步查询已过6个小时，XF系统仍未同步完成");
                            new TryCatchErrHelper().HandlerReBackTradeError(item.ReBackTradeSysID);
                        }
                    }
                }
            }
        }

        #endregion

    }
}
