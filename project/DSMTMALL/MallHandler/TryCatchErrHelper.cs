using DSMTMALL.Core.Common;
using DSMTMALL.Core.Common.MyEnum;
using System;

namespace MallHandler
{
    //这个类用来处理try-catch到错误信息后对数据进行操作
    public class TryCatchErrHelper
    {
        /// <summary>
        /// 执行捕获的错误代码方法
        /// </summary>
        /// <param name="pushType">推送给XF系统的方式（统一/分单）</param>
        /// <param name="pushOrderSn">推送给XF系统的订单编号(统一/分单)</param>
        public void HandlerError(string orderID, string orderSn,string errInfo)
        {
            try
            {
                DSMTMALL.DB.Model.M_OrderInfo orderInfo = new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Slave).GetModel<DSMTMALL.DB.Model.M_OrderInfo>("OrderID=@_OrderID" ,new { _OrderID=orderID });//因为有try-catch所以不再判断是否为空，如果为空就让他报异常
                if (orderInfo != null) {
                    if (!new DSMTMALL.DB.BLL.MY_Bll(DBEnum.Master).UpdateModel<DSMTMALL.DB.Model.M_OrderInfo>("OrderID=@_OrderID", new { OrderStatus = 7, Postscript = errInfo, Version = orderInfo.Version+1 }))
                    {
                        string mailContent = DateTime.Now + "订单编号：" + orderSn + "，严重警告数据库操作出错，错误信息：系统支付异常状态添加出错,请立即校对相应订单";
                        new DSMTMALL.DB.BLL.MB_Bll(DBEnum.Master).RecordErrInfoNote("同步程序链接数据库异常", mailContent);
                    } 
                 }
            }
            catch (Exception e)
            {
                string mailContent = DateTime.Now + "订单编号：" + orderSn + "，严重警告数据库操作出错，错误信息：" + e.Message + "，请立即校对相应订单";
                new DSMTMALL.DB.BLL.MB_Bll(DBEnum.Master).RecordErrInfoNote("同步程序链接数据库异常", mailContent);
            }
        }
        
        /// <summary>
        /// 流水异常更改流水状态
        /// </summary>
        /// <param name="tradeSysID"></param>
        public void HandlerTradeError(string tradeSysID)
        {
            new DSMTMALL.DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<DSMTMALL.DB.Model.M_OrderTrade>(" TradeSysID=@_TradeSysID ", new { _TradeSysID = tradeSysID , ComfirmStatus =90 });//流水异常
        }

        /// <summary>
        /// 退款流水异常更改流水状态
        /// </summary>
        /// <param name="tradeSysID"></param>
        public void HandlerReBackTradeError(string tradeSysID)
        {
            new DSMTMALL.DB.BLL.MB_Bll(DBEnum.Master).UpdateModel<DSMTMALL.DB.Model.M_ReBackTrade>(" ReBackTradeSysID=@_ReBackTradeSysID ", new { _TradeSysID = tradeSysID, ComfirmStatus = 90 });//流水异常
        }

    }
}
